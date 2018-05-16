using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ChessDotNet.Pieces;
using System.Text;

namespace ChessDotNet
{
    /// <summary>
    /// 本库的最重要类：ChessGame。它完整地存储了一盘游戏的所有信息，包括棋盘、何方行动、迄今移动半步数、从上一次走兵/吃子算起的移动半步数，等等；此外，它还提供了一系列方法函数。
    /// </summary>
    public class ChessGame
    {
        bool _drawn = false;
        string _drawReason = null;
        Player _resigned = Player.None;

        public bool DrawClaimed
        {
            get
            {
                return _drawn;
            }
        }

        public string DrawReason
        {
            get
            {
                return _drawReason;
            }
        }

        public Player Resigned
        {
            get
            {
                return _resigned;
            }
        }

        protected virtual int[] AllowedFenPartsLength
        {
            get
            {
                return new int[1] { 6 };
            }
        }

        protected virtual bool UseTildesInFenGeneration
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 这些都是游戏的初始设定，即各车在王车易位前都应该处于什么列（File）。
        /// </summary>
        public File InitialWhiteRookFileKingsideCastling { get; protected set; }
        public File InitialWhiteRookFileQueensideCastling { get; protected set; }
        public File InitialBlackRookFileKingsideCastling { get; protected set; }
        public File InitialBlackRookFileQueensideCastling { get; protected set; }
        public File InitialWhiteKingFile { get; protected set; }
        public File InitialBlackKingFile { get; protected set; }

        /// <summary>
        /// 将字符映射到对应的棋子的静态字典（Dictionary）。
        /// </summary>
        public static Dictionary<char, Piece> OriginalFenMappings = new Dictionary<char, Piece>()
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

        /// <summary>
        /// 同上。
        /// </summary>
        protected virtual Dictionary<char, Piece> FenMappings
        {
            get
            {
                return OriginalFenMappings;
            }
        }

        /// <summary>
        /// 将 SAN 中的字母对应到棋子。
        /// </summary>
        /// <param name="c">SAN 字母。</param>
        /// <param name="owner">由于 SAN 字母只有大写，所以还需传参表示棋子是哪方。</param>
        /// <returns></returns>
        public virtual Piece MapPgnCharToPiece(char c, Player owner)
        {
            switch (c)
            {
                case 'K':
                    return owner == Player.White ? FenMappings['K'] : FenMappings['k'];
                case 'Q':
                    return owner == Player.White ? FenMappings['Q'] : FenMappings['q'];
                case 'R':
                    return owner == Player.White ? FenMappings['R'] : FenMappings['r'];
                case 'B':
                    return owner == Player.White ? FenMappings['B'] : FenMappings['b'];
                case 'N':
                    return owner == Player.White ? FenMappings['N'] : FenMappings['n'];
                case 'P':
                    return owner == Player.White ? FenMappings['P'] : FenMappings['p'];
                default:
                    if (!char.IsLower(c))
                    {
                        throw new PgnException("Unrecognized piece type: " + c.ToString());
                    }
                    return owner == Player.White ? FenMappings['P'] : FenMappings['p'];
            }
        }

        /// <summary>
        /// 同上。
        /// </summary>
        /// <param name="c"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static Piece OriginalMapPgnCharToPiece(char c, Player owner)
        {
            switch (c)
            {
                case 'K':
                    return owner == Player.White ? OriginalFenMappings['K'] : OriginalFenMappings['k'];
                case 'Q':
                    return owner == Player.White ? OriginalFenMappings['Q'] : OriginalFenMappings['q'];
                case 'R':
                    return owner == Player.White ? OriginalFenMappings['R'] : OriginalFenMappings['r'];
                case 'B':
                    return owner == Player.White ? OriginalFenMappings['B'] : OriginalFenMappings['b'];
                case 'N':
                    return owner == Player.White ? OriginalFenMappings['N'] : OriginalFenMappings['n'];
                case 'P':
                    return owner == Player.White ? OriginalFenMappings['P'] : OriginalFenMappings['p'];
                default:
                    if (!char.IsLower(c))
                    {
                        throw new PgnException("Unrecognized piece type: " + c.ToString());
                    }
                    return owner == Player.White ? OriginalFenMappings['P'] : OriginalFenMappings['p'];
            }
        }

        public virtual bool NeedsPgnMoveSpecialTreatment(string move, Player player) { return false; }
        public virtual bool HandleSpecialPgnMove(string move, Player player) { return false;  } 

        /// <summary>
        /// 五十步和棋状态。
        /// 如果是 true，即距离上次动兵/吃子有了50步。
        /// </summary>
        protected bool fiftyMoves = false;
        public virtual bool FiftyMovesAndThisCanResultInDraw { get { return fiftyMoves; } }

        /// <summary>
        /// 本盘游戏中，是否强制必须轮流走棋。一般为 true，为 false 的情况仅作调试用。
        /// </summary>
        public bool careWhoseTurnItIs = true;

        /// <summary>
        /// 返回现在是否和棋。不能将军/僵局，而且有五十步。
        /// </summary>
        public virtual bool DrawCanBeClaimed
        {
            get
            {
                return FiftyMovesAndThisCanResultInDraw && !IsCheckmated(WhoseTurn) && !IsStalemated(WhoseTurn);
            }
        }

        /// <summary>
        /// 字面意思。
        /// </summary>
        public Player WhoseTurn
        {
            get;
            protected set;
        }

        /// <summary>
        /// 半步（一方走一下称半步）数。
        /// </summary>
        protected int i_halfMoveClock = 0;
        public int HalfMoveClock
        {
            get
            {
                return i_halfMoveClock;
            }
        }

        /// <summary>
        /// 步（两方轮流下一次子算一步）数。
        /// </summary>
        protected int i_fullMoveNumber = 1;
        public int FullMoveNumber
        {
            get
            {
                return i_fullMoveNumber;
            }
        }

        /// <summary>
        /// 返回棋盘上所有棋子的集合。
        /// 由于 this.Board 本质上是 Piece 的数组的数组（Piece [][]），所以在计算的时候就对 Board 用了一个 Where 方法，返回棋子的集合。
        /// </summary>
        public ReadOnlyCollection<Piece> PiecesOnBoard
        {
            get
            {
                return new ReadOnlyCollection<Piece>(Board.SelectMany(x => x).Where(x => x != null).ToList());
            }
        }

        public virtual int BoardWidth
        {
            get
            {
                return 8;
            }
        }

        public virtual int BoardHeight
        {
            get
            {
                return 8;
            }
        }

        /// <summary>
        /// 游戏内棋盘。Board 本质上是 Piece 的数组的数组。
        /// </summary>
        protected Piece[][] Board;
        public Piece[][] GetBoard()
        {
            return CloneBoard(Board);
        }

        /// <summary>
        /// 本游戏记录的历史移动。历史移动统一用 MoreDetailedMove 以记载更多信息。
        /// </summary>
        List<MoreDetailedMove> _moves = new List<MoreDetailedMove>();
        public ReadOnlyCollection<MoreDetailedMove> Moves
        {
            get
            {
                return new ReadOnlyCollection<MoreDetailedMove>(_moves);
            }
            protected set
            {
                _moves = value.ToList();
            }
        }

        /// <summary>
        /// 黑棋现在能够进行王翼（短）易位吗？
        /// </summary>
        public bool CanBlackCastleKingSide
        {
            get;
            protected set;
        }

        public bool CanBlackCastleQueenSide
        {
            get;
            protected set;
        }

        public bool CanWhiteCastleKingSide
        {
            get;
            protected set;
        }

        public bool CanWhiteCastleQueenSide
        {
            get;
            protected set;
        }

        /// <summary>
        /// 现在可以王车易位吗？
        /// </summary>
        protected virtual bool CastlingCanBeLegal
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 复制一个原有的 Board，返回。（由于 C# 的传值（如果不是最原始的那些数据类型）本质上是传引用，所以如果直接把 this.Board 传出去会遭到修改）
        /// </summary>
        /// <param name="originalBoard"></param>
        /// <returns></returns>
        protected static Piece[][] CloneBoard(Piece[][] originalBoard)
        {
            ChessUtilities.ThrowIfNull(originalBoard, "originalBoard");
            Piece[][] newBoard = new Piece[originalBoard.Length][];
            for (int i = 0; i < originalBoard.Length; i++)
            {
                newBoard[i] = new Piece[originalBoard[i].Length];
                Array.Copy(originalBoard[i], newBoard[i], originalBoard[i].Length);
            }
            return newBoard;
        }

        /// <summary>
        /// ChessGame 的构造函数。初始化一些内部成员，生成最开始的棋盘。
        /// </summary>
        public ChessGame()
        {
            WhoseTurn = Player.White;
            _moves = new List<MoreDetailedMove>();
            Board = new Piece[8][];
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
            };
            CanBlackCastleKingSide = CanBlackCastleQueenSide = CanWhiteCastleKingSide = CanWhiteCastleQueenSide = CastlingCanBeLegal;
            InitialBlackRookFileKingsideCastling = InitialWhiteRookFileKingsideCastling = File.H;
            InitialBlackRookFileQueensideCastling = InitialWhiteRookFileQueensideCastling = File.A;
            InitialBlackKingFile = InitialWhiteKingFile = File.E;
        }

        /// <summary>
        /// ChessGame 的构造函数，原始构造后，接受一串 Move（IEnumerable 是通用的枚举类型），应用到初始局面上。
        /// </summary>
        /// <param name="moves"></param>
        /// <param name="movesAreValidated"></param>
        public ChessGame(IEnumerable<Move> moves, bool movesAreValidated) : this()
        {
            if (moves == null)
                throw new ArgumentNullException("moves");
            if (moves.Count() == 0)
                throw new ArgumentException("The Count of moves has to be greater than 0.");
            foreach (Move m in moves)
            {
                if (ApplyMove(m, movesAreValidated) == MoveType.Invalid)
                {
                    throw new ArgumentException("Invalid move passed to ChessGame constructor.");
                }
            }
        }

        /// <summary>
        /// 从 FEN 创建游戏。
        /// </summary>
        /// <param name="fen"></param>
        public ChessGame(string fen)
        {
            GameCreationData data = FenStringToGameCreationData(fen);
            UseGameCreationData(data);
        }

        /// <summary>
        /// 从创建游戏数据（GameCreationData）创建一个游戏。
        /// </summary>
        /// <param name="data"></param>
        public ChessGame(GameCreationData data)
        {
            UseGameCreationData(data);
        }

        /// <summary>
        /// 构造函数，全盘复制一个 ChessGame。
        /// </summary>
        /// <param name="game"></param>
        public ChessGame(ChessGame game)
        {
            GameCreationData gcd = new GameCreationData();
            gcd.Board = game.Board;
            gcd.CanWhiteCastleKingSide = game.CanWhiteCastleKingSide;
            gcd.CanWhiteCastleQueenSide = game.CanWhiteCastleQueenSide;
            gcd.CanBlackCastleKingSide = game.CanBlackCastleKingSide;
            gcd.CanBlackCastleQueenSide = game.CanBlackCastleQueenSide;
            gcd.EnPassant = null;
            gcd.WhoseTurn = game.WhoseTurn;
            if (game._moves.Count > 0)
            {
                DetailedMove last = game._moves.Last();
                if (last.Piece is Pawn && new PositionDistance(last.OriginalPosition, last.NewPosition).DistanceY == 2)
                {
                    gcd.EnPassant = new Position(last.NewPosition.File, last.Player == Player.White ? 3 : 6);
                }
            }
            gcd.HalfMoveClock = game.i_halfMoveClock;
            gcd.FullMoveNumber = game.i_fullMoveNumber;
            UseGameCreationData(gcd);
        }

        [Obsolete("This constructor is obsolete, use ChessGame(GameCreationData) instead.")]
        public ChessGame(Piece[][] board, Player whoseTurn)
        {
            Board = CloneBoard(board);
            _moves = new List<MoreDetailedMove>();
            WhoseTurn = whoseTurn;
            Piece e1 = GetPieceAt(File.E, 1);
            Piece e8 = GetPieceAt(File.E, 8);
            Piece a1 = GetPieceAt(File.A, 1);
            Piece h1 = GetPieceAt(File.H, 1);
            Piece a8 = GetPieceAt(File.A, 8);
            Piece h8 = GetPieceAt(File.H, 8);
            CanBlackCastleKingSide = CanBlackCastleQueenSide = CanWhiteCastleKingSide = CanWhiteCastleQueenSide = CastlingCanBeLegal;
            InitialBlackRookFileKingsideCastling = InitialWhiteRookFileKingsideCastling = File.H;
            InitialBlackRookFileQueensideCastling = InitialWhiteRookFileQueensideCastling = File.A;
            InitialBlackKingFile = InitialWhiteKingFile = File.E;
            if (CastlingCanBeLegal)
            {
                if (!(e1 is King) || e1.Owner != Player.White)
                    CanWhiteCastleKingSide = CanWhiteCastleQueenSide = false;
                if (!(e8 is King) || e8.Owner != Player.Black)
                    CanBlackCastleKingSide = CanBlackCastleQueenSide = false;
                if (!(a1 is Rook) || a1.Owner != Player.White)
                    CanWhiteCastleQueenSide = false;
                if (!(h1 is Rook) || h1.Owner != Player.White)
                    CanWhiteCastleKingSide = false;
                if (!(a8 is Rook) || a8.Owner != Player.Black)
                    CanBlackCastleQueenSide = false;
                if (!(h8 is Rook) || h8.Owner != Player.Black)
                    CanBlackCastleKingSide = false;
            }
        }

        /// <summary>
        /// 被某个接受 GameCreationData 的构造函数调用的，载入这个 GameCreationData 的方法。
        /// </summary>
        /// <param name="data">这个 GameCreationData。</param>
        protected virtual void UseGameCreationData(GameCreationData data)
        {
            Board = CloneBoard(data.Board);
            WhoseTurn = data.WhoseTurn;
            careWhoseTurnItIs = data.careWhoseTurnItIs;

            Piece[] eighthRank = Board[0];
            Piece[] firstRank = Board[7];

            // 可易位性必须要复制
            CanBlackCastleKingSide = CanBlackCastleQueenSide = CanWhiteCastleKingSide = CanWhiteCastleQueenSide = CastlingCanBeLegal;
            InitialWhiteKingFile = (File)Array.IndexOf(firstRank, new King(Player.White));
            InitialBlackKingFile = (File)Array.IndexOf(eighthRank, new King(Player.Black));
            if (CastlingCanBeLegal)
            {
                CanBlackCastleKingSide = data.CanBlackCastleKingSide;
                CanBlackCastleQueenSide = data.CanBlackCastleQueenSide;
                CanWhiteCastleKingSide = data.CanWhiteCastleKingSide;
                CanWhiteCastleQueenSide = data.CanWhiteCastleQueenSide;
            }
            InitialBlackRookFileQueensideCastling = CanBlackCastleQueenSide ? (File)Array.IndexOf(eighthRank, new Rook(Player.Black)) : File.None;
            InitialBlackRookFileKingsideCastling = CanBlackCastleKingSide ? (File)Array.LastIndexOf(eighthRank, new Rook(Player.Black)) : File.None;
            InitialWhiteRookFileQueensideCastling = CanWhiteCastleQueenSide ? (File)Array.IndexOf(firstRank, new Rook(Player.White)) : File.None;
            InitialWhiteRookFileKingsideCastling = CanWhiteCastleKingSide ? (File)Array.LastIndexOf(firstRank, new Rook(Player.White)) : File.None;

            if (InitialBlackRookFileQueensideCastling == File.None) CanBlackCastleQueenSide = false;
            if (InitialBlackRookFileKingsideCastling == File.None) CanBlackCastleKingSide = false;
            if (InitialWhiteRookFileKingsideCastling == File.None) CanWhiteCastleKingSide = false;
            if (InitialWhiteRookFileQueensideCastling == File.None) CanWhiteCastleQueenSide = false;

            // 此段代码存疑。
            if (!data.Moves.Any() && data.EnPassant != null)
            {
                /*Move primMove = new Move(new Position(data.EnPassant.File, data.WhoseTurn == Player.White ? 7 : 2),
                        new Position(data.EnPassant.File, data.WhoseTurn == Player.White ? 5 : 4),
                        ChessUtilities.GetOpponentOf(data.WhoseTurn));
                bool causeCheck = false; // magic
                bool causeCheckmate = false; // magic

                
                MoreDetailedMove latestMove = new MoreDetailedMove(primMove,
                    new Pawn(ChessUtilities.GetOpponentOf(data.WhoseTurn)),
                    false,
                    CastlingType.None,
                    new Pawn(data.WhoseTurn),
                    true,
                    causeCheck,
                    causeCheckmate
                    );
                _moves.Add(latestMove);
                */
            }
            else
            {
                _moves = data.Moves.ToList();
            }

            i_halfMoveClock = data.HalfMoveClock;
            i_fullMoveNumber = data.FullMoveNumber;

            _drawn = data.DrawClaimed;
            _drawReason = data.DrawReason;
            _resigned = data.Resigned;
        }

        /// <summary>
        /// 获取当前局面的 FEN 字符串。
        /// </summary>
        /// <returns></returns>
        public virtual string GetFen()
        {
            StringBuilder fenBuilder = new StringBuilder();
            Piece[][] board = GetBoard();
            for (int i = 0; i < board.Length; i++)
            {
                Piece[] row = board[i];
                int empty = 0;
                foreach (Piece piece in row)
                {
                    char pieceChar = piece == null ? '\0' : piece.GetFenCharacter();
                    if (pieceChar == '\0')
                    {
                        empty++;
                        continue;
                    }
                    if (empty != 0)
                    {
                        fenBuilder.Append(empty);
                        empty = 0;
                    }
                    fenBuilder.Append(pieceChar);
                    if (piece.IsPromotionResult && UseTildesInFenGeneration)
                    {
                        fenBuilder.Append('~');
                    }
                }
                if (empty != 0)
                {
                    fenBuilder.Append(empty);
                }
                if (i != board.Length - 1)
                {
                    fenBuilder.Append('/');
                }
            }

            fenBuilder.Append(' ');

            fenBuilder.Append(WhoseTurn == Player.White ? 'w' : 'b');

            fenBuilder.Append(' ');

            bool hasAnyCastlingOptions = false;


            if (CanWhiteCastleKingSide)
            {
                fenBuilder.Append('K');
                hasAnyCastlingOptions = true;
            }
            if (CanWhiteCastleQueenSide)
            {
                fenBuilder.Append('Q');
                hasAnyCastlingOptions = true;
            }


            if (CanBlackCastleKingSide)
            {
                fenBuilder.Append('k');
                hasAnyCastlingOptions = true;
            }
            if (CanBlackCastleQueenSide)
            {
                fenBuilder.Append('q');
                hasAnyCastlingOptions = true;
            }
            if (!hasAnyCastlingOptions)
            {
                fenBuilder.Append('-');
            }

            fenBuilder.Append(' ');

            DetailedMove last;
            if (Moves.Count > 0 && (last = Moves[Moves.Count - 1]).Piece is Pawn && Math.Abs(last.OriginalPosition.Rank - last.NewPosition.Rank) == 2
                && last.OriginalPosition.Rank == (last.Player == Player.White ? 2 : 7))
            {
                fenBuilder.Append(last.NewPosition.File.ToString().ToLowerInvariant());
                fenBuilder.Append(last.Player == Player.White ? 3 : 6);
            }
            else
            {
                fenBuilder.Append("-");
            }

            fenBuilder.Append(' ');

            fenBuilder.Append(i_halfMoveClock);

            fenBuilder.Append(' ');

            fenBuilder.Append(i_fullMoveNumber);

            return fenBuilder.ToString();
        }

        protected virtual int[] ValidFenBoardRows { get { return new int[1] { 8 }; } }

        /// <summary>
        /// 解析 FEN，并返回对应的 GameCreationData。
        /// </summary>
        /// <param name="fen"></param>
        /// <returns></returns>
        protected virtual GameCreationData FenStringToGameCreationData(string fen)
        {
            Dictionary<char, Piece> fenMappings = FenMappings;
            string[] parts = fen.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (!AllowedFenPartsLength.Contains(parts.Length)) throw new ArgumentException("The FEN string has too much, or too few, parts.");
            Piece[][] board = new Piece[8][];
            string[] rows = parts[0].Split('/');
            if (!ValidFenBoardRows.Contains(rows.Length)) throw new ArgumentException("The board in the FEN string has an invalid number of rows.");
            GameCreationData data = new GameCreationData
            {
                Board = InterpretBoardOfFen(parts[0])
            };

            if (parts[1] == "w")
            {
                data.WhoseTurn = Player.White;
            }
            else if (parts[1] == "b")
            {
                data.WhoseTurn = Player.Black;
            }
            else
            {
                throw new ArgumentException("Expected `w` or `b` for the active player in the FEN string.");
            }

            if (parts[2].Contains("K")) data.CanWhiteCastleKingSide = true;
            else data.CanWhiteCastleKingSide = false;

            if (parts[2].Contains("Q")) data.CanWhiteCastleQueenSide = true;
            else data.CanWhiteCastleQueenSide = false;

            if (parts[2].Contains("k")) data.CanBlackCastleKingSide = true;
            else data.CanBlackCastleKingSide = false;

            if (parts[2].Contains("q")) data.CanBlackCastleQueenSide = true;
            else data.CanBlackCastleQueenSide = false;

            if (parts[3] == "-") data.EnPassant = null;
            else
            {
                Position ep = new Position(parts[3]);
                if ((data.WhoseTurn == Player.White && (ep.Rank != 6 || !(data.Board[3][(int)ep.File] is Pawn))) ||
                    (data.WhoseTurn == Player.Black && (ep.Rank != 3 || !(data.Board[4][(int)ep.File] is Pawn))))
                {
                    throw new ArgumentException("Invalid en passant field in FEN.");
                }
                data.EnPassant = ep;
            }

            int halfmoveClock;
            if (int.TryParse(parts[4], out halfmoveClock))
            {
                data.HalfMoveClock = halfmoveClock;
            }
            else
            {
                throw new ArgumentException("Halfmove clock in FEN is invalid.");
            }

            int fullMoveNumber;
            if (int.TryParse(parts[5], out fullMoveNumber))
            {
                data.FullMoveNumber = fullMoveNumber;
            }
            else
            {
                throw new ArgumentException("Fullmove number in FEN is invalid.");
            }

            return data;
        }


        /// <summary>
        /// 将 FEN 中的 Board 部分解析为 Piece[][] 方便导入 this.Board。
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        protected virtual Piece[][] InterpretBoardOfFen(string board)
        {
            Piece[][] pieceArr = new Piece[8][];
            string[] rows = board.Split('/');
            for (int i = 0; i < 8; i++)
            {
                string row = rows[i];
                Piece[] currentRow = new Piece[8] { null, null, null, null, null, null, null, null };
                int j = 0;
                foreach (char c in row)
                {
                    if (char.IsDigit(c))
                    {
                        j += (int)char.GetNumericValue(c);
                        continue;
                    }
                    if (c == '~')
                    {
                        if (j == 0)
                        {
                            throw new ArgumentException("Error in FEN: misplaced '~'.");
                        }
                        if (currentRow[j - 1] == null)
                        {
                            throw new ArgumentException("Error in FEN: misplaced '~'.");
                        }
                        currentRow[j - 1] = currentRow[j - 1].AsPromotion();
                        continue;
                    }
                    if (!FenMappings.ContainsKey(c)) throw new ArgumentException("The FEN string contains an unknown piece.");
                    currentRow[j] = FenMappings[c];
                    j++;
                }
                if (j != 8)
                {
                    throw new ArgumentException("Not enough pieces provided for a row in the FEN string.");
                }
                pieceArr[i] = currentRow;
            }
            return pieceArr;
        }

        /// <summary>
        /// 获取某个 Position 对应的棋子。
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Piece GetPieceAt(Position position)
        {
            ChessUtilities.ThrowIfNull(position, "position");
            return GetPieceAt(position.File, position.Rank);
        }

        /// <summary>
        /// 获取某行（Rank）某列（File）对应的棋子。
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rank"></param>
        /// <returns></returns>
        public Piece GetPieceAt(File file, int rank)
        {
            return Board[8 - rank][(int)file];
        }

        /// <summary>
        /// 在某行某列设置一个棋子。（如果要移除一个棋子，就把 piece 设为 null）
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rank"></param>
        /// <param name="piece"></param>
        public virtual void SetPieceAt(File file, int rank, Piece piece)
        {
            Board[8 - rank][(int)file] = piece;
        }

        /// <summary>
        /// 重要方法：判断某个 Move 在该游戏中是不是合法的 Move。（符合游戏规则，不能送吃）（传递到有参最多的那个方法。）
        /// </summary>
        /// <param name="move">该 Move。</param>
        /// <returns></returns>
        public bool IsValidMove(Move move)
        {
            ChessUtilities.ThrowIfNull(move, "move");
            return IsValidMove(move, true, careWhoseTurnItIs);
        }

        /// <summary>
        /// 同上。
        /// </summary>
        /// <param name="move">该 Move。</param>
        /// <param name="validateCheck">如果为 true（默认），就会检测是否王送吃。</param>
        /// <returns></returns>
        public bool IsValidMove(Move move, bool validateCheck)
        {
            ChessUtilities.ThrowIfNull(move, "move");
            return IsValidMove(move, validateCheck, careWhoseTurnItIs);
        }

        /// <summary>
        /// 同上。
        /// </summary>
        /// <param name="move"></param>
        /// <param name="validateCheck"></param>
        /// <param name="careAboutWhoseTurnItIs">如果为 true，就不在意当前是谁执子。否则，若这个 Move 是对方做出的，则判为不合法。</param>
        /// <returns></returns>
        public virtual bool IsValidMove(Move move, bool validateCheck, bool careAboutWhoseTurnItIs)
        {
            ChessUtilities.ThrowIfNull(move, "move");
            if (move.OriginalPosition.Equals(move.NewPosition))
                return false;
            Piece piece = GetPieceAt(move.OriginalPosition.File, move.OriginalPosition.Rank);
            if (careAboutWhoseTurnItIs && move.Player != WhoseTurn) return false;
            if (piece == null || piece.Owner != move.Player) return false;
            Piece pieceAtDestination = GetPieceAt(move.NewPosition);
            bool isCastle = pieceAtDestination is Rook && piece is King && pieceAtDestination.Owner == piece.Owner;
            if (pieceAtDestination != null && pieceAtDestination.Owner == move.Player && !isCastle)
            {
                return false;
            }
            else if(move is MoreDetailedMove m)
                if (pieceAtDestination != null && pieceAtDestination.Owner == ChessUtilities.GetOpponentOf(move.Player))
                {
                    m.IsCapture = true;
                    m.CapturedPiece = pieceAtDestination;
                }
            if (!piece.IsValidMove(move, this))
            {
                return false;
            }
            if (validateCheck)
            {
                if (!isCastle && WouldBeInCheckAfter(move, move.Player))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 已知 Move 是一个易位 Move，对游戏应用这个 Move。
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        protected virtual CastlingType ApplyCastle(Move move)
        {
            CastlingType castle;
            int rank = move.Player == Player.White ? 1 : 8;
            File rookFile = move.NewPosition.File;
            if (move.Player == Player.White)
            {
                if (rookFile == File.C && GetPieceAt(File.C, rank) == null && InitialWhiteKingFile == File.E)
                {
                    rookFile = File.A;
                }
                else if (rookFile == File.G && GetPieceAt(File.G, rank) == null && InitialWhiteKingFile == File.E)
                {
                    rookFile = File.H;
                }
            }
            else
            {
                if (rookFile == File.C && GetPieceAt(File.C, rank) == null && InitialBlackKingFile == File.E)
                {
                    rookFile = File.A;
                }
                else if (rookFile == File.G && GetPieceAt(File.G, rank) == null && InitialBlackKingFile == File.E)
                {
                    rookFile = File.H;
                }
            }

            File newRookFile;
            File newKingFile;
            if (rookFile == (move.Player == Player.White ? InitialWhiteRookFileQueensideCastling : InitialBlackRookFileQueensideCastling))
            {
                castle = CastlingType.QueenSide;
                newRookFile = File.D;
                newKingFile = File.C;
            }
            else
            {
                castle = CastlingType.KingSide;
                newRookFile = File.F;
                newKingFile = File.G;
            }
            SetPieceAt(rookFile, rank, null);
            SetPieceAt(move.OriginalPosition.File, rank, null);
            SetPieceAt(newRookFile, rank, new Rook(move.Player));
            SetPieceAt(newKingFile, rank, new King(move.Player));
            return castle;
        }

        /// <summary>
        /// 重要函数。对游戏应用某个 Move。传递到参数最多的 ApplyMove 上。
        /// </summary>
        /// <param name="move">该 Move。</param>
        /// <param name="alreadyValidated">该 Move 是否已经验证过是合法的，就无需再验证。</param>
        /// <returns></returns>
        public virtual MoveType ApplyMove(Move move, bool alreadyValidated)
        {
            Piece captured;
            return ApplyMove(move, alreadyValidated, out captured);
        }

        /// <summary>
        /// 同上。
        /// </summary>
        /// <param name="move">该 Move。</param>
        /// <param name="alreadyValidated">该 Move 是否已经验证过是合法的，就无需再验证。</param>
        /// <param name="captured">是一个传出（out）参数。如果该 Move 吃子，将所吃子送到 captured 上。</param>
        /// <param name="probing">无关紧要。如果为 true，则在最后省略判断重新生成的 MoreDetailedMove 和传入的 MoreDetailedMove 内容是否一致。</param>
        /// <returns></returns>
        public virtual MoveType ApplyMove(Move move, bool alreadyValidated, out Piece captured, bool probing = false)
        {
            ChessGame copy = null;
            if(!(move is MoreDetailedMove mdm && mdm.StoredSANString != null))
            {
                copy = new ChessGame(this);
            }
            ChessUtilities.ThrowIfNull(move, "move");
            captured = null;
            if (!alreadyValidated && !IsValidMove(move))
                return MoveType.Invalid;
            MoveType type = MoveType.Move;
            Piece movingPiece = GetPieceAt(move.OriginalPosition.File, move.OriginalPosition.Rank);
            Piece capturedPiece = GetPieceAt(move.NewPosition.File, move.NewPosition.Rank);
            captured = capturedPiece;
            Piece newPiece = movingPiece;
            bool isCapture = capturedPiece != null;
            CastlingType castle = CastlingType.None;
            bool isEnpassant = false;
            if (movingPiece is Pawn)
            {
                i_halfMoveClock = 0;
                PositionDistance pd = new PositionDistance(move.OriginalPosition, move.NewPosition);
                if (pd.DistanceX == 1 && pd.DistanceY == 1 && GetPieceAt(move.NewPosition) == null)
                { // en passant
                    isCapture = true;
                    isEnpassant = true;
                    captured = GetPieceAt(move.NewPosition.File, move.OriginalPosition.Rank);
                    SetPieceAt(move.NewPosition.File, move.OriginalPosition.Rank, null);
                }
                if (move.NewPosition.Rank == (move.Player == Player.White ? 8 : 1))
                {
                    newPiece = MapPgnCharToPiece(move.Promotion.Value, move.Player).AsPromotion();
                    type |= MoveType.Promotion;
                }
            }
            else if (movingPiece is King)
            {
                if (movingPiece.Owner == Player.White)
                    CanWhiteCastleKingSide = CanWhiteCastleQueenSide = false;
                else
                    CanBlackCastleKingSide = CanBlackCastleQueenSide = false;

                if (CastlingCanBeLegal &&
                    ((GetPieceAt(move.NewPosition) is Rook && GetPieceAt(move.NewPosition).Owner == move.Player) ||
                        ((move.NewPosition.File == File.C || move.NewPosition.File == File.G) &&
                        (move.Player == Player.White ? InitialWhiteKingFile : InitialBlackKingFile) == File.E
                        && move.OriginalPosition.File == File.E)))
                {
                    castle = ApplyCastle(move);
                    type |= MoveType.Castling;
                    isCapture = false;
                    captured = null;
                }
            }
            else if (movingPiece is Rook)
            {
                if (move.Player == Player.White)
                {
                    if (move.OriginalPosition.File == File.A && move.OriginalPosition.Rank == 1)
                        CanWhiteCastleQueenSide = false;
                    else if (move.OriginalPosition.File == File.H && move.OriginalPosition.Rank == 1)
                        CanWhiteCastleKingSide = false;
                }
                else
                {
                    if (move.OriginalPosition.File == File.A && move.OriginalPosition.Rank == 8)
                        CanBlackCastleQueenSide = false;
                    else if (move.OriginalPosition.File == File.H && move.OriginalPosition.Rank == 8)
                        CanBlackCastleKingSide = false;
                }
            }
            if (isCapture)
            {
                type |= MoveType.Capture;
                i_halfMoveClock = 0;
                if (move.NewPosition.File == File.A && move.NewPosition.Rank == 1)
                    CanWhiteCastleQueenSide = false;
                else if (move.NewPosition.File == File.H && move.NewPosition.Rank == 1)
                    CanWhiteCastleKingSide = false;
                else if (move.NewPosition.File == File.A && move.NewPosition.Rank == 8)
                    CanBlackCastleQueenSide = false;
                else if (move.NewPosition.File == File.H && move.NewPosition.Rank == 8)
                    CanBlackCastleKingSide = false;
            }
            if (!isCapture && !(movingPiece is Pawn))
            {
                i_halfMoveClock++;
                if (i_halfMoveClock >= 100)
                {
                    fiftyMoves = true;
                }
                else
                {
                    fiftyMoves = false;
                }
            }
            if (move.Player == Player.Black)
            {
                i_fullMoveNumber++;
            }
            if (castle == CastlingType.None)
            {
                SetPieceAt(move.NewPosition.File, move.NewPosition.Rank, newPiece);
                SetPieceAt(move.OriginalPosition.File, move.OriginalPosition.Rank, null);
            }
            WhoseTurn = ChessUtilities.GetOpponentOf(move.Player);
            if(!probing && move is MoreDetailedMove m)
            {
                var mNew = new MoreDetailedMove(move, movingPiece, isCapture, castle, captured, isEnpassant, IsInCheck(WhoseTurn, false), IsCheckmated(WhoseTurn, false));
                bool twoMovesEqual = mNew.Equals(m);
                if (!twoMovesEqual)
                {
                    throw new Exception("Two MoreDetailedMove differ.");
                }
                if(m.StoredSANString == null)
                    m.GenerateSANString(copy);
                AddMoreDetailedMove(m);
            }
            else
            {
                var mNew = new MoreDetailedMove(move, movingPiece, isCapture, castle, captured, isEnpassant, IsInCheck(WhoseTurn, false), IsCheckmated(WhoseTurn, false));
                mNew.GenerateSANString(copy);
                AddMoreDetailedMove(mNew);
            }
            return type;
        }

        /// <summary>
        /// 将某个 MoreDetailedMove 加入到游戏的历史走子中。
        /// </summary>
        /// <param name="dm"></param>
        protected virtual void AddMoreDetailedMove(MoreDetailedMove dm)
        {
            _moves.Add(dm);
        }

        /// <summary>
        /// 重要函数。返回所有从 Position from 开始的合法走子。传递到最多参的 GetValidMoves 运行。
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public ReadOnlyCollection<MoreDetailedMove> GetValidMoves(Position from)
        {
            ChessUtilities.ThrowIfNull(from, "from");
            return GetValidMoves(from, false);
        }

        public virtual ReadOnlyCollection<MoreDetailedMove> GetValidMoves(Position from, bool returnIfAny)
        {
            return GetValidMoves(from, returnIfAny, careWhoseTurnItIs);
        }

        /// <summary>
        /// 重要函数。返回所有从 Position from 开始的合法走子，返回 MoreDetailedMove 的只读集合。传递到最多参的 GetValidMoves 运行。
        /// </summary>
        /// <param name="from">合法走子的出发点。</param>
        /// <param name="returnIfAny">如果找到一个，立即返回，不找其他的。</param>
        /// <param name="careAboutWhoseTurnItIs">是否限定哪方执子。</param>
        /// <returns></returns>
        public virtual ReadOnlyCollection<MoreDetailedMove> GetValidMoves(Position from, bool returnIfAny, bool careAboutWhoseTurnItIs)
        {
            ChessUtilities.ThrowIfNull(from, "from");
            Piece piece = GetPieceAt(from);
            if (piece == null || (careAboutWhoseTurnItIs && piece.Owner != WhoseTurn)) return new ReadOnlyCollection<MoreDetailedMove>(new List<MoreDetailedMove>());
            return piece.GetValidMoves(from, returnIfAny, this, IsValidMove);
        }

        public ReadOnlyCollection<MoreDetailedMove> GetValidMoves(Player player)
        {
            return GetValidMoves(player, false);
        }

        public virtual ReadOnlyCollection<MoreDetailedMove> GetValidMoves(Player player, bool returnIfAny)
        {
            return GetValidMoves(player, returnIfAny, careWhoseTurnItIs);
        }

        /// <summary>
        /// 重要函数。返回某一方 Player 的所有合法走子。遍历棋盘，调用基于 Position 的 GetValidMoves 来实现。
        /// </summary>
        /// <param name="player"></param>
        /// <param name="returnIfAny"></param>
        /// <param name="careAboutWhoseTurnItIs"></param>
        /// <returns></returns>
        public virtual ReadOnlyCollection<MoreDetailedMove> GetValidMoves(Player player, bool returnIfAny, bool careAboutWhoseTurnItIs)
        {
            if (careAboutWhoseTurnItIs && player != WhoseTurn) return new ReadOnlyCollection<MoreDetailedMove>(new List<MoreDetailedMove>());
            List<MoreDetailedMove> validMoves = new List<MoreDetailedMove>();
            for (int r = 1; r <= Board.Length; r++)
            {
                for (int f = 0; f < Board[8 - r].Length; f++)
                {
                    Piece p = GetPieceAt((File)f, r);
                    if (p != null && p.Owner == player)
                    {
                        validMoves.AddRange(GetValidMoves(new Position((File)f, r), returnIfAny));
                        if (returnIfAny && validMoves.Count > 0)
                        {
                            return new ReadOnlyCollection<MoreDetailedMove>(validMoves);
                        }
                    }
                }
            }
            return new ReadOnlyCollection<MoreDetailedMove>(validMoves);
        }

        public virtual bool HasAnyValidMoves(Position from)
        {
            ChessUtilities.ThrowIfNull(from, "from");
            ReadOnlyCollection<MoreDetailedMove> validMoves = GetValidMoves(from, true);
            return validMoves.Count > 0;
        }

        public virtual bool HasAnyValidMoves(Position from, bool careAboutWhoseTurnItIs)
        {
            ChessUtilities.ThrowIfNull(from, "from");
            ReadOnlyCollection<MoreDetailedMove> validMoves = GetValidMoves(from, true, careAboutWhoseTurnItIs);
            return validMoves.Count > 0;
        }

        public virtual bool HasAnyValidMoves(Player player)
        {
            ReadOnlyCollection<MoreDetailedMove> validMoves = GetValidMoves(player, true);
            return validMoves.Count > 0;
        }

        public virtual bool HasAnyValidMoves(Player player, bool careAboutWhoseTurnItIs)
        {
            ReadOnlyCollection<MoreDetailedMove> validMoves = GetValidMoves(player, true, careAboutWhoseTurnItIs);
            return validMoves.Count > 0;
        }

        protected Cache<bool> inCheckCacheWhite = new Cache<bool>(false, -1);
        protected Cache<bool> inCheckCacheBlack = new Cache<bool>(false, -1);
        /// <summary>
        /// 返回某方 Player 是否被将军。
        /// </summary>
        /// <param name="player">该 Player。</param>
        /// <param name="useCache">是否使用缓存机制。</param>
        /// <returns></returns>
        public virtual bool IsInCheck(Player player, bool useCache = true)
        {
            if (player == Player.None)
            {
                throw new ArgumentException("IsInCheck: Player.None is an invalid argument.");
            }

            Cache<bool> cache = player == Player.White ? inCheckCacheWhite : inCheckCacheBlack;

            if (useCache)
            {
                if (cache.CachedAt == Moves.Count)
                {
                    return cache.Value;
                }
            }

            Position kingPos = new Position(File.None, -1);

            for (int r = 1; r <= Board.Length; r++)
            {
                for (int f = 0; f < Board[8 - r].Length; f++)
                {
                    Piece curr = GetPieceAt((File)f, r);
                    if (curr is King && curr.Owner == player)
                    {
                        kingPos = new Position((File)f, r);
                        break;
                    }
                }
                if (kingPos != new Position(File.None, -1))
                {
                    break;
                }
            }

            if (kingPos.File == File.None)
                return cache.UpdateCache(false, Moves.Count);

            for (int r = 1; r <= Board.Length; r++)
            {
                for (int f = 0; f < Board[8 - r].Length; f++)
                {
                    Piece curr = GetPieceAt((File)f, r);
                    if (curr == null) continue;
                    Player p = curr.Owner;
                    Move move = new Move(new Position((File)f, r), kingPos, p);
                    List<Move> moves = new List<Move>();
                    if (curr is Pawn && ((move.NewPosition.Rank == 8 && move.Player == Player.White) || (move.NewPosition.Rank == 1 && move.Player == Player.Black)))
                    {
                        moves.Add(new Move(move.OriginalPosition, move.NewPosition, move.Player, 'Q'));
                        moves.Add(new Move(move.OriginalPosition, move.NewPosition, move.Player, 'R'));
                        moves.Add(new Move(move.OriginalPosition, move.NewPosition, move.Player, 'B'));
                        moves.Add(new Move(move.OriginalPosition, move.NewPosition, move.Player, 'N'));
                        moves.Add(new Move(move.OriginalPosition, move.NewPosition, move.Player, 'K'));
                    }
                    else
                    {
                        moves.Add(move);
                    }
                    foreach (Move m in moves)
                    {
                        if (IsValidMove(m, false, false))
                        {
                            return cache.UpdateCache(true, Moves.Count);
                        }
                    }
                }
            }
            return cache.UpdateCache(false, Moves.Count);
        }

        protected Cache<bool> checkmatedCacheWhite = new Cache<bool>(false, -1);
        protected Cache<bool> checkmatedCacheBlack = new Cache<bool>(false, -1);
        /// <summary>
        /// 返回某方 Player 是否被将死。
        /// </summary>
        /// <param name="player"></param>
        /// <param name="useCache"></param>
        /// <returns></returns>
        public virtual bool IsCheckmated(Player player, bool useCache = true)
        {
            Cache<bool> cache = player == Player.White ? checkmatedCacheWhite : checkmatedCacheBlack;
            if (useCache)
            {
                if (cache.CachedAt == Moves.Count)
                {
                    return cache.Value;
                }
            }

            return cache.UpdateCache(IsInCheck(player) && !HasAnyValidMoves(player), Moves.Count);
        }

        protected Cache<bool> stalematedCacheWhite = new Cache<bool>(false, -1);
        protected Cache<bool> stalematedCacheBlack = new Cache<bool>(false, -1);
        /// <summary>
        /// 返回某方 Player 是否僵局。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual bool IsStalemated(Player player)
        {
            Cache<bool> cache = player == Player.White ? stalematedCacheWhite : stalematedCacheBlack;
            if (cache.CachedAt == Moves.Count)
            {
                return cache.Value;
            }

            return cache.UpdateCache(WhoseTurn == player && !IsInCheck(player) && !HasAnyValidMoves(player), Moves.Count);
        }

        public virtual bool IsStalemated(Player player, bool careWhoseTurnItIs)
        {
            Cache<bool> cache = player == Player.White ? stalematedCacheWhite : stalematedCacheBlack;
            if (cache.CachedAt == Moves.Count)
            {
                return cache.Value;
            }

            return cache.UpdateCache((!careWhoseTurnItIs || WhoseTurn == player) && !IsInCheck(player) && !HasAnyValidMoves(player), Moves.Count);
        }

        public virtual bool IsWinner(Player player)
        {
            return IsCheckmated(ChessUtilities.GetOpponentOf(player));
        }

        public virtual bool IsDraw()
        {
            return DrawClaimed || IsStalemated(Player.White) || IsStalemated(Player.Black);
        }

        /// <summary>
        /// 在做了某个 Move 之后，某个 Player 是否被将军。有 Bug，无法处理该落子后出现兵晋升的情况。
        /// </summary>
        /// <param name="move"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual bool WouldBeInCheckAfter(Move move, Player player)
        {
            ChessUtilities.ThrowIfNull(move, "move");
            GameCreationData gcd = new GameCreationData();
            gcd.Board = Board;
            gcd.CanWhiteCastleKingSide = CanWhiteCastleKingSide;
            gcd.CanWhiteCastleQueenSide = CanWhiteCastleQueenSide;
            gcd.CanBlackCastleKingSide = CanBlackCastleKingSide;
            gcd.CanBlackCastleQueenSide = CanBlackCastleQueenSide;
            gcd.WhoseTurn = WhoseTurn;
            gcd.EnPassant = null;
            if (_moves.Count > 0)
            {
                DetailedMove last = _moves.Last();
                if (last.Piece is Pawn && new PositionDistance(last.OriginalPosition, last.NewPosition).DistanceY == 2)
                {
                    gcd.EnPassant = new Position(last.NewPosition.File, last.Player == Player.White ? 3 : 6);
                }
            }
            gcd.HalfMoveClock = i_halfMoveClock;
            gcd.FullMoveNumber = i_fullMoveNumber;
            ChessGame copy = new ChessGame(gcd);
            // 无法处理该落子后出现兵晋升的情况
            Piece p = copy.GetPieceAt(move.OriginalPosition);
            copy.SetPieceAt(move.OriginalPosition.File, move.OriginalPosition.Rank, null);
            copy.SetPieceAt(move.NewPosition.File, move.NewPosition.Rank, p);
            return copy.IsInCheck(player);
        }

        /// <summary>
        /// 在做了某个 Move 之后，某个 Player 是否被将军，和被将死。用两个传出（out）参数来传递。可以处理该落子后出现兵晋升的情况。
        /// </summary>
        /// <param name="move"></param>
        /// <param name="player"></param>
        /// <param name="inCheck"></param>
        /// <param name="checkmated"></param>
        public virtual void WouldBeInCheckOrCheckmatedAfter(Move move, Player player, out bool inCheck, out bool checkmated)
        {
            ChessUtilities.ThrowIfNull(move, "move");
            GameCreationData gcd = new GameCreationData();
            gcd.Board = Board;
            gcd.CanWhiteCastleKingSide = CanWhiteCastleKingSide;
            gcd.CanWhiteCastleQueenSide = CanWhiteCastleQueenSide;
            gcd.CanBlackCastleKingSide = CanBlackCastleKingSide;
            gcd.CanBlackCastleQueenSide = CanBlackCastleQueenSide;
            gcd.WhoseTurn = WhoseTurn;
            gcd.EnPassant = null;
            if (_moves.Count > 0)
            {
                DetailedMove last = _moves.Last();
                if (last.Piece is Pawn && new PositionDistance(last.OriginalPosition, last.NewPosition).DistanceY == 2)
                {
                    gcd.EnPassant = new Position(last.NewPosition.File, last.Player == Player.White ? 3 : 6);
                }
            }
            gcd.HalfMoveClock = i_halfMoveClock;
            gcd.FullMoveNumber = i_fullMoveNumber;
            ChessGame copy = new ChessGame(gcd);
            copy.ApplyMove(move, false, out Piece captured, true);
            if(move is MoreDetailedMove m)
            {
                m.AssociatedGameAfterMove = copy;
            }
            inCheck = copy.IsInCheck(player);
            checkmated = copy.IsCheckmated(player);
        }

        public void ClaimDraw(string reason)
        {
            _drawn = true;
            _drawReason = reason;
        }

        public void Resign(Player player)
        {
            _resigned = player;
        }

        /// <summary>
        /// 根据这个 ChessGame，创建一个 GameCreationData 并返回。
        /// </summary>
        /// <returns></returns>
        public GameCreationData GetGameCreationData()
        {
            return new GameCreationData
            {
                Board = Board,
                CanBlackCastleKingSide = CanBlackCastleKingSide,
                CanBlackCastleQueenSide = CanBlackCastleQueenSide,
                CanWhiteCastleKingSide = CanWhiteCastleKingSide,
                CanWhiteCastleQueenSide = CanWhiteCastleQueenSide,
                Moves = Moves.Select(x => x).ToArray(),
                FullMoveNumber = i_fullMoveNumber,
                HalfMoveClock = i_halfMoveClock,
                WhoseTurn = WhoseTurn,
                DrawClaimed = DrawClaimed,
                DrawReason = DrawReason,
                Resigned = Resigned
            };
        }
    }
}
