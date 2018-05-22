using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessDotNet;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ChessDotNet.Pieces;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

// TODO : 开局棋盘显示将死问题
namespace TheChessBoard
{
    /// <summary>
    /// 是 MoreDetailedMove 的表项化，可以将一个 MoreDetailedMove 转化为只记录 Player、SAN 等字符串的表项。在“历史记录”中使用。
    /// </summary>
    public class MoreDetailedMoveImitator
    {
        public int Index { get; set; }
        public string PlayerString { get; set; }
        public string SANString { get; set; }
        public string FriendlyText { get; set; }
        public char[] BoardPrint { get; set; }
        public MoreDetailedMove AssociatedMoreDetailedMove { get; set; }
        public MoreDetailedMoveImitator(int index, MoreDetailedMove m, char[] boardPrint)
        {
            Index = index;
            switch (m.Player)
            {
                case Player.Black:
                    PlayerString = "黑方"; break;
                case Player.White:
                    PlayerString = "白方"; break;
                case Player.None:
                    PlayerString = " - "; break;
            }
            SANString = m.SANString;
            FriendlyText = m.FriendlyText;
            BoardPrint = boardPrint;
            AssociatedMoreDetailedMove = m;
        }

        public MoreDetailedMoveImitator(int index, Player player, string sanString, string friendlyText, char[] boardPrint)
        {
            Index = index;
            switch (player)
            {
                case Player.Black:
                    PlayerString = "黑方"; break;
                case Player.White:
                    PlayerString = "白方"; break;
                case Player.None:
                    PlayerString = " - "; break;
            }
            SANString = sanString;
            FriendlyText = friendlyText;
            BoardPrint = boardPrint;

        }
    }

    public enum SquareColor
    {
        SquareWhite,
        SquareBlack
    }

    /// <summary>
    /// 重要的状态量：ChessBoardGame 的控件状态。
    /// </summary>
    public enum ChessBoardGameControlState
    {
        NotStarted,     //游戏未开始，一些按钮不能操作。
        Idle,           //游戏空闲等待操作，可以按下一些按钮。
        Selected,       //玩家点击了一个棋盘格，棋盘格正产生高亮的状态。
        StdIORunning,   //AI 进程正在处理输出，一些按钮不能操作。
        Stopped         //游戏停止，一些按钮不能操作。
    }

    /// <summary>
    /// 重要的状态量：ChessBoardGame 的游戏进行（Procedure）状态。
    /// </summary>
    public enum ChessBoardGameProcedureState
    {
        NotStarted,
        Running,
        Stopped,
        WhiteWins,
        BlackWins,
        Draw
    }

    /// <summary>
    /// 重要的状态量：ChessBoardGame 的白方/黑方进程状态。
    /// </summary>
    public enum StdIOState
    {
        /// <summary>
        /// 没有载入进程文件。
        /// </summary>
        NotLoaded,
        /// <summary>
        /// 载入了进程文件，但是没有开始。
        /// </summary>
        NotStarted,
        /// <summary>
        /// 开始运行了，但是现在还不需要读它的输入。
        /// </summary>
        NotRequesting,
        /// <summary>
        /// 正在读取它的输入，阻塞中。
        /// </summary>
        Requesting,
    }

    /// <summary>
    /// AI 的类型，白或黑。
    /// </summary>
    public enum StdIOType
    {
        White,
        Black
    }

    /// <summary>
    /// 游戏的模式：未开启，双手动，半自动，全自动。
    /// </summary>
    public enum GameMode
    {
        NotStarted,
        Manual,
        WhiteAuto,
        BlackAuto,
        BothAuto
    }

    /// <summary>
    /// 上述的状态改变时可能要传一些参，因此将这些参数封装为 StatusUpdatedEventArgs。
    /// </summary>
    public class StatusUpdatedEventArgs : EventArgs
    {
        public StatusUpdatedEventArgs(bool updateImportant, string reason)
        {
            UpdateImportant = updateImportant;
            Reason = reason;
        }

        public StatusUpdatedEventArgs(bool updateImportant)
        {
            UpdateImportant = updateImportant;
            Reason = null;
        }

        public StatusUpdatedEventArgs()
        {
            UpdateImportant = false;
            Reason = null;
        }

        public string Reason { get; }
        public bool UpdateImportant { get; }
    }

    /// <summary>
    /// 当应用一个 Move 过后，需要触发的函数。
    /// </summary>
    public delegate void AppliedMoveEventHandler();
    /// <summary>
    /// 当游戏进行状态改变过后，需要触发的函数。
    /// </summary>
    /// <param name="e"></param>
    public delegate void GameProcedureStatusUpdatedEventHandler(StatusUpdatedEventArgs e);
    /// <summary>
    /// 当游戏控件状态改变过后，需要触发的函数。
    /// </summary>
    /// <param name="e"></param>
    public delegate void GameControlStatusUpdatedEventHandler(StatusUpdatedEventArgs e);
    /// <summary>
    /// 当有一方 AI 进程状态改变过后，需要触发的函数。
    /// </summary>
    /// <param name="e"></param>
    public delegate void PlayerIOStatusUpdatedEventHandler(StatusUpdatedEventArgs e);
    /// <summary>
    /// 从 TheChessBoard 传进来的 Invoke 函数，用这个 Invoke 函数来执行某些函数可以确保线程安全。
    /// </summary>
    /// <param name="g">用这个 Invoke 来执行的函数。</param>
    /// <returns></returns>
    public delegate object FormInvoke(Delegate g);

    /// <summary>
    /// 与 TheChessBoard（前台窗体）直接相关的，但是更偏重于游戏进程的逻辑状态而不是 UI 的象棋棋局的类。
    /// </summary>
    public class ChessBoardGameFormLogic : INotifyPropertyChanged
    {
        /// <summary>
        /// 秒表刷新间隔，默认为 17 毫秒。
        /// </summary>
        private static int _defaultWaitPeriod = 17;

        /// <summary>
        /// 从窗体 TheChessBoard 类处会给 FormInvoke 赋值为一个线程安全的函数 FormInvoke。
        /// FormInvoke 接受的参数是一个函数，可以以安全的形式（即用创建窗体 TheChessBoard 的主线程）来执行参数中的函数。
        /// 那么以后要做有关刷新 UI （或者有可能调用刷新 UI 的函数）的函数时，就统一交由 FormInvoke 来做。
        /// 因为如果在新线程中刷新 UI，会有不安全的因素出现。
        /// </summary>
        public FormInvoke FormInvoke;

        /// <summary>
        /// 无参构造，从默认的 _defaultGameCreationData 构造象棋游戏。
        /// </summary>
        public ChessBoardGameFormLogic() : this(_defaultGameCreationData)
        { }

        /// <summary>
        /// 从 gameCreationData 构造象棋游戏 Game。
        /// </summary>
        /// <param name="gameCreationData"></param>
        public ChessBoardGameFormLogic(GameCreationData gameCreationData)
        {
            Game = new ChessGame(gameCreationData);
            Init();
            WhiteStatus = StdIOState.NotLoaded;
            BlackStatus = StdIOState.NotLoaded;
        }


        #region 状态成员变量
        private ChessBoardGameControlState _controlStatus;
        private ChessBoardGameProcedureState _procedureStatus;
        private StdIOState _whiteStatus;
        private StdIOState _blackStatus;
        public GameMode Mode;

        public bool HasWhiteManuallyMoved { get; private set; }
        public bool HasBlackManuallyMoved { get; private set; }

        /// <summary>
        /// 窗体控件状态。
        /// </summary>
        public ChessBoardGameControlState ControlStatus
        {
            // get 访问器：当“取”这个成员的时候，要做的事。
            get { return _controlStatus; }
            // set 访问器：当“设置”这个成员的时候，要做的事。
            set
            {
                _controlStatus = value;
                Trace.TraceInformation("当前窗体控件状态：" + value.ToString());
                if (FormInvoke != null)
                    FormInvoke(new Action(delegate { GameControlStatusUpdated?.Invoke(new StatusUpdatedEventArgs()); }));
                else
                    GameControlStatusUpdated?.Invoke(new StatusUpdatedEventArgs());
            }
        }

        /// <summary>
        /// 游戏进行状态。
        /// </summary>
        public ChessBoardGameProcedureState ProcedureStatus
        {
            get { return _procedureStatus; }
            set
            {
                _procedureStatus = value;
                Trace.TraceInformation("当前游戏进行状态：" + value.ToString());
                if (FormInvoke != null)
                    FormInvoke(new Action(delegate { GameProcedureStatusUpdated?.Invoke(new StatusUpdatedEventArgs()); }));
                else
                    GameProcedureStatusUpdated?.Invoke(new StatusUpdatedEventArgs());
            }
        }

        /// <summary>
        /// 白 AI 状态。
        /// </summary>
        public StdIOState WhiteStatus
        {
            get { return _whiteStatus; }
            set
            {
                _whiteStatus = value;
                if (value != StdIOState.NotRequesting && value != StdIOState.Requesting)
                    Trace.TraceInformation("当前白 AI 状态：" + value.ToString());
                if (FormInvoke != null)
                    FormInvoke(new Action(delegate { PlayerIOStatusUpdated?.Invoke(new StatusUpdatedEventArgs()); }));
                else
                    PlayerIOStatusUpdated?.Invoke(new StatusUpdatedEventArgs());
            }
        }

        /// <summary>
        /// 黑 AI 状态。
        /// </summary>
        public StdIOState BlackStatus
        {
            get { return _blackStatus; }
            set
            {
                _blackStatus = value;
                if (value != StdIOState.NotRequesting && value != StdIOState.Requesting)
                    Trace.TraceInformation("当前黑 AI 状态：" + value.ToString());
                if (FormInvoke != null)
                    FormInvoke(new Action(delegate { PlayerIOStatusUpdated?.Invoke(new StatusUpdatedEventArgs()); }));
                else
                    PlayerIOStatusUpdated?.Invoke(new StatusUpdatedEventArgs());
            }
        }
        #endregion

        /// <summary>
        /// 初始化或重新初始化这个 ChessBoardGame 的逻辑状态。
        /// </summary>
        void Init()
        {
            Trace.TraceInformation("棋盘游戏开始初始化。");
            SetControlStatus(ChessBoardGameControlState.NotStarted, true);
            SetProcedureStatus(ChessBoardGameProcedureState.NotStarted, true);
            HasWhiteManuallyMoved = false;
            HasBlackManuallyMoved = false;
            Mode = GameMode.NotStarted;
            if(_updateUIDoneAfterMoveLocks != null)
                // 把所有因更新 UI 导致的阻塞全部解除。
                foreach (var uiLock in _updateUIDoneAfterMoveLocks)
                    uiLock.Set();
            if (_waitWatchChessLocks != null)
                // 把秒表更新之前的阻塞解除。
                foreach (var waitLock in _waitWatchChessLocks)
                    waitLock.Set();
            
            // 清空这些阻塞锁。
            _updateUIDoneAfterMoveLocks = new List<ManualResetEvent>();
            _waitWatchChessLocks = new List<ManualResetEvent>();
            _allThreadsDoneLocks = new List<ManualResetEvent>();
            _updateWatchLoopLock.Reset();

            // 清空游戏历史步骤。
            GameMoves = new BindingList<MoreDetailedMoveImitator>();
            GameMoves.Add(new MoreDetailedMoveImitator(0, Player.None, " - ", "开局", BoardPrint));

            // 清空历史吃子。
            WhiteCapturedPieces = "";
            BlackCapturedPieces = "";

            InvokeAllUpdates();
            Trace.TraceInformation("棋盘游戏初始化完成。");
        }

        /// <summary>
        /// 以模式 mode 开始这个象棋游戏。
        /// </summary>
        /// <param name="mode">要选择的模式。</param>
        public void Start(GameMode mode)
        {
            if (WhiteStatus == StdIOState.NotStarted)
            {
                ProcessWhiteStart();
            }
            if (BlackStatus == StdIOState.NotStarted)
            {
                ProcessBlackStart();
            }
            SetControlStatus(ChessBoardGameControlState.Idle, true);
            SetProcedureStatus(ChessBoardGameProcedureState.Running, true);
            Mode = mode;
            InvokeAllUpdates();

            foreach (var x in new List<TraceListener>(Trace.Listeners.Cast<TraceListener>()).Where((x) => x is RichTextBoxTraceListener))
                ((RichTextBoxTraceListener)(x)).TraceSuccess("游戏开始，模式：" + Mode.ToString());

            // 游戏开始时，有可能先手是自动模式，所以要立即触发读取下一个走子。
            InvokeNextMoveRequest(WhoseTurn, null);
        }

        /// <summary>
        /// 强制停止这个游戏。
        /// </summary>
        public void Stop()
        {
            Trace.TraceInformation("游戏正在停止，清理残余进程……");
            _whiteStatus = _whiteStatus == StdIOState.NotLoaded ? StdIOState.NotLoaded : StdIOState.NotRequesting;
            _blackStatus = _blackStatus == StdIOState.NotLoaded ? StdIOState.NotLoaded : StdIOState.NotRequesting;
            _controlStatus = ChessBoardGameControlState.Stopped;
            _procedureStatus = ChessBoardGameProcedureState.Stopped;

            // 解除所有阻塞锁。
            foreach (var uiLock in _updateUIDoneAfterMoveLocks)
                uiLock.Set();
            foreach (var waitLock in _waitWatchChessLocks)
                waitLock.Set();
            _updateWatchLoopLock.Set();

            Thread.Sleep(200);
            if (_allThreadsDoneLocks.Count > 0)
            //WaitHandle.WaitAll(_allThreadsDoneLocks.Where((m) => m != null).ToArray(), 1000);   //Time
            {
                try
                {
                    lock (_allThreadsDoneLocks)
                    {
                        foreach (var m in _allThreadsDoneLocks)
                        {
                            // 逐个等待进程结束
                            m?.WaitOne(1000);
                        }
                    }
                }
                catch (InvalidOperationException e)
                {
                    Trace.TraceError("清理进程错误：" + e.Message + Environment.NewLine + e.StackTrace);
                }
            }
            Trace.TraceInformation("残余进程清理完成。");
            WhiteStatus = _whiteStatus == StdIOState.NotLoaded ? StdIOState.NotLoaded : StdIOState.NotRequesting;
            BlackStatus = _blackStatus == StdIOState.NotLoaded ? StdIOState.NotLoaded : StdIOState.NotRequesting;
            ControlStatus = ChessBoardGameControlState.Stopped;
            SetProcedureStatus(ChessBoardGameProcedureState.Stopped, true, "游戏被终止。");

            System.Windows.Forms.Application.DoEvents();
        }

        /// <summary>
        /// 强行终止游戏，并将游戏的各状态复位。和 Stop 的逻辑差不多。
        /// </summary>
        public void KillAllAndResetStatus()
        {
            _whiteStatus = _whiteStatus == StdIOState.NotLoaded ? StdIOState.NotLoaded : StdIOState.NotStarted;
            _blackStatus = _blackStatus == StdIOState.NotLoaded ? StdIOState.NotLoaded : StdIOState.NotStarted;
            _controlStatus = ChessBoardGameControlState.NotStarted;
            _procedureStatus = ChessBoardGameProcedureState.NotStarted;
            foreach (var uiLock in _updateUIDoneAfterMoveLocks)
                uiLock.Set();
            foreach (var waitLock in _waitWatchChessLocks)
                waitLock.Set();

            _updateWatchLoopLock.Set();

            Thread.Sleep(200);

            if (_allThreadsDoneLocks.Count > 0)
            {
                try
                {
                    lock (_allThreadsDoneLocks)
                    {
                        foreach (var m in _allThreadsDoneLocks)
                        {
                            m?.WaitOne(1000);
                        }
                    }
                }
                catch (InvalidOperationException e)
                {
                    Trace.TraceError("清理进程错误：" + e.Message + Environment.NewLine + e.StackTrace);
                }
            }

            WhiteAIProcess?.Kill();
            BlackAIProcess?.Kill();

            WhiteStatus = _whiteStatus == StdIOState.NotLoaded ? StdIOState.NotLoaded : StdIOState.NotStarted;
            BlackStatus = _blackStatus == StdIOState.NotLoaded ? StdIOState.NotLoaded : StdIOState.NotStarted;
            ControlStatus = ChessBoardGameControlState.NotStarted;
            ProcedureStatus = ChessBoardGameProcedureState.NotStarted;
        }

        /// <summary>
        /// 以 gameCreationData 重设游戏。
        /// </summary>
        /// <param name="gameCreationData"></param>
        public void ResetGame(GameCreationData gameCreationData)
        {
            Trace.TraceInformation("正重置游戏，残余进程清理中……");
            KillAllAndResetStatus();
            Trace.TraceInformation("残余进程清理完成。");

            Game = new ChessGame(gameCreationData);
            Init();
            InvokeAllUpdates();
        }

        /// <summary>
        /// 以默认的 gameCreationData 重设游戏。
        /// </summary>
        public void ResetGame()
        {
            ResetGame(_defaultGameCreationData);
        }

        /// <summary>
        /// 刷新一遍所有的状态。
        /// 因为这几个状态都是挂钩到主窗体上的文字、和控件是否可用上的，触发这个函数可以强制调用这几个状态的更新函数，从而刷新主窗体。
        /// </summary>
        /// <param name="updateImportant"></param>
        public void InvokeAllUpdates(bool updateImportant = false)
        {
            var commonEventArgs = new StatusUpdatedEventArgs(updateImportant);
            if (FormInvoke != null)
            {
                FormInvoke(new Action(delegate { GameProcedureStatusUpdated?.Invoke(commonEventArgs); }));
                FormInvoke(new Action(delegate { GameControlStatusUpdated?.Invoke(commonEventArgs); }));
                FormInvoke(new Action(delegate { PlayerIOStatusUpdated?.Invoke(commonEventArgs); }));
            }
            else
            {
                GameProcedureStatusUpdated?.Invoke(commonEventArgs);
                GameControlStatusUpdated?.Invoke(commonEventArgs);
                PlayerIOStatusUpdated?.Invoke(commonEventArgs);
            }
            NotifyPropertyChanged(new[] { "BoardPrint", "WhoseTurn", "CareWhoseTurnItIs", "WhiteStopwatchTime", "BlackStopwatchTime" }, null);
        }




        #region 与线程事件有关的成员对象（线程锁、各种EventHandler）
        /// <summary>
        /// 秒表更新有关的锁，当它阻塞的时候就一直刷新秒表。当它放通的时候，计时就已经结束，不需要刷新秒表了。
        /// </summary>
        ManualResetEvent _updateWatchLoopLock = new ManualResetEvent(false);

        /// <summary>
        /// UI 更新有关的锁。当 UI 在更新的时候，这个锁就一直阻塞，不在 ApplyMove 中自动触发读取下一个走子的操作。当 UI 最终更新完毕，这个锁放行，允许读取下一个走子。
        /// </summary>
        List<ManualResetEvent> _updateUIDoneAfterMoveLocks;

        /// <summary>
        /// 当主线程每启动一个新的线程时，这个线程就会设立一个等待阻塞锁，并将这个锁插入到 _allThreadsDoneLocks 中，直到线程运行完成才解除这个锁的阻塞。这样，在需要对游戏进行“停止”或“重置”操作时，就可以等待里面的锁全部解除阻塞才开始重置，这样比较安全。
        /// </summary>
        List<ManualResetEvent> _allThreadsDoneLocks;
        /// <summary>
        /// 观棋锁。这个锁放行时，允许读取下一个走子。
        /// </summary>
        List<ManualResetEvent> _waitWatchChessLocks;


        /// <summary>
        /// 当这个类里面的某些属性改变过后，需要触发的事件。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 当游戏进行状态改变过后，需要触发的事件。
        /// </summary>
        public event GameProcedureStatusUpdatedEventHandler GameProcedureStatusUpdated;
        /// <summary>
        /// 当窗体控件状态改变过后，需要触发的事件。
        /// </summary>
        public event GameControlStatusUpdatedEventHandler GameControlStatusUpdated;
        /// <summary>
        /// 当有一方 AI 的状态发生改变时，需要触发的事件。
        /// </summary>
        public event PlayerIOStatusUpdatedEventHandler PlayerIOStatusUpdated;




        /// <summary>
        /// 触发属性更新事件：只触发一个属性更新事件。
        /// </summary>
        /// <param name="propertyName">触发更新的属性名。</param>
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (FormInvoke != null)
            {
                FormInvoke(new Action(delegate { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }));
            }
            else
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            Application.DoEvents();
        }

        /// <summary>
        /// 触发属性更新事件：触发多个属性更新事件。
        /// </summary>
        /// <param name="propertyNames">触发更新的属性名。</param>
        /// <param name="uiLock">由于只在一个地方用到多属性更新（ApplyMove），而且需要用到 uiLock，所以就加为了参数。
        /// 由于触发更新属性要做的操作大多数是更新 UI，所以 uiLock 是作为一个阻塞锁存在的，当更新完 UI 之后解除阻塞。</param>
        private void NotifyPropertyChanged(String[] propertyNames, ManualResetEvent uiLock)
        {
            if (PropertyChanged == null) return;
            if (FormInvoke != null)
            {
                FormInvoke(new Action(delegate
                {
                    foreach (var propertyName in propertyNames)
                        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                    // 解除 UI 阻塞锁。
                    uiLock?.Set();
                }));
            }
            else
            {
                foreach (var propertyName in propertyNames)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                uiLock?.Set();
            }
            // 这句我也不太清楚，大概就是刚才做了一堆操作，现在触发这个函数就是让系统把积压的操作全部做完。
            System.Windows.Forms.Application.DoEvents();
        }


        #endregion

#region 和两个AI进程有关的成员对象
        /// <summary>
        /// 白 AI 进程。
        /// </summary>
        public AIProcess WhiteAIProcess;
        /// <summary>
        /// 黑 AI 进程。
        /// </summary>
        public AIProcess BlackAIProcess;

        // 下面几个自行体会。

        public string WhiteStopwatchTime
        {
            get
            {
                if (WhiteAIProcess == null)
                {
                    return "--:--.-------";
                }
                var t = WhiteAIProcess.Watch.Elapsed;
                return string.Format("{0}:{1}", Math.Floor(t.TotalMinutes), t.ToString("ss\\.fffffff"));
            }
        }

        public string WhiteStopwatchStatus
        {
            get
            {
                if (WhiteAIProcess == null)
                {
                    return "No";
                }
                return WhiteAIProcess.Watch.IsRunning ? "Yes" : "No";
            }
        }

        public string BlackStopwatchTime
        {
            get
            {
                if (BlackAIProcess == null)
                {
                    return "--:--.-------";
                }
                var t = BlackAIProcess.Watch.Elapsed;
                return string.Format("{0}:{1}", Math.Floor(t.TotalMinutes), t.ToString("ss\\.fffffff"));
            }
        }

        public string BlackStopwatchStatus
        {
            get
            {
                if (BlackAIProcess == null)
                {
                    return "No";
                }
                return BlackAIProcess.Watch.IsRunning ? "Yes" : "No";
            }
        }


        /// <summary>
        /// 刷新秒表用的函数。在 _updateWatchLoopLock 解除阻塞之前，一直循环，通过 NotifyPropertyChanged 通知主窗体更新秒表 label。
        /// </summary>
        /// <param name="obj">秒表的计时字符串</param>
        void ThreadUpdateUIWatchHelper(object obj)
        {
            if (ProcedureStatus != ChessBoardGameProcedureState.Running)
                return;
            var mre = new ManualResetEvent(false);
            _allThreadsDoneLocks.Add(mre);

            string stopwatchTimeUpdateString = (string)obj;

            this._updateWatchLoopLock.Reset();

            while (true)
            {
                NotifyPropertyChanged(stopwatchTimeUpdateString);
                if (this._updateWatchLoopLock.WaitOne(_defaultWaitPeriod))
                {
                    break;
                }
            }

            mre.Set();
            try { _allThreadsDoneLocks.Remove(mre); }
            catch (Exception e)
            {
                Trace.TraceError("向 _allThreadsDoneLocks 移除锁错误：" + e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        #endregion

        #region 和当前游戏的窗体状态、游戏进行状态有关的成员
        
        /// <summary>
        /// 也是设置 ProcedureStatus，和它的 set 访问器的不同点在于多了一些可传递的选项。
        /// </summary>
        /// <param name="pState">要设置的状态。</param>
        /// <param name="updateImportant">这一设置是否是“重要的”。</param>
        /// <param name="reason">设置的原因。</param>
        public void SetProcedureStatus(ChessBoardGameProcedureState pState, bool updateImportant, string reason = null)
        {
            _procedureStatus = pState;
            Trace.TraceInformation("当前游戏进行状态：" + (updateImportant ? @"\b " : "") + pState.ToString() + (updateImportant ? @"\b0 " : "") + "，" + @"因为：" + (reason != "" && reason != null ? reason : "无"));
            if (FormInvoke != null)
                FormInvoke(new Action(delegate { GameProcedureStatusUpdated?.Invoke(new StatusUpdatedEventArgs(updateImportant, reason)); }));
            else
                GameProcedureStatusUpdated?.Invoke(new StatusUpdatedEventArgs(updateImportant, reason));
        }

        /// <summary>
        /// 也是设置 ControlStatus，和它的 set 访问器的不同点在于多了一些可传递的选项。
        /// </summary>
        /// <param name="cState"></param>
        /// <param name="updateImportant"></param>
        /// <param name="reason"></param>
        public void SetControlStatus(ChessBoardGameControlState cState, bool updateImportant, string reason = null)
        {
            _controlStatus = cState;
            Trace.TraceInformation("当前窗体控件状态：" + (updateImportant?@"\b ":"") + cState.ToString() + (updateImportant ? @"\b0 " : "") + "，" + @"因为：" + (reason != "" && reason != null ? reason : "无"));
            if (FormInvoke != null)
                FormInvoke(new Action(delegate { GameControlStatusUpdated?.Invoke(new StatusUpdatedEventArgs(updateImportant, reason)); }));
            else
                GameControlStatusUpdated?.Invoke(new StatusUpdatedEventArgs(updateImportant, reason));
            //System.Windows.Forms.Application.DoEvents();
        }

        /// <summary>
        /// 判断游戏是否终止，如果终止的话就更新 ProcedureStatus。
        /// </summary>
        public void UpdateProcedureStatusIfGameEnds()
        {
            bool whiteWins = Game.IsCheckmated(Player.Black);
            bool blackWins = Game.IsCheckmated(Player.White);
            bool drawWhiteStalemate = Game.IsStalemated(Player.White);
            bool drawBlackStalemate = Game.IsStalemated(Player.Black);
            bool drawFifty = Game.FiftyMovesAndThisCanResultInDraw;
            if (whiteWins || blackWins || drawBlackStalemate || drawWhiteStalemate || drawFifty)
            {

                string resultStr;
                ChessBoardGameProcedureState pState;
                if (whiteWins)
                {
                    resultStr = ("黑方被将死。白方胜！");
                    pState = ChessBoardGameProcedureState.WhiteWins;
                }
                else if (blackWins)
                {
                    resultStr = ("白方被将死。黑方胜！");
                    pState = ChessBoardGameProcedureState.BlackWins;
                }
                else if (drawWhiteStalemate)
                {
                    resultStr = ("白方陷入僵局。和局！");
                    pState = ChessBoardGameProcedureState.Draw;
                }
                else if (drawBlackStalemate)
                {
                    resultStr = ("黑方陷入僵局。和局！");
                    pState = ChessBoardGameProcedureState.Draw;
                }
                else if (drawFifty)
                {
                    resultStr = ("50 回合内无走兵或吃子动作。和局！");
                    pState = ChessBoardGameProcedureState.Draw;
                }
                else
                {
                    throw new Exception();
                }

                SetProcedureStatus(pState, updateImportant : true, reason : resultStr);
            }
        }
        #endregion

        #region 和象棋的规则、走子动作有关的成员。

        #region 一些默认的值的定义
        private static readonly Dictionary<char, Piece> FenMappings = new Dictionary<char, Piece>()
        {
            { 'K', new King(Player.White) },
            { 'k', new King(Player.Black) },
            { 'Q', new Queen(Player.White) },
            { 'q', new Queen(Player.Black) },
            { 'R', new Rook(Player.White) },
            { 'r', new Rook(Player.Black) },
            { 'B', new Bishop(Player.White) },
            { 'b', new Bishop(Player.Black) },
            { 'N', new Knight(Player.White) },
            { 'n', new Knight(Player.Black) },
            { 'P', new Pawn(Player.White) },
            { 'p', new Pawn(Player.Black) },
        };

        static readonly Dictionary<Tuple<char, SquareColor>, char> fenAndSquareColorMappings = new Dictionary<Tuple<char, SquareColor>, char>()
        {
            {new Tuple<char, SquareColor>('K', SquareColor.SquareWhite), 'k' },
            {new Tuple<char, SquareColor>('K', SquareColor.SquareBlack), 'K' },
            {new Tuple<char, SquareColor>('k', SquareColor.SquareWhite), 'l' },
            {new Tuple<char, SquareColor>('k', SquareColor.SquareBlack), 'L' },

            {new Tuple<char, SquareColor>('Q', SquareColor.SquareWhite), 'q' },
            {new Tuple<char, SquareColor>('Q', SquareColor.SquareBlack), 'Q' },
            {new Tuple<char, SquareColor>('q', SquareColor.SquareWhite), 'w' },
            {new Tuple<char, SquareColor>('q', SquareColor.SquareBlack), 'W' },

            {new Tuple<char, SquareColor>('R', SquareColor.SquareWhite), 'r' },
            {new Tuple<char, SquareColor>('R', SquareColor.SquareBlack), 'R' },
            {new Tuple<char, SquareColor>('r', SquareColor.SquareWhite), 't' },
            {new Tuple<char, SquareColor>('r', SquareColor.SquareBlack), 'T' },

            {new Tuple<char, SquareColor>('B', SquareColor.SquareWhite), 'b' },
            {new Tuple<char, SquareColor>('B', SquareColor.SquareBlack), 'B' },
            {new Tuple<char, SquareColor>('b', SquareColor.SquareWhite), 'v' },
            {new Tuple<char, SquareColor>('b', SquareColor.SquareBlack), 'V' },

            {new Tuple<char, SquareColor>('N', SquareColor.SquareWhite), 'n' },
            {new Tuple<char, SquareColor>('N', SquareColor.SquareBlack), 'N' },
            {new Tuple<char, SquareColor>('n', SquareColor.SquareWhite), 'm' },
            {new Tuple<char, SquareColor>('n', SquareColor.SquareBlack), 'M' },

            {new Tuple<char, SquareColor>('P', SquareColor.SquareWhite), 'p' },
            {new Tuple<char, SquareColor>('P', SquareColor.SquareBlack), 'P' },
            {new Tuple<char, SquareColor>('p', SquareColor.SquareWhite), 'o' },
            {new Tuple<char, SquareColor>('p', SquareColor.SquareBlack), 'O' },

        };


        static readonly Piece kw = FenMappings['K'];
        static readonly Piece kb = FenMappings['k'];
        static readonly Piece qw = FenMappings['Q'];
        static readonly Piece qb = FenMappings['q'];
        static readonly Piece rw = FenMappings['R'];
        static readonly Piece rb = FenMappings['r'];
        static readonly Piece nw = FenMappings['N'];
        static readonly Piece nb = FenMappings['n'];
        static readonly Piece bw = FenMappings['B'];
        static readonly Piece bb = FenMappings['b'];
        static readonly Piece pw = FenMappings['P'];
        static readonly Piece pb = FenMappings['p'];
        static readonly Piece o = null;


        private static GameCreationData _defaultGameCreationData = new GameCreationData
        {
            Board = new Piece[8][]
                {
                    new[] { rb, nb, bb, qb, kb, bb, nb, rb },
                    new[] { pb, pb, pb, pb, pb, pb, pb, pb },
                    new[] { o, o, o, o, o, o, o, o },
                    new[] { o, o, o, o, o, o, o, o },
                    new[] { o, o, o, o, o, o, o, o },
                    new[] { o, o, o, o, o, o, o, o },
                    new[] { pw, pw, pw, pw, pw, pw, pw, pw },
                    new[] { rw, nw, bw, qw, kw, bw, nw, rw }
                },
            DrawClaimed = false,
            DrawReason = "",
            WhoseTurn = Player.White,
            careWhoseTurnItIs = true,
            CanWhiteCastleKingSide = true,
            CanWhiteCastleQueenSide = true,
            CanBlackCastleKingSide = true,
            CanBlackCastleQueenSide = true,
            EnPassant = null
        };

        #endregion
        /// <summary>
        /// 实现象棋规则的 ChessGame 成员。
        /// </summary>
        public ChessGame Game;

        /// <summary>
        /// 记录表项化的 MoreDetailedMove 的历史走子列表。
        /// </summary>
        public BindingList<MoreDetailedMoveImitator> GameMoves
        {
            get;
            private set;
        }

        public bool CareWhoseTurnItIs
        {
            get
            {
                return Game.careWhoseTurnItIs;
            }
            set
            {
                Game.careWhoseTurnItIs = value;
                NotifyPropertyChanged();
            }
        }

        public Player WhoseTurn
        {
            get
            {
                return Game.WhoseTurn;
            }
        }

        /// <summary>
        /// 被吃的白子。
        /// </summary>
        private String _whiteCapturedPieces;

        /// <summary>
        /// 被吃的白子。
        /// </summary>
        public String WhiteCapturedPieces
        {
            get
            {
                return _whiteCapturedPieces;
            }
            private set
            {
                _whiteCapturedPieces = value;
                NotifyPropertyChanged("WhiteCapturedPieces");
            }
        }

        /// <summary>
        /// 被吃的黑子。
        /// </summary>
        private String _blackCapturedPieces;

        /// <summary>
        /// 被吃的黑子。
        /// </summary>
        public String BlackCapturedPieces
        {
            get
            {
                return _blackCapturedPieces;
            }
            private set
            {
                _blackCapturedPieces = value;
                NotifyPropertyChanged("BlackCapturedPieces");
            }
        }

        /// <summary>
        /// 将 Game 里的棋盘状态转成六十四个字符组成的数组，以便用特定的象棋字体显示这些字符，和 Form 中的棋盘字符相关。
        /// </summary>
        public char[] BoardPrint
        {
            get
            {
                Piece[][] gameBoard = Game.GetBoard();
                char[] boardPrint = new char[64];
                for (int r = 0; r < gameBoard.Length; r++)
                {
                    for (int j = 0; j < gameBoard[0].Length; j++)
                    {
                        Piece piece = gameBoard[r][j];

                        char charOnBoard;
                        SquareColor sc = ((r + j) % 2 == 1) ? SquareColor.SquareBlack : SquareColor.SquareWhite;
                        if (piece == null)
                        {
                            // 在那个字体里， + 是黑方块，空格是白方块。
                            charOnBoard = (sc == SquareColor.SquareBlack) ? '+' : ' ';
                        }
                        else
                        {
                            var thisTuple = Tuple.Create(piece.GetFenCharacter(), sc);
                            // fenAndSquareColorMappings 将 (棋子的记谱法字符, 棋盘格颜色) 这个二元组转换为可以用象棋字体显示的字符。
                            charOnBoard = fenAndSquareColorMappings[thisTuple];
                        }
                        boardPrint[r * 8 + j] = charOnBoard;
                    }

                }
                return boardPrint;
            }
        }

        /// <summary>
        /// 分析 moveInStr 中的标准记谱法（SAN）字符串，产生一个 Move，并对 ChessGame 应用这个 Move。
        /// </summary>
        /// <param name="moveInStr">存有标准代数记谱法的字符串。</param>
        /// <param name="player">行动方 Player。</param>
        /// <param name="captured">传出参数，当有吃子时给 captured 赋值。</param>
        /// <returns></returns>
        public MoveType ParseAndApplyMove(string moveInStr, Player player, out Piece captured)
        {
            Move move = PgnMoveReader.ParseMove(moveInStr, player, Game);
            var moveResult = ApplyMove(move, false, out captured);
            return moveResult;
        }

        /// <summary>
        /// 点击窗体中的棋盘格子所触发的走子，就会用这个 ManualMove 来处理。
        /// </summary>
        /// <param name="move">用户点击走子所构造的一个 Move。</param>
        /// <param name="alreadyValidated">这个 Move 是否已经被检查过是合法的。</param>
        /// <param name="captured">传出参数，当有吃子时给 captured 赋值。</param>
        /// <returns></returns>
        public MoveType ManualMove(Move move, bool alreadyValidated, out Piece captured)
        {
            SetControlStatus(ChessBoardGameControlState.Idle, updateImportant: true);
            MoveType moveType = ApplyMove(move, alreadyValidated, out captured);
            Trace.TraceInformation((move.Player == Player.White ? "白方" : "黑方") + "手动走子：" + Game.Moves.Last().SANString);

            if (move.Player == Player.White)
                HasWhiteManuallyMoved = true;
            else
                HasBlackManuallyMoved = true;
            return moveType;
        }

        /// <summary>
        /// 用户在窗体中手动输入的 SAN，会用这个 ManualMove 来处理。 
        /// </summary>
        /// <param name="moveInStr">窗体中手动输入的 SAN 字符串。</param>
        /// <param name="player">行动方 Player。</param>
        /// <param name="captured">传出参数，当有吃子时给 captured 赋值。</param>
        /// <returns></returns>
        public MoveType ManualMove(string moveInStr, Player player, out Piece captured)
        {
            SetControlStatus(ChessBoardGameControlState.Idle, updateImportant: true);
            Trace.TraceInformation((player == Player.White ? "白方" : "黑方") + "输入 SAN 走子：" + moveInStr);

            MoveType moveType = ParseAndApplyMove(moveInStr, player, out captured);
            if (player == Player.White)
                HasWhiteManuallyMoved = true;
            else
                HasBlackManuallyMoved = true;
            return moveType;
        }

        /// <summary>
        /// 较为重要的一个方法：对这盘游戏应用一个走子 Move。
        /// </summary>
        /// <param name="move">要应用的走子 Move。</param>
        /// <param name="alreadyValidated">这个走子是否已经验证过合法。</param>
        /// <param name="captured">传出参数，当有吃子时给 captured 赋值。</param>
        /// <returns></returns>
        public MoveType ApplyMove(Move move, bool alreadyValidated, out Piece captured)
        {
            // 对内部的象棋规则对象 Game 应用这个走子。
            var moveResult = Game.ApplyMove(move, alreadyValidated, out captured);

            WhiteCapturedPieces = _whiteCapturedPieces.ToLower();
            BlackCapturedPieces = _blackCapturedPieces.ToLower();

            if (captured != null)
            {
                if (captured.Owner == Player.White)
                    WhiteCapturedPieces = _whiteCapturedPieces + fenAndSquareColorMappings[Tuple.Create(captured.GetFenCharacter(), SquareColor.SquareBlack)];
                if (captured.Owner == Player.Black)
                    BlackCapturedPieces = _blackCapturedPieces + fenAndSquareColorMappings[Tuple.Create(captured.GetFenCharacter(), SquareColor.SquareBlack)];
            }

            if (moveResult == MoveType.Invalid)
                throw new ArgumentException("Move Invalid.");

            // 将走子后的 BoardPrint 保存下来，方便之后增加一条 MoreDetailedMove 的表项。
            var boardPrintBackup = BoardPrint;

            // 增加一条 MoreDetailedMove 的表项。注意，由于 GameMoves 是绑定到主窗口中的历史走子控件的，所以要用 FormInvoke 来保证线程安全。
            if (FormInvoke != null)
                FormInvoke(new Action(() =>
                {
                    GameMoves.Add(new MoreDetailedMoveImitator(Game.Moves.Count, Game.Moves.Last(), boardPrintBackup));
                }));
            else
            {
                GameMoves.Add(new MoreDetailedMoveImitator(Game.Moves.Count, Game.Moves.Last(), boardPrintBackup));
            }
            

            // 增加一个 UI 更新等待阻塞锁。
            var _updateUIDoneAfterMoveLock = new ManualResetEvent(false);
            _updateUIDoneAfterMoveLocks.Add(_updateUIDoneAfterMoveLock);

            // 触发 WhoseTurn（行动方）的属性更新事件，UI 就会更新，之后释放锁。
            NotifyPropertyChanged(new String[] { "WhoseTurn", "GameMoves" }, _updateUIDoneAfterMoveLock);

            if (Game.Moves.Last().StoredSANString == null)
            {
                throw new ArgumentException("SAN Not Generated");
            }

            // 如果过程中途遭到“重置”、“停止”的中断，那么这里 ProcedureStatus 会复位到 NotStarted，此时不应继续执行。
            if (ProcedureStatus == ChessBoardGameProcedureState.NotStarted)
                return moveResult;

            // 检查一下游戏是否符合结束条件
            UpdateProcedureStatusIfGameEnds();

            if (move.Player == Player.White)
            {
                if (BlackStatus == StdIOState.NotRequesting && !(HasBlackManuallyMoved))
                {
                    Trace.TraceInformation("向黑 AI 发送：" + Game.Moves.Last().SANString);
                    BlackAIProcess.WriteLine(Game.Moves.Last().StoredSANString);
                }
            }
            else
            {
                if (WhiteStatus == StdIOState.NotRequesting && !(HasWhiteManuallyMoved))
                {
                    Trace.TraceInformation("向白 AI 发送：" + Game.Moves.Last().SANString);
                    WhiteAIProcess?.WriteLine(Game.Moves.Last().StoredSANString);
                }
            }

            // 如果有自动方的话，触发下一个走子请求。
            InvokeNextMoveRequest(ChessUtilities.GetOpponentOf(move.Player), _updateUIDoneAfterMoveLock);
            return moveResult;
        }

        /// <summary>
        /// 触发下一个走子请求。
        /// </summary>
        /// <param name="whoseTurn">走子行动方。</param>
        /// <param name="uiLock">UI 等待锁，这个锁没放行就不能接受下一个走子请求。</param>
        public void InvokeNextMoveRequest(Player whoseTurn, ManualResetEvent uiLock)
        {
            if (ProcedureStatus == ChessBoardGameProcedureState.Running)
            {
                if ((Mode == GameMode.BothAuto || Mode == GameMode.WhiteAuto) && (whoseTurn == Player.White))
                {
                    new Thread(ThreadInvokeNextMoveRequestHelper).Start(new InvokeNextMoveRequestArgs
                    {
                        StdIOType = StdIOType.White,
                        UILock = uiLock
                    });
                }
                if ((Mode == GameMode.BothAuto || Mode == GameMode.BlackAuto) && (whoseTurn == Player.Black))
                {
                    new Thread(ThreadInvokeNextMoveRequestHelper).Start(new InvokeNextMoveRequestArgs
                    {
                        StdIOType = StdIOType.Black,
                        UILock = uiLock
                    });
                }
            }
        }

        #endregion

        #region 和两个AI进程有关的方法

        /// <summary>
        /// 要传递给白 AI Process 的 LineProcessor，处理接收到的一行 SAN 字符串。
        /// </summary>
        /// <param name="sanString"></param>
        private void _whiteLineProcess(string sanString)
        {
            // 指示这个字符串是否存在错误。
            bool error = false;

            // 放行秒表更新锁，秒表不再更新。
            _updateWatchLoopLock.Set();

            // 有可能在这个线程运行过程中遭到“重置”或者“终止”，这个时候不应继续运行。
            if (ProcedureStatus != ChessBoardGameProcedureState.Running)
                return;

            // 线程等待锁 mre。
            var mre = new ManualResetEvent(false);
            _allThreadsDoneLocks.Add(mre);

            NotifyPropertyChanged("WhiteStopwatchTime");

            Trace.TraceInformation("白 AI 已输出：" + sanString);
            try
            {
                ParseAndApplyMove(sanString, Player.White, out Piece captured);
            }
            catch (PgnException e)
            {
                FormInvoke(new Action(delegate
                {
                    MessageBox.Show("白方 AI 的输出 [" + sanString + "] 存在错误：" + Environment.NewLine + e.Message + Environment.NewLine + "将请求重新走子。", "白方出错", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }));
                Trace.TraceError("白方 AI 的输出 [" + sanString + "] 存在错误：" + Environment.NewLine + e.Message.Replace(@"\", @"\\") + Environment.NewLine + "将请求重新走子。");
                error = true;
            }

            // 有可能在这个线程运行过程中遭到“重置”或者“终止”，这个时候不应继续运行。
            if (ProcedureStatus != ChessBoardGameProcedureState.Running)
                return;

            if (error)
            {
                // 发送“重新走子”信号。
                WhiteAIProcess.WriteLine(".");
                ProcessAllowOutputAndWait(StdIOType.White);
                Trace.TraceWarning(@"向白 AI 发送：\b 重走\b0 ");
                mre.Set();
                try
                {
                    _allThreadsDoneLocks.Remove(mre);
                }
                catch(Exception e)
                {
                    Trace.TraceError("向 _allThreadsDoneLocks 移除锁错误：" + e.Message + Environment.NewLine + e.StackTrace);
                }
            }

            WhiteStatus = StdIOState.NotRequesting;
            SetControlStatus(ChessBoardGameControlState.Idle, updateImportant: false);
            mre.Set();
            try { _allThreadsDoneLocks.Remove(mre); }
            catch (Exception e)
            {
                Trace.TraceError("向 _allThreadsDoneLocks 移除锁错误：" + e.Message + Environment.NewLine + e.StackTrace);
            }
            return;
        }

        /// <summary>
        /// 要传递给黑 AI Process 的 LineProcessor，处理接收到的一行 SAN 字符串。
        /// </summary>
        /// <param name="sanString"></param>
        private void _blackLineProcess(string sanString)
        {
            // 指示这个字符串是否存在错误。
            bool error = false;

            // 放行秒表更新锁，秒表不再更新。
            _updateWatchLoopLock.Set();

            // 有可能在这个线程运行过程中遭到“重置”或者“终止”，这个时候不应继续运行。
            if (ProcedureStatus != ChessBoardGameProcedureState.Running)
                return;

            // 线程等待锁 mre。
            var mre = new ManualResetEvent(false);
            _allThreadsDoneLocks.Add(mre);

            NotifyPropertyChanged("BlackStopwatchTime");

            Trace.TraceInformation("黑 AI 已输出：" + sanString);
            try
            {
                ParseAndApplyMove(sanString, Player.Black, out Piece captured);
            }
            catch (PgnException e)
            {
                FormInvoke(new Action(delegate
                {
                    MessageBox.Show("黑方 AI 的输出 [" + sanString + "] 存在错误：" + Environment.NewLine + e.Message + Environment.NewLine + "将请求重新走子。", "黑方出错", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }));
                Trace.TraceError("黑方 AI 的输出 [" + sanString + "] 存在错误：" + Environment.NewLine + e.Message.Replace(@"\", @"\\") + Environment.NewLine + "将请求重新走子。");
                error = true;
            }

            // 有可能在这个线程运行过程中遭到“重置”或者“终止”，这个时候不应继续运行。
            if (ProcedureStatus != ChessBoardGameProcedureState.Running)
                return;

            if (error)
            {
                // 发送“重新走子”信号。
                BlackAIProcess.WriteLine(".");
                ProcessAllowOutputAndWait(StdIOType.Black);
                Trace.TraceWarning(@"向黑 AI 发送：\b 重走\b0 ");
                mre.Set();
                try { _allThreadsDoneLocks.Remove(mre); }
                catch (Exception e)
                {
                    Trace.TraceError("向 _allThreadsDoneLocks 移除锁错误：" + e.Message + Environment.NewLine + e.StackTrace);
                }
                return;
            }

            SetControlStatus(ChessBoardGameControlState.Idle, updateImportant: false);
            BlackStatus = StdIOState.NotRequesting;
            mre.Set();
            try { _allThreadsDoneLocks.Remove(mre); }
            catch (Exception e)
            {
                Trace.TraceError("向 _allThreadsDoneLocks 移除锁错误：" + e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        /// <summary>
        /// 当白 AI 未知退出时，触发的事件。
        /// </summary>
        private void _whiteProcessExited()
        {
            if(ProcedureStatus == ChessBoardGameProcedureState.Running && (Mode == GameMode.WhiteAuto || Mode == GameMode.BothAuto))
            {
                FormInvoke(new Action(() => System.Windows.Forms.MessageBox.Show("白方 AI 程序由于未知原因退出，游戏终止。", "游戏终止", MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                Trace.TraceError("白方 AI 程序由于未知原因退出，游戏终止。");
                Stop();
            }
            WhiteStatus = StdIOState.NotStarted;
        }

        /// <summary>
        /// 当黑 AI 未知退出时，触发的事件。
        /// </summary>
        private void _blackProcessExited()
        {
            if (ProcedureStatus == ChessBoardGameProcedureState.Running && (Mode == GameMode.BlackAuto || Mode == GameMode.BothAuto))
            {
                FormInvoke(new Action(() => System.Windows.Forms.MessageBox.Show("黑方 AI 程序由于未知原因退出，游戏终止。", "游戏终止", MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                Trace.TraceError("黑方 AI 程序由于未知原因退出，游戏终止。");
                Stop();
            }
            BlackStatus = StdIOState.NotStarted;
        }

        /// <summary>
        /// 对 Player 加载 AI。
        /// </summary>
        /// <param name="player">要加载 AI 的 Player。</param>
        /// <param name="execPath">可执行文件地址。</param>
        /// <param name="execArguments">可执行文件参数。</param>
        public bool LoadAIExec(Player player, string execPath, string execArguments)
        {
            try
            {
                if (player == Player.White)
                {
                    WhiteAIProcess = new AIProcess(execPath, execArguments, "白 AI", Properties.Settings.Default.HideAIWindow);
                    WhiteStatus = StdIOState.NotStarted;
                    WhiteAIProcess.LineProcess += _whiteLineProcess;
                    WhiteAIProcess.ProcessExited += _whiteProcessExited;
                }
                else
                {
                    BlackAIProcess = new AIProcess(execPath, execArguments, "黑 AI", Properties.Settings.Default.HideAIWindow);
                    BlackStatus = StdIOState.NotStarted;
                    BlackAIProcess.LineProcess += _blackLineProcess;
                    BlackAIProcess.ProcessExited += _blackProcessExited;
                }
                foreach (var x in new List<TraceListener>(Trace.Listeners.Cast<TraceListener>()).Where((x) => x is RichTextBoxTraceListener))
                    ((RichTextBoxTraceListener)(x)).TraceSuccess(execPath.Replace(@"\", @"\\").Replace(@"{", @"\{").Replace(@"}", @"\}") + " " + execArguments.Replace(@"\", @"\\").Replace(@"{", @"\{").Replace(@"}", @"\}") + " 成功载入到" + (player == Player.White ? "白方" : "黑方"));
                return true;
            }
            catch (Win32Exception)
            {
                Trace.TraceError("错误：" + execPath.Replace(@"\", @"\\").Replace(@"{", @"\{").Replace(@"}", @"\}") + " " + execArguments.Replace(@"\", @"\\").Replace(@"{", @"\{").Replace(@"}", @"\}") + " 不是合法的可执行文件！");
                MessageBox.Show(execPath + " " + execArguments + " 不是合法的可执行文件！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void ProcessWhiteStart()
        {
            WhiteAIProcess.Start();
            Trace.TraceInformation("白 AI 启动。");
            WhiteAIProcess.WriteLine("white");
            Trace.TraceInformation(@"向白 AI 写入：\b white\b0");

            WhiteStatus = StdIOState.NotRequesting;
        }

        public void ProcessBlackStart()
        {
            BlackAIProcess.Start();
            Trace.TraceInformation("黑 AI 启动。");
            BlackAIProcess.WriteLine("black");
            Trace.TraceInformation(@"向黑 AI 写入：\b black\b0");

            BlackStatus = StdIOState.NotRequesting;
        }

        struct InvokeNextMoveRequestArgs
        {
            public StdIOType StdIOType;
            public ManualResetEvent UILock;
        }

        // 触发读取下一个走子的辅助函数，在 UI 锁和观棋锁均放行之后请求下一个走子。用于被新线程调用。
        void ThreadInvokeNextMoveRequestHelper(object obj)
        {
            if (ProcedureStatus != ChessBoardGameProcedureState.Running)
                return;
            var mre = new ManualResetEvent(false);
            _allThreadsDoneLocks.Add(mre);

            var args = (InvokeNextMoveRequestArgs)(obj);
            // 等待 UI 锁。
            if (args.UILock != null)
            {
                args.UILock.WaitOne();
                _updateUIDoneAfterMoveLocks.Remove(args.UILock);
            }

            // 等待观棋锁。
            uint wt;
            if ((wt = Properties.Settings.Default.WatchChessTime) != 0)
            {
                var watchChessLock = new ManualResetEvent(false);
                _waitWatchChessLocks.Add(watchChessLock);
                watchChessLock.WaitOne((int)wt);
            }

            // 请求下一个走子。
            ProcessAllowOutputAndWait(args.StdIOType);
            mre.Set();
            try
            {
                _allThreadsDoneLocks.Remove(mre);
            }
            catch (Exception e)
            {
                Trace.TraceError("向 _allThreadsDoneLocks 移除锁错误：" + e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        /// <summary>
        /// 请求下一个走子。
        /// </summary>
        /// <param name="type"></param>
        public void ProcessAllowOutputAndWait(StdIOType type)
        {
            ControlStatus = ChessBoardGameControlState.StdIORunning;
            var IO = type == StdIOType.White ? WhiteAIProcess : BlackAIProcess;
            string stopwatchTimeUpdateString = type == StdIOType.White ? "WhiteStopwatchTime" : "BlackStopwatchTime";

            if (type == StdIOType.White)
            {
                WhiteStatus = StdIOState.Requesting;
                new Thread(WhiteAIProcess.AllowOutputAndWait).Start();
            }
            else
            {
                BlackStatus = StdIOState.Requesting;
                new Thread(BlackAIProcess.AllowOutputAndWait).Start();
            }

            new Thread(ThreadUpdateUIWatchHelper).Start(stopwatchTimeUpdateString);
        }
#endregion
    }
}
