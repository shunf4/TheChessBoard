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
    public delegate void LineProcessorHandler(string line);
    public delegate void ProcessExitedHandler();

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

        public event LineProcessorHandler LineProcess;
        public event ProcessExitedHandler ProcessExited;
        SynchronizationContext _context = SynchronizationContext.Current;

        public SynchronizationContext Context
        {
            get
            {
                return _context;
            }
            set
            {
                _context = value;
            }
        }

        AutoResetEvent outputWaitHandle = new AutoResetEvent(false);
        ManualResetEvent allowMoveHandle = new ManualResetEvent(false);

        public Stopwatch Watch = new Stopwatch();

        private bool _procStarted = false;
        public bool ProcStarted
        {
            get { return _procStarted; }
            set { _procStarted = value; NotifyPropertyChanged("ProcStarted"); }
        }

        public string Description
        { get; private set; }

        public StdIOHandler(string ExecPath, string ExecArguments, string description = null)
        {
            proc = new Process();

            Description = description ?? "Process";

            proc.StartInfo.FileName = ExecPath;
            proc.StartInfo.Arguments = ExecArguments;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.CreateNoWindow = false;
            proc.EnableRaisingEvents = true;

                //TODO : 异常退出的情况
                //TODO : 错误重走的情况
                //TODO : 日志
            proc.Start();
            if(!proc.HasExited)
            {
                proc.Kill();
            }

            //当proc有行输出的时候（为什么一定要按行啊……flush不行吗……），加到尾部
            proc.OutputDataReceived += (sender, e) =>
            {
                allowMoveHandle.WaitOne();
                outputWaitHandle.Set();
                allowMoveHandle.Reset();
                if (proc.HasExited)
                {
                    return;
                }
                if (e.Data == null)
                {
                }
                else
                {
                    LineProcess?.Invoke(e.Data);
                }
            };

            proc.Exited += (sender, e) =>
            {
                ProcStarted = false;
                try
                {
                    proc.CancelOutputRead();
                }
                catch (InvalidOperationException)
                {
                    Trace.TraceWarning("取消 " + Description + " 的输出流错误，可能原来未开启，或这一操作已经完成。");
                }
                ProcessExited?.Invoke();
            };
        }

        public void Start()
        {
            ProcStarted = proc.Start();
            allowMoveHandle.Reset();
            outputWaitHandle.Reset();
            Watch.Reset();
            proc.BeginOutputReadLine();
        }

        public void PrepareKill()
        {
            allowMoveHandle.Set();
            outputWaitHandle.Set();
            try
            {
                proc.CancelOutputRead();
            }
            catch (InvalidOperationException)
            {
                Trace.TraceWarning("取消 " + Description + " 的输出流错误，可能原来未开启，或这一操作已经完成。");
            }
            Watch.Stop();
            ProcStarted = false;
        }

        public void Kill()
        {
            PrepareKill();
            if(!proc.HasExited)
                proc.Kill();
        }

        public void AllowOutputAndWait()
        {
            Watch.Start();
            allowMoveHandle.Set();
            outputWaitHandle.WaitOne(600000);
            Watch.Stop();
        }

        public void WriteLine(string str)
        {
            if(!ProcStarted)
            {
                Trace.TraceWarning(Description + " 已经退出，无法再写入 " + str + "。");
                return;
            }

            proc.StandardInput.WriteLine(str);
        }

        public void Dispose()
        {
            ;
        }
    }
}
