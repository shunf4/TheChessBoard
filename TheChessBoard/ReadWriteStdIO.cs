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
    public class ReadWriteStdIO : INotifyPropertyChanged
    {
        //INotifyPropertyChanged 大法，将属性成员的改动直接与控件属性值建立联系（控件那边用一个DataBinding）
        #region INotifyPropertyChanged 成员
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        Process proc1;
        Process proc2;

        //两个输出的字符串记录
        StringBuilder output1 = new StringBuilder();
        StringBuilder output2 = new StringBuilder();

        //关于线程的变量
        AutoResetEvent outputWaitHandle1 = new AutoResetEvent(false);
        AutoResetEvent outputWaitHandle2 = new AutoResetEvent(false);
        //这个是为了保证线程安全做的
        private SynchronizationContext _context;

        private bool _procStarted = false;
        public bool procStarted
        {
            get { return _procStarted; }
            set { _procStarted = value; NotifyPropertyChanged(); }
        }

        public bool procNotStarted
        {
            get { return !_procStarted; }
        }

        //两个AppendText的包装，里面做了线程安全（用主线程的上下文来Post一个修改）
        private void AppendText1(string Data)
        {
            output1.AppendLine(Data);
            _context.Post(delegate
                {
                    NotifyPropertyChanged("outputStr1");
                }, null
            );
            
        }

        private void AppendText2(string Data)
        {
            output2.AppendLine(Data);
            _context.Post(delegate
            {
                NotifyPropertyChanged("outputStr2");
            }, null
            );
        }

        public string outputStr1
        {
            get
            {
                return output1.ToString();
            }
        }

        public string outputStr2
        {
            get
            {
                return output2.ToString();
            }
        }

        


        public void Init(string Exec1FileName, string Exec1Arguments, string Exec2FileName, string Exec2Arguments)
        {
            proc1 = new Process();
            proc2 = new Process();
            procStarted = false;
            
            //保存主线程的上下文
            _context = SynchronizationContext.Current;

            proc1.StartInfo.FileName = Exec1FileName;
            proc1.StartInfo.Arguments = Exec1Arguments;
            proc1.StartInfo.UseShellExecute = false;
            proc1.StartInfo.RedirectStandardInput = true;
            proc1.StartInfo.RedirectStandardOutput = true;
            proc1.StartInfo.CreateNoWindow = true;

            proc2.StartInfo.FileName = Exec2FileName;
            proc2.StartInfo.Arguments = Exec2Arguments;
            proc2.StartInfo.UseShellExecute = false;
            proc2.StartInfo.RedirectStandardInput = true;
            proc2.StartInfo.RedirectStandardOutput = true;
            proc2.StartInfo.CreateNoWindow = true;

            //当proc有行输出的时候（为什么一定要按行啊……flush不行吗……），加到尾部
            proc1.OutputDataReceived += (sender, e) =>
            {
                if (e.Data == null)
                {
                    //DataReceivedEventArgs 的 Data成员为空即读取完毕
                    //waitHandle用于计时
                    outputWaitHandle1.Set();
                }
                else
                {
                    AppendText1(e.Data);
                }
            };

            proc2.OutputDataReceived += (sender, e) =>
            {
                if (e.Data == null)
                {
                    outputWaitHandle2.Set();
                }
                else
                {
                    AppendText2(e.Data);
                }
            };

            proc1.Exited += (sender, e) =>
            {
                MessageBox.Show("x");
                if (proc2.HasExited)
                    procStarted = false;
            };

            proc2.Exited += (sender, e) =>
            {
                if (proc1.HasExited)
                    procStarted = false;
            };
        }

        public void Start()
        {
            if(proc1 == null || proc2 == null)
            {
                throw new ApplicationException("Process proc1 and/or proc2 not initialized.");
            }

            output1.Clear();
            output2.Clear();

            procStarted = proc1.Start();
            procStarted = procStarted && proc2.Start();

            proc1.BeginOutputReadLine();
            proc2.BeginOutputReadLine();

        }

        public void Stop()
        {
            proc1.CancelOutputRead();
            proc2.CancelOutputRead();
            if(!proc1.HasExited)
                proc1.Kill();
            if (!proc2.HasExited)
                proc2.Kill();
            procStarted = false;
        }

        public void WriteToProc2(string str)
        {
            if (proc1 == null || proc2 == null || !procStarted)
            {
                throw new ApplicationException("Process proc1 and/or proc2 not initialized.");
            }

            proc2.StandardInput.WriteLine(str);
        }
    }
}
