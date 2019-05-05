// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System.Collections.Generic;
using Agebull.Common.AppManage;
using Agebull.Common.OAuth;

#endregion

namespace Agebull.Common.OAuth
{
    /// <summary>
    ///     Ȩ�޲�������ע�����
    /// </summary>
    public interface IPowerChecker
    {
        /// <summary>
        ///     �����û��˺���Ϣ
        /// </summary>
        bool LoadAuthority(string page);

        /// <summary>
        /// ����ҳ��Ķ���
        /// </summary>
        void SavePageAction(int pageid, string name, string title, string action,string type);

        /// <summary>
        /// ���������û���Ϣ
        /// </summary>
        void ReloadLoginUserInfo();

        /// <summary>
        ///     ����ҳ������
        /// </summary>
        /// <param name="url">���������������url</param>
        /// <returns>ҳ������</returns>
        IPageItem LoadPageConfig(string url);

        /// <summary>
        ///     ����ҳ������İ�ť����
        /// </summary>
        /// <param name="loginUser">��¼�û�</param>
        /// <param name="page">ҳ��,����Ϊ��</param>
        /// <returns>��ť���ü���</returns>
        List<string> LoadPageButtons(ILoginUserInfo loginUser, IPageItem page);

        /// <summary>
        ///     ����ҳ������İ�ť����
        /// </summary>
        /// <param name="loginUser">��¼�û�</param>
        /// <param name="page">ҳ��</param>
        /// <param name="action">����</param>
        /// <returns>�Ƿ��ִ��ҳ�涯��</returns>
        bool CanDoAction(ILoginUserInfo loginUser, IPageItem page, string action);

        /// <summary>
        ///     �����û��Ľ�ɫȨ��
        /// </summary>
        /// <param name="loginUser">��¼�û�</param>
        /// <returns></returns>
        List<IRolePower> LoadUserPowers(ILoginUserInfo loginUser);

        /// <summary>
        ///     �����û��ĵ�ҳ��ɫȨ��
        /// </summary>
        /// <param name="loginUser">��¼�û�</param>
        /// <param name="page">ҳ��</param>
        /// <returns></returns>
        IRolePower LoadPagePower(ILoginUserInfo loginUser, IPageItem page);

        /// <summary>
        ///     �����û��Ĳ�ѯ��ʷ
        /// </summary>
        /// <param name="loginUser">��¼�û�</param>
        /// <param name="page">����ҳ��</param>
        /// <param name="args">��ѯ����</param>
        void SaveQueryHistory(ILoginUserInfo loginUser, IPageItem page, Dictionary<string, string> args);

        /// <summary>
        ///     ��ȡ�û��Ĳ�ѯ��ʷ
        /// </summary>
        /// <param name="loginUser">��¼�û�</param>
        /// <param name="page">����ҳ��</param>
        /// <returns>���ص��ǲ����ֵ��JSON��ʽ���ı�</returns>
        string LoadQueryHistory(ILoginUserInfo loginUser, IPageItem page);
    }

}