
namespace ChessDotNet
{
    /// <summary>
    /// Move 的派生类，额外多了一些描述这个走子（Move）的属性。现已弃用，全面使用 MoreDetailedMove。
    /// </summary>
    public class DetailedMove : Move
    {
        /// <summary>
        /// 哪个 Piece 走的子。
        /// </summary>
        public Piece Piece
        {
            get;
            set;
        }

        /// <summary>
        /// 这个 Move 有没有吃子。
        /// </summary>
        public bool IsCapture
        {
            get;
            set;
        }

        /// <summary>
        /// 这个 Move 是不是易位，以及易位的类型。
        /// </summary>
        public CastlingType Castling
        {
            get;
            set;
        }

        /// <summary>
        /// 无参构造函数。
        /// </summary>
        protected DetailedMove() { }

        /// <summary>
        /// 返回这个 DetailedMove 的文字描述。
        /// </summary>
        public virtual string FriendlyText
        {
            get
            {
                var sb = new System.Text.StringBuilder();
                sb.Append(Piece.GetFriendlyName());
                sb.Append("从");
                sb.Append(OriginalPosition.ToString());
                sb.Append("到");
                sb.Append(NewPosition.ToString());
                if (IsCapture)
                {
                    sb.Append(", 吃子");
                }
                if (Promotion.HasValue)
                {
                    sb.Append(", 晋升为");
                    sb.Append(ChessGame.OriginalMapPgnCharToPiece(Promotion.Value, Piece.Owner).GetFriendlyName());
                }
                if(Castling.Equals(CastlingType.KingSide))
                {
                    sb.Append(", 王翼易位");
                }
                if (Castling.Equals(CastlingType.QueenSide))
                {
                    sb.Append(", 后翼易位");
                }
                return sb.ToString();
            }
        }

        public DetailedMove(Position originalPosition, Position newPosition, Player player, char? promotion, Piece piece, bool isCapture, CastlingType castling) : 
            base(originalPosition, newPosition, player, promotion)
        {
            Piece = piece;
            IsCapture = isCapture;
            Castling = castling;
        }

        public DetailedMove(Move move, Piece piece, bool isCapture, CastlingType castling)
            : this(move.OriginalPosition, move.NewPosition, move.Player, move.Promotion, piece, isCapture, castling)
        {
        }


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            DetailedMove move = (DetailedMove)obj;
            return OriginalPosition.Equals(move.OriginalPosition)
                && NewPosition.Equals(move.NewPosition)
                && Player == move.Player
                && Promotion == move.Promotion
                && Piece == move.Piece
                && IsCapture == move.IsCapture
                && Castling == move.Castling;
        }

        public override int GetHashCode()
        {
            return new { OriginalPosition, NewPosition, Player, Promotion, Piece, IsCapture, Castling }.GetHashCode();
        }

        public static bool operator ==(DetailedMove move1, DetailedMove move2)
        {
            if (ReferenceEquals(move1, move2))
                return true;
            if ((object)move1 == null || (object)move2 == null)
                return false;
            return move1.Equals(move2);
        }

        public static bool operator !=(DetailedMove move1, DetailedMove move2)
        {
            if (ReferenceEquals(move1, move2))
                return false;
            if ((object)move1 == null || (object)move2 == null)
                return true;
            return !move1.Equals(move2);
        }


    }
}
