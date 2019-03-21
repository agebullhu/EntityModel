// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.UI;
using Agebull.Common.Logging;
using Gboxt.Common.DataModel;
using Gboxt.Common.SystemModel;

#endregion

namespace Gboxt.Common.WebUI
{
    /// <summary>
    ///     ҳ��Ļ���
    /// </summary>
    public abstract class MyPageBase : Page
    {
        #region Ȩ�����
        
        /// <summary>
        ///     �Ƿ�ҳ���ڶ��������
        /// </summary>
        protected bool AllAction { get; set; }

        /// <summary>
        ///     �Ƿ񹫿�ҳ��
        /// </summary>
        protected bool IsPublicPage { get; set; }

        /// <summary>
        ///     ��ǰҳ��ڵ�����
        /// </summary>
        public IPageItem PageItem { get; private set; }

        /// <summary>
        ///     ��ǰҳ��Ȩ������
        /// </summary>
        public IRolePower PagePower { get; private set; }

        /// <summary>
        ///     ��ǰ�û��Ƿ��ѵ�¼�ɹ�
        /// </summary>
        protected bool UserIsLogin => LoginUser != null && BusinessContext.Current.LoginUserId > 0;

        /// <summary>
        ///     ��ǰ��¼�û�
        /// </summary>
        protected ILoginUser LoginUser => BusinessContext.Current.LoginUser;

        /// <summary>
        ///     ��ǰ��ҳ��(��Ӧ��������)
        /// </summary>
        protected string CurrentPageName { get; private set; }

        /// <summary>
        /// �����û��˺���Ϣ
        /// </summary>
        protected bool LoadAuthority()
        {
            BusinessContext.Current.PowerChecker.ReloadLoginUserInfo();
            if (!CheckLogined())
            {
                LogRecorder.MonitorTrace($"�Ƿ��û�:{Request.RawUrl}");
                return false;
            }
            LogRecorder.MonitorTrace($"��ǰ�û�:{LoginUser.RealName}({LoginUser.Id})**{LoginUser.RealName}");
            if (IsPublicPage)
            {
                LogRecorder.MonitorTrace("����ҳ��");
                PagePower = new SimpleRolePower
                {
                    Id = -1,
                    PageItemId = -1,
                    Power = 1,
                    RoleId = LoginUser.RoleId
                };
            }
            else
            {
                PageItem = BusinessContext.Current.PowerChecker.LoadPageConfig(CurrentPageName);
                if (PageItem == null)
                {
                    LogRecorder.MonitorTrace("�Ƿ�ҳ��");
                    return false;
                }
                if (PageItem.IsHide)
                {
                    LogRecorder.MonitorTrace("����ҳ��");
                    PagePower = new SimpleRolePower
                    {
                        Id = -1,
                        PageItemId = -1,
                        Power = 1,
                        RoleId = LoginUser.RoleId
                    };
                }
                else
                {
                    PagePower = BusinessContext.Current.PowerChecker.LoadPagePower(BusinessContext.Current.LoginUser, PageItem);
                    if (PagePower == null)
                    {
                        LogRecorder.MonitorTrace("�Ƿ�����");
                        return false;
                    }
                    LogRecorder.MonitorTrace("��Ȩ����");
                }
            }
            BusinessContext.Current.PageItem = PageItem;
            BusinessContext.Current.CurrentPagePower = PagePower;
            return true;
        }

        /// <summary>
        ///     ����¼���
        /// </summary>
        protected virtual bool CheckLogined()
        {
            return UserIsLogin;
        }

        /// <summary>
        ///     ��鶯���Ƿ�����
        /// </summary>
        protected virtual bool CheckCanDo()
        {
            return true;
        }

        /// <summary>
        /// ������ǰ���ʵ���ҳ��
        /// </summary>
        /// <returns></returns>
        private string GetFriendPageUrl()
        {
            var paths = Request.RawUrl.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var friendPage = "/";
            var index = 0;
            for (; index < paths.Length - 1; index++)
            {
                friendPage += paths[index] + "/";
            }
            friendPage += "Index.aspx";
            return friendPage;
        }

        #endregion

        #region ҳ�����봦��

        /// <summary>
        ///     ҳ������������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentPageName = GetFriendPageUrl();
            LogRecorder.BeginMonitor(CurrentPageName);
            LogRecorder.MonitorTrace(Request.Url.AbsolutePath);

            LogRequestInfo();
            try
            {
                ProcessRequest();
            }
            catch (Exception exception)
            {
                LogRecorder.EndStepMonitor();
                LogRecorder.BeginStepMonitor("Exception");
                LogRecorder.MonitorTrace(exception.Message);
                LogRecorder.Exception(exception);
                Debug.WriteLine(exception);
                OnFailed(exception);
                LogRecorder.EndStepMonitor();
            }
            LogRecorder.BeginStepMonitor("Result");
            OnResult();
            LogRecorder.EndStepMonitor();
            LogRecorder.EndMonitor();
        }
        private void LogRequestInfo()
        {
            if (!LogRecorder.LogMonitor)
                return;
            var args = new StringBuilder();
            args.Append("Headers:");
            foreach (var head in Request.Headers.AllKeys)
            {
                args.Append($"[{head}]{Request[head]}");
            }
            LogRecorder.MonitorTrace(args.ToString());
            args.Clear();
            args.Append("QueryString:");
            foreach (var head in Request.QueryString.AllKeys)
            {
                args.Append($"[{head}]{Request[head]}");
            }
            LogRecorder.MonitorTrace(args.ToString());
            args.Clear();
            args.Append("Form:");
            foreach (var head in Request.Form.AllKeys)
            {
                args.Append($"[{head}]{Request[head]}");
            }
            LogRecorder.MonitorTrace(args.ToString());
            args.Clear();
            args.Append("Cookies:");
            foreach (var head in Request.Cookies.AllKeys)
            {
                args.Append($"[{head}]{Request[head]}");
            }
            LogRecorder.MonitorTrace(args.ToString());
        }

        private void ProcessRequest()
        {
            LogRecorder.BeginStepMonitor("LoadAuthority");
            var canDo = LoadAuthority();
            LogRecorder.MonitorTrace(canDo.ToString());
            LogRecorder.EndStepMonitor();
            if (!canDo)
            {
                BusinessContext.Current.LastMessage = "�Ƿ�����";
                return;
            }
            LogRecorder.BeginStepMonitor("Prepare");
            OnPrepare();
            LogRecorder.EndStepMonitor();
            LogRecorder.BeginStepMonitor("CheckCanDo");
            canDo = IsPublicPage || CheckCanDo();
            LogRecorder.MonitorTrace(canDo.ToString());
            LogRecorder.EndStepMonitor();
            if (!canDo)
                return;
            LogRecorder.BeginStepMonitor("OnPageLoaded");
            OnPageLoaded();
            LogRecorder.EndStepMonitor();
        }

        /// <summary>
        ///     ҳ�洦��ǰ׼��
        /// </summary>
        protected abstract void OnPrepare();

        /// <summary>
        ///     ҳ�洦�����
        /// </summary>
        protected abstract void OnResult();

        /// <summary>
        ///     ҳ�������Ĵ���
        /// </summary>
        protected abstract void OnPageLoaded();

        /// <summary>
        ///     ҳ��ִ�г���Ĵ���
        /// </summary>
        protected virtual void OnFailed(Exception exception)
        {
        }

        #endregion

        #region ��������


        /// <summary>
        ///     ת��ҳ�����
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="action">ת������</param>
        protected void ConvertQueryString(string name, Action<string> action)
        {
            var val = Request[name];
            if (!string.IsNullOrEmpty(val))
            {
                action(val);
            }
        }

        /// <summary>
        ///     ��ǰ�����Ƿ�����������
        /// </summary>
        /// <param name="name">��������</param>
        /// <returns>�Ƿ�����������</returns>
        protected bool ContainsArgument(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            return Request[name] != null;
        }
        /// <summary>
        /// �������
        /// </summary>
        private readonly Dictionary<string, string> _args = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetArg(string name, string value)
        {
            if (_args.ContainsKey(name))
                _args[name] = value;
            else _args.Add(name, value);
        }
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetArg(string name, int value)
        {
            if (_args.ContainsKey(name))
                _args[name] = value.ToString();
            else _args.Add(name, value.ToString());
        }
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetArg(string name, object value)
        {
            if (_args.ContainsKey(name))
                _args[name] = value?.ToString();
            else _args.Add(name, value?.ToString());
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="name"></param>
        protected string GetArgValue(string name)
        {
            if (_args.ContainsKey(name))
                return _args[name];
            return Request[name];
        }

        /// <summary>
        ///     ��ȡҳ�����(�ı�)
        /// </summary>
        /// <param name="name">��������</param>
        /// <returns>�ı�</returns>
        protected string GetArg(string name)
        {
            var value = GetArgValue(name);
            if (value == null)
            {
                return null;
            }
            var vl = value.Trim();
            if (vl == "null")
            {
                return null;
            }
            if (value == "undefined")
            {
                return null;
            }
            return string.IsNullOrWhiteSpace(vl) ? null : vl;
        }

        /// <summary>
        ///     ������(�ı�),�������Ϊ�ջ򲻴���,��Ĭ��ֵ���
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="convert">ת������</param>
        /// <param name="def">Ĭ��ֵ</param>
        /// <returns>�ı�</returns>
        protected T GetArg<T>(string name, Func<string, T> convert, T def)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return def;
            }
            return convert(value);
        }


        /// <summary>
        ///     ������(����)
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="convert">ת������</param>
        /// <returns>����Ϊ�ջ򲻴���,���ز��ɹ�,���������convert����ֵ���п���</returns>
        protected bool GetArg(string name, Func<string, bool> convert)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return false;
            }
            return convert(value);
        }

        /// <summary>
        ///     ������(�ı�),�������Ϊ�ջ򲻴���,��Ĭ��ֵ���
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="def">Ĭ��ֵ</param>
        /// <returns>�ı�</returns>
        protected string GetArg(string name, string def)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return def;
            }
            return value;
        }

        /// <summary>
        ///     ��ȡҳ�����(��������)
        /// </summary>
        /// <param name="name">��������</param>
        /// <returns>��������,Ϊ����Ϊ��,��������Ҳ���תΪ�������ͽ������쳣</returns>
        protected DateTime? GetDateArg(string name)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return null;
            }
            return DateTime.Parse(value);
        }

        /// <summary>
        ///     ��ȡҳ�����(��������)
        /// </summary>
        /// <param name="name">��������</param>
        /// <returns>��������,Ϊ����ΪDateTime.MinValue,��������Ҳ���תΪ�������ͽ������쳣</returns>
        protected DateTime GetDateArg2(string name)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return DateTime.MinValue;
            }
            return DateTime.Parse(value);
        }

        /// <summary>
        ///     ��ȡҳ�����(��������)
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="def"></param>
        /// <returns>��������,Ϊ����Ϊ��,��������Ҳ���תΪ�������ͽ������쳣</returns>
        protected DateTime GetDateArg(string name, DateTime def)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return def;
            }
            return DateTime.Parse(value);
        }

        /// <summary>
        ///     ��ȡҳ�����int����
        /// </summary>
        /// <param name="name">��������</param>
        /// <returns>int����,Ϊ����Ϊ-1,��������Ҳ���תΪint���ͽ������쳣</returns>
        protected int GetIntArg(string name)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return -1;
            }
            return int.Parse(value);
        }

        /// <summary>
        ///     ��ȡҳ�����int����
        /// </summary>
        /// <param name="name">��������</param>
        /// <returns>int����,Ϊ����Ϊ-1,��������Ҳ���תΪint���ͽ������쳣</returns>
        protected int[] GetIntArrayArg(string name)
        {
            var value = GetArgValue(name);

            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return new int[0];
            }
            return value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        }

        /// <summary>
        ///     ��ȡҳ�����bool����
        /// </summary>
        /// <param name="name">��������</param>
        /// <returns>int����,Ϊ����Ϊ-1,��������Ҳ���תΪint���ͽ������쳣</returns>
        protected bool GetBoolArg(string name)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return false;
            }
            return value != "0" && (value == "1" || value == "yes" || bool.Parse(value));
        }

        /// <summary>
        ///     ��ȡҳ�����(int����),�������Ϊ�ջ򲻴���,��Ĭ��ֵ���
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="def">Ĭ��ֵ</param>
        /// <returns>int����,��������Ҳ���תΪint���ͽ������쳣</returns>
        protected int GetIntArg(string name, int def)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "NaN" || value == "undefined" || value == "null")
            {
                return def;
            }
            return int.Parse(value);
        }

        /// <summary>
        ///     ��ȡҳ�����(����),ģ�����ƶ�ȡ
        /// </summary>
        /// <param name="names">�������</param>
        /// <returns>���ƽ������ĵ�һ����Ϊ0������,��������ƴ����Ҳ���תΪint���ͽ������쳣</returns>
        protected int GetIntAnyArg(params string[] names)
        {
            return names.Select(p => GetIntArg(p, 0)).FirstOrDefault(re => re != 0);
        }

        /// <summary>
        ///     ��ȡҳ�����(decimal������)
        /// </summary>
        /// <param name="name">��������</param>
        /// <returns>decimal������,���δ��ȡֵ��Ϊ-1,��������Ҳ���תΪdecimal���ͽ������쳣</returns>
        protected decimal GetDecimalArg(string name)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return -1;
            }
            return decimal.Parse(value);
        }

        /// <summary>
        ///     ��ȡҳ�����(decimal������),�������Ϊ�ջ򲻴���,��Ĭ��ֵ���
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="def">Ĭ��ֵ</param>
        /// <returns>decimal������,��������Ҳ���תΪdecimal���ͽ������쳣</returns>
        protected decimal GetDecimalArg(string name, decimal def)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return def;
            }
            return decimal.Parse(value);
        }

        /// <summary>
        ///     ��ȡҳ�����(long������),�������Ϊ�ջ򲻴���,��Ĭ��ֵ���
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="def">Ĭ��ֵ</param>
        /// <returns>long������,��������Ҳ���תΪlong���ͽ������쳣</returns>
        protected long GetLongArg(string name, long def = -1)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return def;
            }
            return long.Parse(value);
        }

        #endregion

        #region ������չ

        /// <summary>
        /// ��HTML����
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ToHtmlParagraph(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? "<p class='memo_html'></p>"
                : $"<p class='memo_html'>{value.Replace("\n", "</p><p class='memo_html'>")}</p>";
        }

        /// <summary>
        /// ��HTML����
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ToHtml(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "" : value.Replace("\n", "<br\\>");
        }


        /// <summary>
        /// ��HTML����
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ToHtmlDate(DateTime value)
        {
            return value == DateTime.MinValue ? "" : value.ToString("yyyy��M��d��");
        }
        /// <summary>
        /// ��HTML����
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ToMoney(decimal value)
        {
            return value == 0M ? "" : value.ToChineseMoney();
        }

        #endregion
    }
}