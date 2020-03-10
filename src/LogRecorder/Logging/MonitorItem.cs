using System;
using System.Linq;
using System.Text;

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

        internal readonly StringBuilder Texter = new StringBuilder();

        internal DateTime Id = DateTime.Now;

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
            InMonitor = true;
#if !NETCOREAPP
            if (!AppDomain.MonitoringIsEnabled)
                AppDomain.MonitoringIsEnabled = true;
#endif
            {
                Stack.SetFix(new MonitorData
                {
                    Title = title
                });
                //Texter.Append("����");
                //Texter.Append(' ', 24);
                //Texter.Append("|״̬|   ʱ��   |   �� ʱ(ms)   |");
//#if !NETCOREAPP
//                Texter.Append("|  CPU(ms) |�ڴ����Kb| �ܷ���Mb |�ڴ�פ��Kb| ��פ��Mb |");
//#endif
                Write(title, ItemType.First);
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
            Write(title, ItemType.Begin);
        }

        /// <summary>
        ///     ˢ����Դ���
        /// </summary>
        public void Flush(string title, bool number = false)
        {
            Stack.Current.Coll();
            Stack.Current.FlushMessage();
            Write(title, ItemType.Begin);//BUG:
            Stack.Current.Flush();
        }

        /// <summary>
        ///     ˢ����Դ���
        /// </summary>
        public string End()
        {
            InMonitor = false;
            try
            {
                MonitorData pre = null;
                while (Stack.StackCount > 0)
                {
                    Stack.Current.Coll(pre);
                    Stack.Current.EndMessage();
                    Write(Stack.Current.Title, ItemType.End);
                    pre = Stack.Current;
                    Stack.Pop();
                }
                Stack.Current.Coll(pre);
                Stack.FixValue.EndMessage();
                Write(Stack.FixValue.Title ?? "Monitor", ItemType.End);
                return Texter.ToString();
            }
            catch (Exception ex)
            {
                LogRecorder.Error("EndMonitor{0}", ex);
                return null;
            }
            finally
            {
                Texter.Clear();
            }
        }

        internal enum ItemType
        {
            First,
            Begin,
            End,
            Item
        }
        internal void Write(string title, ItemType type, bool showMonitorValue = true)
        {
            Texter.AppendLine();
            var cnt = Stack.StackCount;
            switch (type)
            {
                case ItemType.First :
                case ItemType.Begin:
                    if (cnt > 0)
                    {
                        Texter.Append('��', cnt - 1);
                        Texter.Append("����");
                    }
                    else
                    {
                        Texter.Append('��');
                    }
                    break;
                //if (cnt > 0)
                //    Texter.Append('��', cnt);
                //Texter.Append('��');
                //break;
                case ItemType.End:
                    if (cnt > 1)
                        Texter.Append('��', cnt);
                    Texter.Append('��');
                    break;
                default:
                //case ItemType.Item:
                //    if (cnt > 0)
                //        Texter.Append('��', cnt);
                //    Texter.Append('��');
                //    break;
                    if (cnt > 0)
                        Texter.Append('��', cnt);
                    Texter.Append('��');
                    cnt++;
                    break;
            }

            if (string.IsNullOrWhiteSpace(title))
                title = "*";
            Texter.Append(title);
            if (!showMonitorValue)
            {
                return;
            }
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
            Write(Stack.Current.Title, ItemType.End);
            var pre = Stack.Current;
            Stack.Pop();
            Stack.Current.Coll(pre);
            Stack.Current.Flush();
        }

    }
}