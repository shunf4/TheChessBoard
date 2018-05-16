using System.Text;

namespace ChessDotNet
{
    /// <summary>
    /// 最基础的 Move 类，记录一个走子的基础信息。
    /// </summary>
    public class Move
    {
        /// <summary>
        /// 原 Position。
        /// </summary>
        public Position OriginalPosition
        {
            get;
            set;
        }

        /// <summary>
        /// 新 Position。
        /// </summary>
        public Position NewPosition
        {
            get;
            set;
        }

        /// <summary>
        /// 做出这个 Move 的 Player。
        /// </summary>
        public Player Player
        {
            get;
            set;
        }

        /// <summary>
        /// 如果这个 Move 里有兵的晋升操作，则设为 true。
        /// </summary>
        public char? Promotion
        {
            get;
            set;
        }

        protected Move() { }

        public Move(Position originalPosition, Position newPosition, Player player)
            : this(originalPosition, newPosition, player, null)
        { }

        public Move(string originalPosition, string newPosition, Player player)
            : this(originalPosition, newPosition, player, null)
        { }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="originalPosition"></param>
        /// <param name="newPosition"></param>
        /// <param name="player"></param>
        /// <param name="promotion"></param>
        public Move(Position originalPosition, Position newPosition, Player player, char? promotion)
        {
            OriginalPosition = originalPosition;
            NewPosition = newPosition;
            Player = player;
            if (promotion.HasValue)
            {
                Promotion = char.ToUpper(promotion.Value);
            }
            else
            {
                Promotion = null;
            }
        }

        public Move(string originalPosition, string newPosition, Player player, char? promotion)
        {
            OriginalPosition = new Position(originalPosition);
            NewPosition = new Position(newPosition);
            Player = player;
            if (promotion.HasValue)
            {
                Promotion = char.ToUpper(promotion.Value);
            }
            else
            {
                Promotion = null;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            Move move1 = this;
            Move move2 = (Move)obj;
            return move1.OriginalPosition.Equals(move2.OriginalPosition)
                && move1.NewPosition.Equals(move2.NewPosition)
                && move1.Player == move2.Player
                && move1.Promotion == move2.Promotion;
        }

        public override int GetHashCode()
        {
            return new { OriginalPosition, NewPosition, Player, Promotion }.GetHashCode();
        }

        public static bool operator ==(Move move1, Move move2)
        {
            if (ReferenceEquals(move1, move2))
                return true;
            if ((object)move1 == null || (object)move2 == null)
                return false;
            return move1.Equals(move2);
        }

        public static bool operator !=(Move move1, Move move2)
        {
            if (ReferenceEquals(move1, move2))
                return false;
            if ((object)move1 == null || (object)move2 == null)
                return true;
            return !move1.Equals(move2);
        }

        public override string ToString()
        {
            return OriginalPosition.ToString() + "-" + NewPosition.ToString();
        }
    }
}
