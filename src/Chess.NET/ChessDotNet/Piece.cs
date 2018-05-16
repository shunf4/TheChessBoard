using System;
using System.Collections.ObjectModel;

namespace ChessDotNet
{
    /// <summary>
    /// <para>这是表示棋子的抽象类 Piece。一个 Piece 可以表示一个棋子的【种类】，所以它是个抽象类，必须有具体的类继承它才能起作用。</para>
    /// <para>例如，Bishop 继承自 Piece，实现“象”的特性。</para>
    /// </summary>
    public abstract class Piece
    {
        /// <summary>
        /// 返回该棋子的所有者（也就是这个棋子是白棋还是黑棋）。
        /// </summary>
        public abstract Player Owner
        {
            get;
            set;
        }

        /// <summary>
        /// 返回这个棋子是否是晋升的结果。（用来推断某一个 Move 是否合法时，生成新棋子时使用。）
        /// </summary>
        public abstract bool IsPromotionResult
        {
            get;
            set;
        }

        /// <summary>
        /// 返回同种，颜色相异的棋子。
        /// </summary>
        /// <returns>返回那个棋子。</returns>
        public abstract Piece GetWithInvertedOwner();

        public abstract Piece AsPromotion();

        /// <summary>
        /// 返回两个 Piece 对象是否相等的结果。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null || GetType() != obj.GetType())
                return false;
            Piece piece1 = this;
            Piece piece2 = (Piece)obj;
            return piece1.Owner == piece2.Owner;
        }

        /// <summary>
        /// 返回该棋子的哈希值，用于查询。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return new { Piece = GetFenCharacter(), Owner }.GetHashCode();
        }

        public static bool operator ==(Piece piece1, Piece piece2)
        {
            if (ReferenceEquals(piece1, piece2))
                return true;
            if ((object)piece1 == null || (object)piece2 == null)
                return false;
            return piece1.Equals(piece2);
        }

        public static bool operator !=(Piece piece1, Piece piece2)
        {
            if (ReferenceEquals(piece1, piece2))
                return false;
            if ((object)piece1 == null || (object)piece2 == null)
                return true;
            return !piece1.Equals(piece2);
        }

        /// <summary>
        /// 返回该棋子的 FEN 表示法。如白后记作 ‘Q’，黑后记作 ‘q’。
        /// </summary>
        /// <returns></returns>
        public abstract char GetFenCharacter();

        /// <summary>
        /// 返回该棋子的中文描述。
        /// </summary>
        /// <returns></returns>
        public abstract string GetFriendlyName();

        /// <summary>
        /// 判断这个棋子做出的某一个 Move，在某一个特定的 ChessGame 里是否是合法的。
        /// </summary>
        /// <param name="move">该 Move。</param>
        /// <param name="game">该 ChessGame。</param>
        /// <returns>布尔值，表明是否合法。</returns>
        public abstract bool IsValidMove(Move move, ChessGame game);

        /// <summary>
        /// 针对这个棋子类型，给出这个棋子在某一个 Position，在某一个 ChessGame 里，用 gameMoveValidator（在我们的项目里就是 ChessGame 类里的 IsValidMove）来判断这个棋子移动是否合法，来生成所有合法的移动（Move）。
        /// </summary>
        /// <param name="from">该棋子出发的 Position。</param>
        /// <param name="returnIfAny">为 true ：只要找到一个，就立刻返回。</param>
        /// <param name="game">该 ChessGame。</param>
        /// <param name="gameMoveValidator">见上。</param>
        /// <returns>一个 MoreDetailedMove 的只读集合（ReadOnlyCollection），装了合法的移动。</returns>
        public abstract ReadOnlyCollection<MoreDetailedMove> GetValidMoves(Position from, bool returnIfAny, ChessGame game, Func<Move, bool> gameMoveValidator);
    }
}
