using System;
using System.Collections.Generic;
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
        WaitForInput,
        Outputing,
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

    public class ChessBoardGame : INotifyPropertyChanged
    {
        private static int _defaultWaitPeriod = 17;
        public ChessBoardGame()
        {
            Game = new ChessGame(_defaultGameCreationData);
            Init();
            WhiteStatus = StdIOState.NotLoaded;
            BlackStatus = StdIOState.NotLoaded;
            
        }

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
            Mode = GameMode.NotStarted;
            InvokeAllUpdates();
        }

        public void Start(GameMode mode)
        {
            SetControlStatus(ChessBoardGameControlState.Idle, true);
            SetProcedureStatus(ChessBoardGameProcedureState.Running, true);
            Mode = mode;
            InvokeAllUpdates();
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
            Game = new ChessGame(gameCreationData);
            Init();
            InvokeAllUpdates();
        }

        public void ResetGame()
        {
            Game = new ChessGame(_defaultGameCreationData);
            Init();
            InvokeAllUpdates();
        }


        #region INotifyPropertyChanged 成员
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region 与线程事件有关的成员对象（线程锁、各种EventHandler）
        ManualResetEvent _updateUILock = new ManualResetEvent(false);

        public event GameProcedureStatusUpdatedEventHandler GameProcedureStatusUpdated;
        public event GameControlStatusUpdatedEventHandler GameControlStatusUpdated;
        public event AppliedMoveEventHandler AppliedMove;
        public event PlayerIOStatusUpdatedEventHandler PlayerIOStatusUpdated;

        System.Threading.SynchronizationContext _context;

        public void InvokeAllUpdates(bool updateImportant = false)
        {
            var commonEventArgs = new StatusUpdatedEventArgs(updateImportant);
            GameProcedureStatusUpdated?.Invoke(commonEventArgs);
            GameControlStatusUpdated?.Invoke(commonEventArgs);
            PlayerIOStatusUpdated?.Invoke(commonEventArgs);
            NotifyPropertyChanged("BoardPrint");
            NotifyPropertyChanged("WhoseTurn");
            NotifyPropertyChanged("CareWhoseTurnItIs");
            NotifyPropertyChanged("WhiteStopwatchTime");
            NotifyPropertyChanged("BlackStopwatchTime");
        }
        #endregion

        public ChessGame Game;

        #region 和两个AI进程有关的成员对象
        StdIOHandler WhiteIO;
        StdIOHandler BlackIO;

        public string WhiteStopwatchTime
        {
            get
            {
                if(WhiteIO == null)
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
        GameMode Mode;

        public ChessBoardGameControlState ControlStatus
        {
            get { return _controlStatus; }
            set
            {
                _controlStatus = value;
                GameControlStatusUpdated?.Invoke(new StatusUpdatedEventArgs());
            }
        }
        public ChessBoardGameProcedureState ProcedureStatus
        {
            get { return _procedureStatus; }
            set
            {
                _procedureStatus = value;
                GameProcedureStatusUpdated?.Invoke(new StatusUpdatedEventArgs());
            }
        }

        public StdIOState WhiteStatus
        {
            get { return _whiteStatus; }
            set
            {
                _whiteStatus = value;
                PlayerIOStatusUpdated?.Invoke(new StatusUpdatedEventArgs());
            }
        }

        public StdIOState BlackStatus
        {
            get { return _blackStatus; }
            set
            {
                _blackStatus = value;
                PlayerIOStatusUpdated?.Invoke(new StatusUpdatedEventArgs());
            }
        }

        public void SetProcedureStatus(ChessBoardGameProcedureState pState, bool updateImportant, string reason = null)
        {
            _procedureStatus = pState;
            GameProcedureStatusUpdated?.Invoke(new StatusUpdatedEventArgs(updateImportant, reason));
        }

        public void SetControlStatus(ChessBoardGameControlState cState, bool updateImportant, string reason = null)
        {
            _controlStatus = cState;
            GameControlStatusUpdated?.Invoke(new StatusUpdatedEventArgs(updateImportant, reason));
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
            else
            {
                System.Diagnostics.Debug.Assert(ProcedureStatus == ChessBoardGameProcedureState.Running);
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

        public MoveType ApplyMove(Move move, bool alreadyValidated, out Piece captured)
        {
            var moveResult = Game.ApplyMove(move, alreadyValidated, out captured);
            if (moveResult == MoveType.Invalid)
                throw new ArgumentException("Move Invalid.");
            NotifyPropertyChanged("BoardPrint");
            NotifyPropertyChanged("WhoseTurn");
            AppliedMove?.Invoke();
            GameProcedureStatusUpdate();
            return moveResult;
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
            ParseAndApplyMove(sanString, Player.White, out Piece captured);
            NotifyPropertyChanged("WhiteStopwatchTime");
            WhiteStatus = StdIOState.WaitForInput;
            _updateUILock.Set();
        }

        private void _blackLineProcess(string sanString)
        {
            ParseAndApplyMove(sanString, Player.Black, out Piece captured);
            NotifyPropertyChanged("BlackStopwatchTime");
            BlackStatus = StdIOState.WaitForInput;
            _updateUILock.Set();
        }

        public void LoadAIExec(string whiteExecPath, string whiteExecArguments, string blackExecPath, string blackExecArguments)
        {
            WhiteIO = new StdIOHandler(whiteExecPath, whiteExecArguments);
            BlackIO = new StdIOHandler(blackExecPath, blackExecArguments);
            WhiteStatus = StdIOState.WaitForInput;
            BlackStatus = StdIOState.WaitForInput;

            WhiteIO.Context = _context;
            BlackIO.Context = _context;

            WhiteIO.LineProcess += _whiteLineProcess;
            BlackIO.LineProcess += _blackLineProcess;
        }

        public void LoadSynchronizationContext(System.Threading.SynchronizationContext context)
        {
            _context = context;
            if(WhiteIO != null)
                WhiteIO.Context = _context;
            if (BlackIO != null)
                BlackIO.Context = _context;
        }

        public void ProcessWhiteStart()
        {
            WhiteIO.Start();
        }

        public void ProcessBlackStart()
        {
            BlackIO.Start();
        }

        public void ProcessWhiteAllowOutputAndWait () { ProcessAllowOutputAndWait(StdIOType.White); }
        public void ProcessBlackAllowOutputAndWait() { ProcessAllowOutputAndWait(StdIOType.Black); }

        private void ProcessAllowOutputAndWait(StdIOType type)
        {
            ControlStatus = ChessBoardGameControlState.StdIORunning;
            var IO = type == StdIOType.White ? WhiteIO : BlackIO;
            string stopwatchTimeUpdateString = type == StdIOType.White ? "WhiteStopwatchTime" : "BlackStopwatchTime";
            Thread t = new Thread(new ThreadStart(IO.AllowOutputAndWait));
            t.Start();

            Thread updateUI = new Thread(new ThreadStart(
                () =>
                {
                    this._updateUILock.Reset();

                    while (true)
                    {
                        if(_context == null)
                        {
                            throw new NullReferenceException("Context of this ChessBoardGame is null. Didn't initialize it with LoadSynchronizationContext(context)?");
                        }
                        this._context.Post(delegate
                        {
                            NotifyPropertyChanged(stopwatchTimeUpdateString);
                        }, null);
                        if (this._updateUILock.WaitOne(_defaultWaitPeriod))
                        {
                            break;
                        }
                    }

                }
                ));

            updateUI.Start();
        }
        #endregion
    }
}
