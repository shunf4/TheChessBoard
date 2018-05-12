using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

namespace TheChessBoard
{
    public delegate void WriteToLogHandler(String logText);

    class ChessBoardTraceListener : TraceListener
    {
        WriteToLogHandler LogFunc;
        public Tuple<int, int ,int> ColorCritical = new Tuple<int, int, int>(200,0,0);
        public Tuple<int, int ,int> ColorError = new Tuple<int, int, int>(255,0,0);
        public Tuple<int, int ,int> ColorWarning = new Tuple<int, int, int>(128,128,0);
        public Tuple<int, int ,int> ColorInformation = new Tuple<int, int, int>(0, 0, 128);
        public Tuple<int, int ,int> ColorSuccess = new Tuple<int, int, int>(0, 128, 0);
        public Tuple<int, int ,int> ColorUsual = new Tuple<int, int, int>(0, 0, 0);
        public ChessBoardTraceListener(WriteToLogHandler _logFunc)
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

    
    static class TheChessBoardStart
    {
        static TheChessBoard boardForm;

        [STAThread]
        static void Main()
        {
            //FormGame.LoadAIFilenames(@"..\..\..\TestConsoleApp1\bin\Debug\TestConsoleApp1.exe", "", "", "");
            
            Application.EnableVisualStyles();
            if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();

            boardForm = new TheChessBoard();
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new ChessBoardTraceListener(boardForm.AppendLog));
            Trace.AutoFlush = true;
            Trace.TraceInformation("日志组件开始运作");
            Application.Run(boardForm);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
