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

    public enum StdIOGameControlState
    {
        Idle,
        Selected,
        StdIORunning
    }

    public enum StdIOGameProcedureState
    {
        Running,
        WhiteWins,
        BlackWins,
        Draw
    }

    public delegate void AppliedMoveEventHandler();
    public delegate void GameProcedureStatusUpdateEventHandler(string reason);
    public delegate void GameControlStatusUpdateEventHandler();
    public delegate void StdIOProcessFinishedHandler();

    public class StdIOGame : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged 成员
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        ManualResetEvent _updateUILock = new ManualResetEvent(false);

        public event GameProcedureStatusUpdateEventHandler GameProcedureStatusUpdated;
        public event GameControlStatusUpdateEventHandler GameControlStatusUpdated;
        public event AppliedMoveEventHandler AppliedMove;
        public event StdIOProcessFinishedHandler StdIOProcessFinished;

        StdIOHandler plyWhiteIO;
        StdIOHandler plyBlackIO;

        public string plyWhiteStopwatchTime
        {
            get
            {
                var t = plyWhiteIO.Watch.Elapsed;
                return string.Format("{0}:{1}", Math.Floor(t.TotalMinutes), t.ToString("ss\\.ff"));
            }
        }

        public string plyWhiteStopwatchStatus
        {
            get
            {
                return plyWhiteIO.Watch.IsRunning ? "Yes" : "No";
            }
        }

        private StdIOGameControlState _controlStatus;
        private StdIOGameProcedureState _procedureStatus;

        public StdIOGameControlState ControlStatus
        {
            get { return _controlStatus; }
            set
            {
                _controlStatus = value;
                GameControlStatusUpdated?.Invoke();
            }
        }
        public StdIOGameProcedureState ProcedureStatus
        {
            get { return _procedureStatus; }
            set
            {
                _procedureStatus = value;
                GameProcedureStatusUpdated?.Invoke("");
            }
        }

        public void SetProcedureStatus(StdIOGameProcedureState pState, string reason)
        {
            _procedureStatus = pState;
            GameProcedureStatusUpdated?.Invoke(reason);
        }

        public ChessGame Game;

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
                StdIOGameProcedureState pState;
                if (whiteWins)
                {
                    resultStr = ("黑方被将死。白方胜！");
                    pState = StdIOGameProcedureState.WhiteWins;
                }
                else if(blackWins)
                {
                    resultStr = ("白方被将死。黑方胜！");
                    pState = StdIOGameProcedureState.BlackWins;
                }
                else if (drawWhiteStalemate)
                {
                    resultStr = ("白方陷入僵局。和局！");
                    pState = StdIOGameProcedureState.Draw;
                }
                else if (drawBlackStalemate)
                {
                    resultStr = ("黑方陷入僵局。和局！");
                    pState = StdIOGameProcedureState.Draw;
                }
                else if (drawFifty)
                {
                    resultStr = ("50 回合内无走兵或吃子动作。和局！");
                    pState = StdIOGameProcedureState.Draw;
                }
                else
                {
                    throw new Exception();
                }

                SetProcedureStatus(pState, resultStr);
            }
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

        public StdIOGame(string plyWhiteExecFileName, string plyWhiteExecArguments, string plyBlackExecFileName, string plyBlackExecArguments)
        {
            plyWhiteIO = new StdIOHandler(plyWhiteExecFileName, plyWhiteExecArguments);
            plyBlackIO = new StdIOHandler(plyBlackExecFileName, plyBlackExecArguments);

            plyWhiteIO.LineProcess += (sanString) =>
            {
                ParseAndApplyMove(sanString, Player.White, out Piece captured);
                //plyWhiteIO.Watch.Stop();
                NotifyPropertyChanged("plyWhiteStopwatchTime");
                StdIOProcessFinished?.Invoke();
                _updateUILock.Set();
            };


            Piece kw = FenMappings['K'];
            Piece kb = FenMappings['k'];
            Piece qw = FenMappings['Q'];
            Piece qb = FenMappings['q'];
            Piece rw = FenMappings['R'];
            Piece rb = FenMappings['r'];
            Piece nw = FenMappings['N'];
            Piece nb = FenMappings['n'];
            Piece bw = FenMappings['B'];
            Piece bb = FenMappings['b'];
            Piece pw = FenMappings['P'];
            Piece pb = FenMappings['p'];
            Piece o = null;

            var gameCreationData = new GameCreationData
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

            Game = new ChessGame(gameCreationData);
            ControlStatus = StdIOGameControlState.Idle;
            ProcedureStatus = StdIOGameProcedureState.Running;
        }

        System.Threading.SynchronizationContext _context;

        public void LoadSynchronizationContext(System.Threading.SynchronizationContext context)
        {
            _context = context;
            plyWhiteIO.Context = _context;
            plyBlackIO.Context = _context;
        }

        public void ProcessWhiteStart()
        {
            plyWhiteIO.Start();
        }


        public void ProcessWhiteAllowOutputAndWait()
        {
            ControlStatus = StdIOGameControlState.StdIORunning;
            Thread t = new Thread(new ThreadStart(plyWhiteIO.AllowOutputAndWait));
            t.Start();

            Thread updateUI = new Thread(new ThreadStart(
                () =>
                {
                    this._updateUILock.Reset();

                    while (true)
                    {
                        this._context.Post(delegate
                        {
                            NotifyPropertyChanged("plyWhiteStopwatchTime");
                        }, null);
                        if (this._updateUILock.WaitOne(17))
                        {
                            break;
                        }
                    }

                }
                ));

            updateUI.Start();
        }

        public static readonly Dictionary<Tuple<char, SquareColor>, char> fenAndSquareColorMappings = new Dictionary<Tuple<char, SquareColor>, char>()
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


    }
}
