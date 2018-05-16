namespace ChessDotNet
{
    public class GameCreationData
    {
        public Piece[][] Board
        {
            get;
            set;
        }

        public MoreDetailedMove[] Moves
        {
            get;
            set;
        } = new MoreDetailedMove[] {};

        public bool DrawClaimed
        {
            get;
            set;
        }

        public string DrawReason
        {
            get;
            set;
        }

        public Player Resigned
        {
            get;
            set;
        } = Player.None;

        public Player WhoseTurn
        {
            get;
            set;
        }

        public bool careWhoseTurnItIs
        {
            get;
            set;
        }

        public bool CanWhiteCastleKingSide
        {
            get;
            set;
        }

        public bool CanWhiteCastleQueenSide
        {
            get;
            set;
        }

        public bool CanBlackCastleKingSide
        {
            get;
            set;
        }

        public bool CanBlackCastleQueenSide
        {
            get;
            set;
        }

        public Position EnPassant
        {
            get;
            set;
        }

        public int HalfMoveClock
        {
            get;
            set;
        }

        public int FullMoveNumber
        {
            get;
            set;
        }

        public int ThreeCheck_ChecksByWhite
        {
            get;
            set;
        }

        public int ThreeCheck_ChecksByBlack
        {
            get;
            set;
        }
    }
}