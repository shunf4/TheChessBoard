using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace TheChessBoard
{
    public delegate void LineProcessor(string line);

    public class StdIOHandler : INotifyPropertyChanged
    {
        //INotifyPropertyChanged 大法，将属性成员的改动直接与控件属性值建立联系（控件那边用一个DataBinding）
        #region INotifyPropertyChanged 成员
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        Process proc;

        LineProcessor lineProc;
        SynchronizationContext _context;

        AutoResetEvent outputWaitHandle = new AutoResetEvent(false);

        private bool _procStarted = false;
        public bool ProcStarted
        {
            get { return _procStarted; }
            set { _procStarted = value; NotifyPropertyChanged("ProcStarted"); }
        }

        public StdIOHandler(string ExecFileName, string ExecArguments, LineProcessor LineProc)
        {
            proc = new Process();
            lineProc = LineProc;

            _context = SynchronizationContext.Current;

            proc.StartInfo.FileName = ExecFileName;
            proc.StartInfo.Arguments = ExecArguments;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.CreateNoWindow = true;
            proc.EnableRaisingEvents = true;

            //当proc有行输出的时候（为什么一定要按行啊……flush不行吗……），加到尾部
            proc.OutputDataReceived += (sender, e) =>
            {
                if (e.Data == null)
                {
                    //DataReceivedEventArgs 的 Data成员为空即读取完毕
                    //waitHandle用于计时
                    outputWaitHandle.Set();
                }
                else
                {
                    lineProc(e.Data);
                }
            };

            proc.Exited += (sender, e) =>
            {
                ProcStarted = false;
                proc.CancelOutputRead();
            };
        }

        public void Start()
        {
            ProcStarted = proc.Start();

            proc.BeginOutputReadLine();
        }

        public void Stop()
        {
            proc.CancelOutputRead();
            ProcStarted = false;
        }

        public void Write(string str)
        {
            if(!ProcStarted)
            {
                throw new ApplicationException("Process not started.");
            }

            proc.StandardInput.WriteLine(str);
        }
    }
}
