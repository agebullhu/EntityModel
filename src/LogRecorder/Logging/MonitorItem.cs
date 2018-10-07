using System;
using System.Linq;
using System.Text;
using Agebull.Common.Base;

namespace Agebull.Common.Logging
{
    /// <summary>
    ///     ������Ϣ
    /// </summary>
    [Serializable]
    internal class MonitorItem
    {
        /// <summary>
        ///     ��¼��ջ
        /// </summary>
        internal LogStack<MonitorData> Stack = new LogStack<MonitorData>();

        public readonly StringBuilder Texter = new StringBuilder();


        /// <summary>
        ///     ��⿪��
        /// </summary>
        internal bool InMonitor;

        /// <summary>
        ///     ��ʼ�����Դ
        /// </summary>
        public void BeginMonitor(string title)
        {
            if (!InMonitor)
                Begin(title);
            else
                BeginStep(title);
        }


        private void Begin(string title)
        {
            Stack.SetFix(new MonitorData
            {
                Title = title
            });
            InMonitor = true;
#if !NETSTANDARD2_0
            if (!AppDomain.MonitoringIsEnabled)
                AppDomain.MonitoringIsEnabled = true;
#endif
            {
                Stack.SetFix(new MonitorData
                {
                    Title = title
                });
                Texter.AppendLine(
                    $"<<RequestId:{LogRecorder.GetRequestId()}>>*<<Machine:{LogRecorder.GetMachineName()}>>");
                Texter.Append(' ', 24);
                Texter.Append("����");
                Texter.Append(' ', 24);
                Texter.Append("|״̬|   ʱ��   |   �� ʱ(ms)   |");
#if !NETSTANDARD2_0
                Texter.Append("|  CPU(ms) |�ڴ����Kb| �ܷ���Mb |�ڴ�פ��Kb| ��פ��Mb |");
#endif
                Write(title, 0);
            }
        }

        /// <summary>
        ///     ˢ����Դ���
        /// </summary>
        internal void BeginStep(string title)
        {
            Stack.Push(new MonitorData
            {
                Title = title
            });
            Write(title, 1);
        }

        /// <summary>
        ///     ˢ����Դ���
        /// </summary>
        public void Flush(string title, bool number = false)
        {
            Stack.Current.Coll();
            Stack.Current.FlushMessage();
            Write(title, 4);
            Stack.Current.Flush();
        }

        /// <summary>
        ///     ˢ����Դ���
        /// </summary>
        public string End()
        {
            try
            {
                MonitorData pre = null;
                while (Stack.StackCount > 0)
                {
                    Stack.Current.Coll(pre);
                    Stack.Current.EndMessage();
                    Write(Stack.Current.Title, 2);
                    pre = Stack.Current;
                    Stack.Pop();
                }
                Stack.Current.Coll(pre);
                Stack.FixValue.EndMessage();
                Write(Stack.FixValue.Title ?? "Monitor", 3);
                return Texter.ToString();
            }
            catch (Exception ex)
            {
                LogRecorder.SystemTrace(LogLevel.Error, "EndMonitor", ex);
                return null;
            }
            finally
            {
                Texter.Clear();
                InMonitor = false;
            }
        }

        internal void Write(string title, int type, bool showMonitorValue = true)
        {
            Texter.AppendLine();
            var cnt = Stack.StackCount;
            switch (type)
            {
                case 0:
                case 1:
                    if (cnt > 0)
                        Texter.Append('��', cnt);
                    Texter.Append('��');
                    break;
                //if (cnt > 0)
                //    Texter.Append('��', cnt);
                //Texter.Append('��');
                //break;
                case 2:
                    if (cnt > 1)
                        Texter.Append('��', cnt - 1);
                    Texter.Append("����");
                    break;
                case 3:
                    if (cnt > 0)
                        Texter.Append('��', cnt);
                    Texter.Append('��');
                    break;
                default:
                    if (cnt > 0)
                        Texter.Append('��', cnt);
                    Texter.Append("����");
                    cnt++;
                    break;
            }

            if (string.IsNullOrWhiteSpace(title))
                title = "*";
            Texter.Append(title);
            if (!showMonitorValue) return;
            var l = cnt * 2 + title.GetLen();
            if (l < 50)
                Texter.Append(' ', 50 - l);
            Texter.Append(Stack.Current.Message);

        }


        /// <summary>
        ///     ˢ����Դ���
        /// </summary>
        public void EndStepMonitor()
        {
            if (Stack.FixValue == Stack.Current)
            {
                return;
            }
            Stack.Current.EndMessage();
            Write(Stack.Current.Title, 2);
            var pre = Stack.Current;
            Stack.Pop();
            Stack.Current.Coll(pre);
            Stack.Current.Flush();
        }

    }

    /// <summary>
    /// ���ݲ��跶Χ
    /// </summary>
    public class MonitorScope : ScopeBase
    {
        private bool _isStep;
        /// <summary>
        /// ���ɷ�Χ
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MonitorScope CreateScope(string name)
        {
            var scope= new MonitorScope
            {
                _isStep = LogRecorder.Data.InMonitor
            };
            if (LogRecorder.Data.InMonitor)
                LogRecorder.BeginStepMonitor(name);
            else
                LogRecorder.BeginMonitor(name);
            return scope;
        }

        /// <summary>
        /// ������Դ
        /// </summary>
        protected override void OnDispose()
        {
            if (_isStep)
                LogRecorder.EndStepMonitor();
            else
                LogRecorder.EndMonitor();
        }
    }

    /// <summary>
    /// ���ݲ��跶Χ
    /// </summary>
    public class MonitorStepScope : MonitorScope
    {

    }
}