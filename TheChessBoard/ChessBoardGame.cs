#define ALTERNATIVE_SAFELY_UPDATEUI

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

namespace TheChessBoard
{

    public class MoreDetailedMoveImitator
    {
        public int Index { get; set; }
        public string PlayerString { get; set; }
        public string SANString { get; set; }
        public string FriendlyText { get; set; }
        public MoreDetailedMoveImitator(int index, MoreDetailedMove m)
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
        }

        public MoreDetailedMoveImitator(int index, Player player, string sanString, string friendlyText)
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
        }

        public MoreDetailedMoveImitator(KeyValuePair<int, MoreDetailedMove> kv) : this(kv.Key, kv.Value)
        { }
    }

    public enum SquareColor
    {
        SquareWhite,
        SquareBlack
    }

    public enum ChessBoardGameControlState
    {
        NotStarted,
        Idle,
        Selected,
        StdIORunning
    }

    public enum ChessBoardGameProcedureState
    {
        NotStarted,
        Running,
        WhiteWins,
        BlackWins,
        Draw
    }

    public enum StdIOState
    {
        NotLoaded,
        NotStarted,
        NotRequesting,
        Requesting,
    }

    public enum StdIOType
    {
        White,
        Black
    }

    public enum GameMode
    {
        NotStarted,
        Manual,
        WhiteAuto,
        BlackAuto,
        BothAuto
    }

    public class StatusUpdatedEventArgs : EventArgs
    {
        private bool _updateImportant;
        private string _reason;
        public StatusUpdatedEventArgs(bool updateImportant, string reason)
        {
            _updateImportant = updateImportant;
            _reason = reason;
        }

        public StatusUpdatedEventArgs(bool updateImportant)
        {
            _updateImportant = updateImportant;
            _reason = null;
        }

        public StatusUpdatedEventArgs()
        {
            _updateImportant = false;
            _reason = null;
        }

        public string Reason { get { return _reason; } }
        public bool UpdateImportant { get { return _updateImportant; } }
    }
    public delegate void AppliedMoveEventHandler();
    public delegate void GameProcedureStatusUpdatedEventHandler(StatusUpdatedEventArgs e);
    public delegate void GameControlStatusUpdatedEventHandler(StatusUpdatedEventArgs e);
    public delegate void PlayerIOStatusUpdatedEventHandler(StatusUpdatedEventArgs e);
    public delegate object FormInvoke(Delegate g);


    public class ChessBoardGame : INotifyPropertyChanged
    {
        private static int _defaultWaitPeriod = 17;

        public ChessBoardGame() : this(_defaultGameCreationData)
        { }

        public ChessBoardGame(GameCreationData gameCreationData)
        {
            Game = new ChessGame(gameCreationData);
            Init();
            WhiteStatus = StdIOState.NotLoaded;
            BlackStatus = StdIOState.NotLoaded;
        }

        void Init()
        {
            SetControlStatus(ChessBoardGameControlState.NotStarted, true);
            SetProcedureStatus(ChessBoardGameProcedureState.NotStarted, true);
            _hasWhiteManuallyMoved = false;
            _hasBlackManuallyMoved = false;
            Mode = GameMode.NotStarted;
            if(_updateUIDoneAfterMoveLocks != null)
                foreach (var uiLock in _updateUIDoneAfterMoveLocks)
                    uiLock.Set();
            _updateUIDoneAfterMoveLocks = new List<ManualResetEvent>();
            _allThreadsDoneLocks = new List<ManualResetEvent>();
            _updateWatchLoopLock.Reset();

            GameMoves = new BindingList<MoreDetailedMoveImitator>();
            GameMoves.Add(new MoreDetailedMoveImitator(0, Player.None, " - ", "开局"));

            InvokeAllUpdates();
        }

        public void Start(GameMode mode)
        {
            SetControlStatus(ChessBoardGameControlState.Idle, true);
            SetProcedureStatus(ChessBoardGameProcedureState.Running, true);
            Mode = mode;
            InvokeAllUpdates();
            InvokeNextMoveRequest(WhoseTurn, null);
        }

        public void ResetAll()
        {
            ResetGame();
            WhiteIO.LineProcess -= _whiteLineProcess;
            BlackIO.LineProcess -= _blackLineProcess;
            WhiteIO.Dispose();
            BlackIO.Dispose();
            WhiteIO = null;
            BlackIO = null;
        }

        public void ResetGame(GameCreationData gameCreationData)
        {
            KillAllAndResetStatus();
            Game = new ChessGame(gameCreationData);
            Init();
            InvokeAllUpdates();
        }

        public void ResetGame()
        {
            ResetGame(_defaultGameCreationData);
        }

        public FormInvoke FormInvoke;

        #region INotifyPropertyChanged 成员
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (_context != null)
            {
#if !ALTERNATIVE_SAFELY_UPDATEUI
                _context.Post((obj) => {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }, null);
#else
                FormInvoke(new Action(delegate { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }));
#endif
            }
            else
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            System.Windows.Forms.Application.DoEvents();
        }

        private void NotifyPropertyChanged(String[] propertyNames, ManualResetEvent uiLock)
        {
            if (PropertyChanged == null) return;
            if (_context != null)
            {
                _context.Post((obj) => {
                    foreach (var propertyName in propertyNames)
                        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                    uiLock?.Set();
                }, null);
            }
            else
            {
                foreach (var propertyName in propertyNames)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                uiLock?.Set();
            }
            System.Windows.Forms.Application.DoEvents();
        }
#endregion

#region 与线程事件有关的成员对象（线程锁、各种EventHandler）
        ManualResetEvent _updateWatchLoopLock = new ManualResetEvent(false);
        List<ManualResetEvent> _updateUIDoneAfterMoveLocks;
        List<ManualResetEvent> _allThreadsDoneLocks;

        public event GameProcedureStatusUpdatedEventHandler GameProcedureStatusUpdated;
        public event GameControlStatusUpdatedEventHandler GameControlStatusUpdated;
        public event AppliedMoveEventHandler AppliedMove;
        public event PlayerIOStatusUpdatedEventHandler PlayerIOStatusUpdated;

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
            _allThreadsDoneLocks.Remove(mre);
        }

        SynchronizationContext _context;

        public void InvokeAllUpdates(bool updateImportant = false)
        {
            var commonEventArgs = new StatusUpdatedEventArgs(updateImportant);
            if (_context != null)
#if !ALTERNATIVE_SAFELY_UPDATEUI
            {
                _context?.Post((obj) => GameProcedureStatusUpdated?.Invoke(commonEventArgs), null);
                _context?.Post((obj) => GameControlStatusUpdated?.Invoke(commonEventArgs), null);
                _context?.Post((obj) => PlayerIOStatusUpdated?.Invoke(commonEventArgs), null);

            }
#else
            {
                FormInvoke(new Action(delegate { GameProcedureStatusUpdated?.Invoke(commonEventArgs); }));
                FormInvoke(new Action(delegate { GameControlStatusUpdated?.Invoke(commonEventArgs); }));
                FormInvoke(new Action(delegate { PlayerIOStatusUpdated?.Invoke(commonEventArgs); }));
            }
#endif
            else
            {
                GameProcedureStatusUpdated?.Invoke(commonEventArgs);
                GameControlStatusUpdated?.Invoke(commonEventArgs);
                PlayerIOStatusUpdated?.Invoke(commonEventArgs);
            }
            NotifyPropertyChanged(new[] { "BoardPrint", "WhoseTurn", "CareWhoseTurnItIs", "WhiteStopwatchTime", "BlackStopwatchTime" }, null);
        }

        public void KillAllAndResetStatus()
        {
            _whiteStatus = _whiteStatus == StdIOState.NotLoaded ? StdIOState.NotLoaded : StdIOState.NotStarted;
            _blackStatus = _blackStatus == StdIOState.NotLoaded ? StdIOState.NotLoaded : StdIOState.NotStarted;
            _controlStatus = ChessBoardGameControlState.NotStarted;
            _procedureStatus = ChessBoardGameProcedureState.NotStarted;
            foreach (var uiLock in _updateUIDoneAfterMoveLocks)
                uiLock.Set();
            _updateWatchLoopLock.Set();

            Thread.Sleep(100);
            if(_allThreadsDoneLocks.Count > 0)
                WaitHandle.WaitAll(_allThreadsDoneLocks.Where((m)=>m!=null).ToArray(), 1000);   //Time

            WhiteIO?.Kill();
            BlackIO?.Kill();

            _whiteStatus = _whiteStatus == StdIOState.NotLoaded ? StdIOState.NotLoaded : StdIOState.NotStarted;
            _blackStatus = _blackStatus == StdIOState.NotLoaded ? StdIOState.NotLoaded : StdIOState.NotStarted;
            _controlStatus = ChessBoardGameControlState.NotStarted;
            _procedureStatus = ChessBoardGameProcedureState.NotStarted;
        }

#endregion

        public ChessGame Game;

#region 和两个AI进程有关的成员对象
        public StdIOHandler WhiteIO;
        public StdIOHandler BlackIO;

        public string WhiteStopwatchTime
        {
            get
            {
                if (WhiteIO == null)
                {
                    return "--:--.-------";
                }
                var t = WhiteIO.Watch.Elapsed;
                return string.Format("{0}:{1}", Math.Floor(t.TotalMinutes), t.ToString("ss\\.fffffff"));
            }
        }

        public string WhiteStopwatchStatus
        {
            get
            {
                if (WhiteIO == null)
                {
                    return "No";
                }
                return WhiteIO.Watch.IsRunning ? "Yes" : "No";
            }
        }

        public string BlackStopwatchTime
        {
            get
            {
                if (BlackIO == null)
                {
                    return "--:--.-------";
                }
                var t = BlackIO.Watch.Elapsed;
                return string.Format("{0}:{1}", Math.Floor(t.TotalMinutes), t.ToString("ss\\.fffffff"));
            }
        }

        public string BlackStopwatchStatus
        {
            get
            {
                if (BlackIO == null)
                {
                    return "No";
                }
                return BlackIO.Watch.IsRunning ? "Yes" : "No";
            }
        }

#endregion

#region 和当前游戏的窗体状态、游戏进程状态有关的成员
        private ChessBoardGameControlState _controlStatus;
        private ChessBoardGameProcedureState _procedureStatus;
        private StdIOState _whiteStatus;
        private StdIOState _blackStatus;
        private bool _hasWhiteManuallyMoved;
        private bool _hasBlackManuallyMoved;

        public GameMode Mode;

        public bool HasWhiteManuallyMoved { get { return _hasWhiteManuallyMoved; } }
        public bool HasBlackManuallyMoved { get { return _hasBlackManuallyMoved; } }

        public ChessBoardGameControlState ControlStatus
        {
            get { return _controlStatus; }
            set
            {
                _controlStatus = value;

                if (_context != null)
#if !ALTERNATIVE_SAFELY_UPDATEUI
                    _context?.Post((obj) => GameControlStatusUpdated?.Invoke(new StatusUpdatedEventArgs()), null);
#else
                    FormInvoke(new Action(delegate { GameControlStatusUpdated?.Invoke(new StatusUpdatedEventArgs()); }));
#endif
                else
                    GameControlStatusUpdated?.Invoke(new StatusUpdatedEventArgs());
                //System.Windows.Forms.Application.DoEvents();
            }
        }
        public ChessBoardGameProcedureState ProcedureStatus
        {
            get { return _procedureStatus; }
            set
            {
                _procedureStatus = value;

                if (_context != null)
#if !ALTERNATIVE_SAFELY_UPDATEUI
                    _context?.Post((obj) => GameProcedureStatusUpdated?.Invoke(new StatusUpdatedEventArgs()), null);
#else
                    FormInvoke(new Action(delegate { GameProcedureStatusUpdated?.Invoke(new StatusUpdatedEventArgs()); }));
#endif
                else
                    GameProcedureStatusUpdated?.Invoke(new StatusUpdatedEventArgs());
                //System.Windows.Forms.Application.DoEvents();
            }
        }

        public StdIOState WhiteStatus
        {
            get { return _whiteStatus; }
            set
            {
                _whiteStatus = value;
                if (_context != null)
#if !ALTERNATIVE_SAFELY_UPDATEUI
                    _context?.Post((obj) => PlayerIOStatusUpdated?.Invoke(new StatusUpdatedEventArgs()), null);
#else
                    FormInvoke(new Action(delegate { PlayerIOStatusUpdated?.Invoke(new StatusUpdatedEventArgs()); }));
#endif
                else
                    PlayerIOStatusUpdated?.Invoke(new StatusUpdatedEventArgs());
                //System.Windows.Forms.Application.DoEvents();
            }
        }

        public StdIOState BlackStatus
        {
            get { return _blackStatus; }
            set
            {
                _blackStatus = value;
                if (_context != null)
#if !ALTERNATIVE_SAFELY_UPDATEUI
                    _context?.Post((obj) => PlayerIOStatusUpdated?.Invoke(new StatusUpdatedEventArgs()), null);
#else
                    FormInvoke(new Action(delegate { PlayerIOStatusUpdated?.Invoke(new StatusUpdatedEventArgs()); }));
#endif
                else
                    PlayerIOStatusUpdated?.Invoke(new StatusUpdatedEventArgs());
                //System.Windows.Forms.Application.DoEvents();
            }
        }

        public void SetProcedureStatus(ChessBoardGameProcedureState pState, bool updateImportant, string reason = null)
        {
            _procedureStatus = pState;
            if (_context != null)
#if !ALTERNATIVE_SAFELY_UPDATEUI
                _context?.Post((obj) => GameProcedureStatusUpdated?.Invoke(new StatusUpdatedEventArgs(updateImportant, reason)), null);
#else
                FormInvoke(new Action(delegate { GameProcedureStatusUpdated?.Invoke(new StatusUpdatedEventArgs(updateImportant, reason)); }));
#endif
            else
                GameProcedureStatusUpdated?.Invoke(new StatusUpdatedEventArgs(updateImportant, reason));
            //System.Windows.Forms.Application.DoEvents();
        }

        public void SetControlStatus(ChessBoardGameControlState cState, bool updateImportant, string reason = null)
        {
            _controlStatus = cState;
            if (_context != null)
#if !ALTERNATIVE_SAFELY_UPDATEUI
                _context?.Post((obj) => GameControlStatusUpdated?.Invoke(new StatusUpdatedEventArgs(updateImportant, reason)), null);
#else
                FormInvoke(new Action(delegate { GameControlStatusUpdated?.Invoke(new StatusUpdatedEventArgs(updateImportant, reason)); }));
#endif
            else
                GameControlStatusUpdated?.Invoke(new StatusUpdatedEventArgs(updateImportant, reason));
            //System.Windows.Forms.Application.DoEvents();
        }

        public void GameProcedureStatusUpdate()
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

                SetProcedureStatus(pState, true, resultStr);
            }
        }
#endregion

#region 和ChessGame有关的成员

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
                            charOnBoard = (sc == SquareColor.SquareBlack) ? '+' : ' ';
                        }
                        else
                        {
                            var thisTuple = Tuple.Create(piece.GetFenCharacter(), sc);
                            charOnBoard = fenAndSquareColorMappings[thisTuple];
                        }
                        boardPrint[r * 8 + j] = charOnBoard;
                    }

                }
                return boardPrint;
            }
        }

        public MoveType ParseAndApplyMove(string moveInStr, Player player, out Piece captured)
        {
            Move move = PgnMoveReader.ParseMove(moveInStr, player, Game);
            var moveResult = ApplyMove(move, false, out captured);
            if (moveResult == MoveType.Invalid)
                throw new ArgumentException("Move Invalid.");
            return moveResult;
        }

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

        public MoveType ManualMove(Move move, bool alreadyValidated, out Piece captured)
        {
            MoveType moveType = ApplyMove(move, alreadyValidated, out captured);
            if (move.Player == Player.White)
                _hasWhiteManuallyMoved = true;
            else
                _hasBlackManuallyMoved = true;

            return moveType;
        }

        public MoveType ManualMove(string moveInStr, Player player, out Piece captured)
        {
            MoveType moveType = ParseAndApplyMove(moveInStr, player, out captured);
            if (player == Player.White)
                _hasWhiteManuallyMoved = true;
            else
                _hasBlackManuallyMoved = true;
            return moveType;
        }

        public MoveType ApplyMove(Move move, bool alreadyValidated, out Piece captured)
        {
            var moveResult = Game.ApplyMove(move, alreadyValidated, out captured);
            

            if (moveResult == MoveType.Invalid)
                throw new ArgumentException("Move Invalid.");

            var _updateUIDoneAfterMoveLock = new ManualResetEvent(false);
            _updateUIDoneAfterMoveLocks.Add(_updateUIDoneAfterMoveLock);


            NotifyPropertyChanged(new String[] { "BoardPrint", "WhoseTurn", "GameMoves" }, _updateUIDoneAfterMoveLock);

            if (Game.Moves.Last().StoredSANString == null)
            {
                throw new ArgumentException("SAN Not Generated");
            }

            if (ProcedureStatus == ChessBoardGameProcedureState.NotStarted)
                return moveResult;

            if (move.Player == Player.White)
            {
                if(BlackStatus == StdIOState.NotRequesting && !(HasBlackManuallyMoved))
                    BlackIO.Write(Game.Moves.Last().StoredSANString);
            }
            else
            {
                if (WhiteStatus == StdIOState.NotRequesting && !(HasWhiteManuallyMoved))
                    WhiteIO?.Write(Game.Moves.Last().StoredSANString);
            }

            ControlStatus = ChessBoardGameControlState.Idle;
            if (_context != null)
#if !ALTERNATIVE_SAFELY_UPDATEUI
            {
                _context?.Post((obj) => AppliedMove?.Invoke(), null);
                GameMoves.Add(new MoreDetailedMoveImitator(Game.Moves.Count, Game.Moves.Last()));
            }
#else
                FormInvoke(new Action(delegate { GameMoves.Add(new MoreDetailedMoveImitator(Game.Moves.Count, Game.Moves.Last())); AppliedMove?.Invoke();  }));
#endif
            else
            {
                GameMoves.Add(new MoreDetailedMoveImitator(Game.Moves.Count, Game.Moves.Last()));
                AppliedMove?.Invoke();
            }

            System.Windows.Forms.Application.DoEvents();
            GameProcedureStatusUpdate();

            InvokeNextMoveRequest(ChessUtilities.GetOpponentOf(move.Player), _updateUIDoneAfterMoveLock);
            return moveResult;
        }

        public BindingList<MoreDetailedMoveImitator> GameMoves
        {
            get;
            private set;
        }

#region 棋子定义
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
#endregion

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

#region 和两个AI进程有关的方法
        private void _whiteLineProcess(string sanString)
        {
            _updateWatchLoopLock.Set();
            if (ProcedureStatus != ChessBoardGameProcedureState.Running)
                return;
            var mre = new ManualResetEvent(false);
            _allThreadsDoneLocks.Add(mre);
            ParseAndApplyMove(sanString, Player.White, out Piece captured);
            NotifyPropertyChanged("WhiteStopwatchTime");
            if (ProcedureStatus == ChessBoardGameProcedureState.NotStarted) //Killed
                return;
            WhiteStatus = StdIOState.NotRequesting;
            mre.Set();
            _allThreadsDoneLocks.Remove(mre);
        }

        private void _blackLineProcess(string sanString)
        {
            _updateWatchLoopLock.Set();
            if (ProcedureStatus != ChessBoardGameProcedureState.Running)
                return;
            var mre = new ManualResetEvent(false);
            _allThreadsDoneLocks.Add(mre);
            ParseAndApplyMove(sanString, Player.Black, out Piece captured);
            NotifyPropertyChanged("BlackStopwatchTime");
            if (ProcedureStatus == ChessBoardGameProcedureState.NotStarted) //Killed
                return;
            BlackStatus = StdIOState.NotRequesting;
            mre.Set();
            _allThreadsDoneLocks.Remove(mre);
        }

        public void LoadAIExec(Player player, string execPath, string execArguments)
        {
            try
            {
                if (player == Player.White)
                {
                    WhiteIO = new StdIOHandler(execPath, execArguments);
                    WhiteStatus = StdIOState.NotStarted;
                    WhiteIO.Context = _context;
                    WhiteIO.LineProcess += _whiteLineProcess;
                }
                else
                {
                    BlackIO = new StdIOHandler(execPath, execArguments);
                    BlackStatus = StdIOState.NotStarted;
                    BlackIO.Context = _context;
                    BlackIO.LineProcess += _blackLineProcess;
                }
            }
            catch (Win32Exception)
            {
                Trace.TraceError(execPath.Replace(@"\", @"\\").Replace(@"{", @"\{").Replace(@"}", @"\}") + " " + execArguments + " 无法找到！");
            }
            finally
            {
                Trace.TraceInformation(execPath.Replace(@"\", @"\\").Replace(@"{", @"\{").Replace(@"}", @"\}") + " " + execArguments + " 成功载入到" + (player == Player.White ? "白方" : "黑方"));
            }
        }

        public void LoadSynchronizationContext(SynchronizationContext context)
        {
            _context = context;
            if (WhiteIO != null)
                WhiteIO.Context = _context;
            if (BlackIO != null)
                BlackIO.Context = _context;
        }

        public void ProcessWhiteStart()
        {
            WhiteIO.Start();
            WhiteIO.Write("white");
            WhiteStatus = StdIOState.NotRequesting;
        }

        public void ProcessBlackStart()
        {
            BlackIO.Start();
            BlackIO.Write("black");
            BlackStatus = StdIOState.NotRequesting;
        }

        struct InvokeNextMoveRequestArgs
        {
            public StdIOType StdIOType;
            public ManualResetEvent UILock;
        }
        void ThreadInvokeNextMoveRequestHelper(object obj)
        {
            if (ProcedureStatus != ChessBoardGameProcedureState.Running)
                return;
            var mre = new ManualResetEvent(false);
            _allThreadsDoneLocks.Add(mre);

            var args = (InvokeNextMoveRequestArgs)(obj);
            if (args.UILock != null)
            {
                args.UILock.WaitOne();
                _updateUIDoneAfterMoveLocks.Remove(args.UILock);
            }
            ProcessAllowOutputAndWait(args.StdIOType);
            mre.Set();
            _allThreadsDoneLocks.Remove(mre);
        }

        public void ProcessAllowOutputAndWait(StdIOType type)
        {
            ControlStatus = ChessBoardGameControlState.StdIORunning;
            var IO = type == StdIOType.White ? WhiteIO : BlackIO;
            string stopwatchTimeUpdateString = type == StdIOType.White ? "WhiteStopwatchTime" : "BlackStopwatchTime";

            if (type == StdIOType.White)
            {
                WhiteStatus = StdIOState.Requesting;
                new Thread(WhiteIO.AllowOutputAndWait).Start();
            }
            else
            {
                BlackStatus = StdIOState.Requesting;
                new Thread(BlackIO.AllowOutputAndWait).Start();
            }

            new Thread(ThreadUpdateUIWatchHelper).Start(stopwatchTimeUpdateString);
        }
#endregion
    }
}
