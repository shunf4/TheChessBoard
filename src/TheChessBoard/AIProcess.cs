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
    /// <summary>
    /// 是一个委托（Delegate），当 AI 进程读取到一行输入时就调用绑定给 LineProcessorHandler 的所有函数
    /// </summary>
    /// <param name="line">读取到的那行输入。</param>
    public delegate void LineProcessorHandler(string line);

    /// <summary>
    /// 是一个委托，当 AI 进程退出时就调用绑定到它的所有函数
    /// </summary>
    public delegate void ProcessExitedHandler();

    /// <summary>
    /// 表示一个 AI 进程的类。存储了进程的可执行文件路径、参数、运行时间，提供 Start，Stop，WriteLine 等操作进程的方法，以及可绑定的关于进程读取到输入（LineProcess）和进程退出（ProcessExit）的两个事件。
    /// </summary>
    public class AIProcess
    {

        /// <summary>
        /// Process 对象，就是 AI 的进程。
        /// </summary>
        Process proc;

        /// <summary>
        /// LineProcess 事件。外部通过 AI.LineProcess += 函数方法名 来给这个事件绑定外部的触发函数。
        /// </summary>
        public event LineProcessorHandler LineProcess;

        /// <summary>
        /// ProcessExited 事件。
        /// </summary>
        public event ProcessExitedHandler ProcessExited;

        /// <summary>
        /// 一个等待句柄（WaitHandle），用来在等待 AI 进程产生输出（AllowOutputAndWait）时阻塞；当 AI 进程产生输出时，就会在 LineProcessorHandler 中解除阻塞。
        /// </summary>
        ManualResetEvent outputWaitHandle = new ManualResetEvent(false);
        /// <summary>
        /// 一个等待句柄，用来在 AI 进程已经产生输出，但是主程序还没有执行“等待 AI 进程输出”操作时阻塞住所有的 LineProcessorHandler。当主程序开始等待 AI 进程输出时，就会在 AllowOutputAndWait 中解除阻塞。
        /// </summary>
        ManualResetEvent allowMoveHandle = new ManualResetEvent(false);

        /// <summary>
        /// 一个秒表（Stopwatch），用来记录 AI 进程的运算时间。
        /// </summary>
        public Stopwatch Watch = new Stopwatch();

        /// <summary>
        /// 指示该进程的运行和终止状态。
        /// </summary>
        public bool ProcStarted { get; set; } = false;

        /// <summary>
        /// 该进程的文字描述。一般为“白方AI”或“黑方AI”。
        /// </summary>
        public string Description
        { get; private set; }

        /// <summary>
        /// 构造函数。将会建立 Process 对象，并赋给它一些值。
        /// </summary>
        /// <param name="ExecPath"></param>
        /// <param name="ExecArguments"></param>
        /// <param name="description"></param>
        /// <param name="createNoWindow"></param>
        public AIProcess(string ExecPath, string ExecArguments, string description, bool createNoWindow)
        {
            proc = new Process();

            Description = description ?? "Process";

            proc.StartInfo.FileName = ExecPath;
            proc.StartInfo.Arguments = ExecArguments;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.CreateNoWindow = createNoWindow;
            proc.EnableRaisingEvents = true;

            //尝试运行一下该 proc，以证明其有效性。
            proc.Start();
            if(!proc.HasExited)
            {
                proc.Kill();
            }

            //当proc有行输出的时候，做出反应
            proc.OutputDataReceived += (sender, e) =>
            {
                // 首先等待主程序开启 AllowOutputAndWait。
                allowMoveHandle.WaitOne();
                // 主程序开启了 AllowOutputAndWait 之后，解除 AllowOutputAndWait 中的阻塞。（所以，要不就是主程序等进程，要不就是进程等主程序。两个阻塞不可能同时发生，一个阻塞开始前必然解除另一个阻塞）
                outputWaitHandle.Set();
                // 因为进程还有可能继续给出输出，所以解除了之后要重新锁上
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

            // 当 proc 退出时，作出反应
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

        /// <summary>
        /// 解除输出处的阻塞，同时等待输出。在这过程中，计时。
        /// </summary>
        public void AllowOutputAndWait()
        {
            // 秒表启动
            Watch.Start();
            // 先解除输出处的阻塞
            allowMoveHandle.Set();
            // 等待输出处给 outputWaitHandle 解除阻塞（表示终于接收到了输入），能等待 600000 毫秒（10分钟）
            outputWaitHandle.WaitOne(600000);
            // 秒表停止
            Watch.Stop();
        }

        /// <summary>
        /// 主程序给进程写入一行输入
        /// </summary>
        /// <param name="str">要写入的输入。</param>
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
