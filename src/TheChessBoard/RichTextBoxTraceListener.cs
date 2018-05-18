using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;

namespace TheChessBoard
{
    public delegate void WriteToLogHandler(String logText);

    /// <summary>
    /// 一个收集日志数据并显示的 TraceListener，主要功能就是把收集到的日志 展示在日志 RichTextBox 中。
    /// </summary>
    class RichTextBoxTraceListener : TraceListener
    {
        WriteToLogHandler LogFunc;
        public Tuple<int, int, int> ColorCritical = new Tuple<int, int, int>(200, 0, 0);
        public Tuple<int, int, int> ColorError = new Tuple<int, int, int>(255, 0, 0);
        public Tuple<int, int, int> ColorWarning = new Tuple<int, int, int>(128, 128, 0);
        public Tuple<int, int, int> ColorInformation = new Tuple<int, int, int>(0, 0, 128);
        public Tuple<int, int, int> ColorSuccess = new Tuple<int, int, int>(0, 128, 0);
        public Tuple<int, int, int> ColorUsual = new Tuple<int, int, int>(0, 0, 0);
        public RichTextBoxTraceListener(WriteToLogHandler _logFunc)
        {
            LogFunc = _logFunc;
        }

        public override void Write(string sth)
        {
            string tgtStr = String.Format(@"{{\rtf1\ansicpg936 {0}}}", sth.Replace(Environment.NewLine, @"\line "));
            LogFunc(tgtStr);
        }

        public override void WriteLine(string sth)
        {
            string tgtStr = String.Format(@"{{\rtf1\ansicpg936 {0}\line}}", sth.Replace(Environment.NewLine, @"\line "));
            LogFunc(tgtStr);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            string colorFormat = @"{{\rtf1\ansicpg936{{\colortbl;\red{0}\green{1}\blue{2};}}";
            string colorStr;
#pragma warning disable CS0219 // 变量已被赋值，但从未使用过它的值
            string eventTypeStr;
#pragma warning restore CS0219 // 变量已被赋值，但从未使用过它的值
            Tuple<int, int, int> colorType;
            switch (eventType)
            {
                case TraceEventType.Critical:
                    colorType = ColorCritical;
                    eventTypeStr = "[CRIT]";
                    break;
                case TraceEventType.Error:
                    colorType = ColorError;
                    eventTypeStr = "[EROR]";
                    break;
                case TraceEventType.Warning:
                    colorType = ColorWarning;
                    eventTypeStr = "[WARN]";
                    break;
                case TraceEventType.Information:
                    colorType = ColorInformation;
                    eventTypeStr = "[INFO]";
                    break;
                default:
                    colorType = ColorUsual;
                    eventTypeStr = "";
                    break;
            }
            colorStr = string.Format(colorFormat, colorType.Item1, colorType.Item2, colorType.Item3);
            string body = colorStr + DateTime.Now.ToString("HH:mm:ss - ") + @"\cf1 " + message.Replace(Environment.NewLine, @"\line ") + @"\cf0\line}}";
            LogFunc(body);
        }

        public void TraceSuccess(string message)
        {
            string colorFormat = @"{{\rtf1\ansicpg936{{\colortbl;\red{0}\green{1}\blue{2};}}";
            string colorStr;
            var colorType = ColorSuccess;
            colorStr = string.Format(colorFormat, colorType.Item1, colorType.Item2, colorType.Item3);
            string body = colorStr + DateTime.Now.ToString("HH:mm:ss - ") + @"\cf1 " + message.Replace(Environment.NewLine, @"\line ") + @"\cf0\line}}";
            LogFunc(body);
        }

    }

}
