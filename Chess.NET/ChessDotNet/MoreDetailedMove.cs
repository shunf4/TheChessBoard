using System.Collections.Generic;
namespace ChessDotNet
{
    /// <summary>
    /// 是 Move 的派生类，现在已经全面使用，记录了最详细的这个 Move 的信息。
    /// </summary>
    public class MoreDetailedMove : DetailedMove
    {
        /// <summary>
        /// 这个 MoreDetailedMove 关联到的 ChessGame（在这个 MoreDetailedMove 做出之前的状态）。因为要输出 SAN，以及文字描述必须和 ChessGame 挂钩。
        /// </summary>
        public ChessGame AssociatedGame
        {
            get;
            set;
        }

        /// <summary>
        /// 弃用。关联到的 ChessGame （在 Move 做出之后的状态）。
        /// </summary>
        public ChessGame AssociatedGameAfterMove
        {
            get;
            set;
        }

        /// <summary>
        /// 如果该走子吃子，则 CapturedPiece 是被吃子；否则为 null。
        /// </summary>
        public Piece CapturedPiece
        {
            get;
            set;
        }

        /// <summary>
        /// 记录这个走子是否吃过路兵。
        /// </summary>
        public bool IsEnpassant
        {
            get;
            set;
        }

        /// <summary>
        /// 记录这个走子是否将军。
        /// </summary>
        public bool? IsChecking
        {
            get;
            set;
        }

        /// <summary>
        /// 记录这个走子是否将死。
        /// </summary>
        public bool? IsCheckmate
        {
            get;
            set;
        }

        /// <summary>
        /// 如果之前已经计算过 SAN 字符串，则存在这里供以后取用。
        /// </summary>
        private string _storedSANString
        {
            get;
            set;
        }

        public string StoredSANString
        {
            get
            {
                return _storedSANString;
            }
        }

        /// <summary>
        /// 如果之前已经计算过文字描述，则存在这里供以后取用。
        /// </summary>
        private string _storedFriendlyText
        {
            get;
            set;
        }

        public string StoredFriendlyText
        {
            get
            {
                return _storedFriendlyText;
            }
        }

        public override string FriendlyText
        {
            get
            {
                if (_storedFriendlyText == null)
                {
                    GenerateFriendlyText();
                }
                return _storedFriendlyText;
            }
        }

        public string SANString
        {
            get
            {
                if (_storedSANString == null)
                {
                    ChessUtilities.ThrowIfNull(AssociatedGame, "AssociatedGame");
                    GenerateSANString(AssociatedGame);
                }
                return _storedSANString;
            }
        }

        protected MoreDetailedMove() { }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="originalPosition"></param>
        /// <param name="newPosition"></param>
        /// <param name="player"></param>
        /// <param name="promotion"></param>
        /// <param name="piece"></param>
        /// <param name="isCapture"></param>
        /// <param name="castling"></param>
        /// <param name="capturedPiece"></param>
        /// <param name="isEnpassant"></param>
        /// <param name="isChecking"></param>
        /// <param name="isCheckmate"></param>
        public MoreDetailedMove(Position originalPosition, Position newPosition, Player player, char? promotion, Piece piece, bool isCapture, CastlingType castling, Piece capturedPiece, bool isEnpassant, bool isChecking, bool isCheckmate) : base(originalPosition, newPosition, player, promotion, piece, isCapture, castling)
        {
            CapturedPiece = capturedPiece;
            IsEnpassant = isEnpassant;
            IsChecking = isChecking;
            IsCheckmate = isCheckmate;
        }

        public MoreDetailedMove(Move move, Piece piece, bool isCapture, CastlingType castling, Piece capturedPiece, bool isEnpassant, bool isChecking, bool isCheckmate) : this(move.OriginalPosition, move.NewPosition, move.Player, move.Promotion, piece, isCapture, castling, capturedPiece, isEnpassant, isChecking, isCheckmate)
        { }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            MoreDetailedMove move = (MoreDetailedMove)obj;
            return OriginalPosition.Equals(move.OriginalPosition)
                && NewPosition.Equals(move.NewPosition)
                && Player == move.Player
                && Promotion == move.Promotion
                && Piece == move.Piece
                && IsCapture == move.IsCapture
                && Castling == move.Castling
                && CapturedPiece == move.CapturedPiece
                && IsChecking == move.IsChecking
                && IsEnpassant == move.IsEnpassant
                && IsCheckmate == move.IsCheckmate;
        }

        public override int GetHashCode()
        {
            return new { OriginalPosition, NewPosition, Player, Promotion, Piece, IsCapture, Castling, CapturedPiece, IsChecking, IsEnpassant, IsCheckmate}.GetHashCode();
        }

        public static bool operator ==(MoreDetailedMove move1, MoreDetailedMove move2)
        {
            if (ReferenceEquals(move1, move2))
                return true;
            if ((object)move1 == null || (object)move2 == null)
                return false;
            return move1.Equals(move2);
        }

        public static bool operator !=(MoreDetailedMove move1, MoreDetailedMove move2)
        {
            if (ReferenceEquals(move1, move2))
                return false;
            if ((object)move1 == null || (object)move2 == null)
                return true;
            return !move1.Equals(move2);
        }

        /// <summary>
        /// 生成文字描述。
        /// </summary>
        public void GenerateFriendlyText()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(Piece.GetFriendlyName());
            sb.Append("从");
            sb.Append(OriginalPosition.ToString());
            sb.Append("到");
            sb.Append(NewPosition.ToString());
            if (IsCapture && CapturedPiece != null)
            {
                sb.Append(", 吃");
                sb.Append(CapturedPiece.GetFriendlyName());
            }
            if (Promotion.HasValue)
            {
                sb.Append(", 晋升为");
                sb.Append(ChessGame.OriginalMapPgnCharToPiece(Promotion.Value, Player).GetFriendlyName());
            }
            if (Castling.Equals(CastlingType.KingSide))
            {
                sb.Append(", 王翼易位");
            }
            if (Castling.Equals(CastlingType.QueenSide))
            {
                sb.Append(", 后翼易位");
            }
            if(IsEnpassant)
            {
                sb.Append(", 吃过路兵");
            }
            if (IsCheckmate.HasValue && IsCheckmate.Value)
            {
                sb.Append(", 将死");
            } else if (IsChecking.HasValue && IsChecking.Value)
            {
                sb.Append(", 将军");
            }
            this._storedFriendlyText = sb.ToString();
        }

        /// <summary>
        /// 生成 SAN 字符串。
        /// </summary>
        /// <param name="gameBeforeTheMove"></param>
        /// <returns></returns>
        public string GenerateSANString(ChessGame gameBeforeTheMove)
        {
            string SANResult;
            if (Castling.Equals(CastlingType.KingSide))
            {
                SANResult = "O-O";
            }
            else if (Castling.Equals(CastlingType.QueenSide))
            {
                SANResult = "O-O-O";
            }
            else
            {
                var sb = new System.Text.StringBuilder();

                if (!(Piece is Pieces.Pawn))
                {
                    sb.Append(char.ToUpper(Piece.GetFenCharacter()));
                }

                Piece[][] board = gameBeforeTheMove.GetBoard();
                List<Move> validMoves = new List<Move>();
                for (int r = 0; r < 8; r++)
                {
                    for (int f = 0; f < 8; f++)
                    {
                        if (board[r][f] != Piece) continue;
                        Move m = new Move(new Position((File)f, 8 - r), this.NewPosition, this.Player, this.Promotion);
                        if (gameBeforeTheMove.IsValidMove(m))
                        {
                            validMoves.Add(m);
                        }
                    }
                }
                if (validMoves.Count == 0) throw new PgnException("This move " + this.ToString() + " is not valid for gameBeforeTheMove.");
                else if (validMoves.Count > 1)
                {
                    bool fileUnique = true;
                    bool rankUnique = true;
                    foreach (var move in validMoves)
                    {
                        if(!(move.OriginalPosition.Equals(this.OriginalPosition)))
                        {
                            if (move.OriginalPosition.File == this.OriginalPosition.File)
                            {
                                fileUnique = false;
                            }
                            if (move.OriginalPosition.Rank == this.OriginalPosition.Rank)
                            {
                                rankUnique = false;
                            }
                        }
                    }

                    if (fileUnique)
                        sb.Append((char)((int)'a' + (int)this.OriginalPosition.File));
                    else if (rankUnique)
                        sb.Append(this.OriginalPosition.Rank.ToString());
                    else
                    {
                        sb.Append((char)((int)'a' + (int)this.OriginalPosition.File));
                        sb.Append(this.OriginalPosition.Rank.ToString());
                    }
                }

                if (IsCapture)
                    sb.Append("x");

                sb.Append(this.NewPosition.ToString().ToLower());

                if (Promotion.HasValue)
                {
                    sb.Append("=");
                    sb.Append(Promotion.Value);
                }

                if (IsCheckmate.HasValue && IsCheckmate.Value)
                {
                    sb.Append("#");
                }
                else if (IsChecking.HasValue && IsChecking.Value)
                {
                    sb.Append("+");
                }

                SANResult = sb.ToString();
            }
            try
            {
                ChessDotNet.PgnMoveReader.ParseMove(SANResult, Player, gameBeforeTheMove);
            }
            catch (PgnException)
            {
                throw new System.ArgumentException("This move " + SANResult + " is not valid for gameBeforeTheMove.");
            }
            catch (System.ArgumentException)
            {
                throw new System.ArgumentException("This move " + SANResult + " is not valid for gameBeforeTheMove.");
            }
            this._storedSANString = SANResult;
            return SANResult;
        }
        
    }
}
