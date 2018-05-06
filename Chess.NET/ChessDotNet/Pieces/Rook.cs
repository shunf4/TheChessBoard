using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChessDotNet.Pieces
{
    public class Rook : Piece
    {
        public override Player Owner
        {
            get;
            set;
        }

        public override bool IsPromotionResult
        {
            get;
            set;
        }
        
        public Rook() : this(Player.None) {}

        public Rook(Player owner)
        {
            Owner = owner;
            IsPromotionResult = false;
        }

        public override Piece AsPromotion()
        {
            Rook copy = new Rook(Owner);
            copy.IsPromotionResult = true;
            return copy;
        }

        public override Piece GetWithInvertedOwner()
        {
            return new Rook(ChessUtilities.GetOpponentOf(Owner));
        }

        public override char GetFenCharacter()
        {
            return Owner == Player.White ? 'R' : 'r';
        }

        public override string GetFriendlyName()
        {
            return Owner == Player.White ? "白车" : "黑车";
        }

        public override bool IsValidMove(Move move, ChessGame game)
        {
            ChessUtilities.ThrowIfNull(move, "move");
            ChessUtilities.ThrowIfNull(game, "game");
            Position origin = move.OriginalPosition;
            Position destination = move.NewPosition;

            PositionDistance posDelta = new PositionDistance(origin, destination);
            if (posDelta.DistanceX != 0 && posDelta.DistanceY != 0)
                return false;
            bool increasingRank = destination.Rank > origin.Rank;
            bool increasingFile = (int)destination.File > (int)origin.File;
            if (posDelta.DistanceX == 0)
            {
                int f = (int)origin.File;
                for (int r = origin.Rank + (increasingRank ? 1 : -1);
                    increasingRank ? r < destination.Rank : r > destination.Rank;
                    r += increasingRank ? 1 : -1)
                {
                    if (game.GetPieceAt((File)f, r) != null)
                    {
                        return false;
                    }
                }
            }
            else // (posDelta.DeltaY == 0)
            {
                int r = origin.Rank;
                for (int f = (int)origin.File + (increasingFile ? 1 : -1);
                    increasingFile ? f < (int)destination.File : f > (int)destination.File;
                    f += increasingFile ? 1 : -1)
                {
                    if (game.GetPieceAt((File)f, r) != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override ReadOnlyCollection<MoreDetailedMove> GetValidMoves(Position from, bool returnIfAny, ChessGame game, Func<Move, bool> gameMoveValidator)
        {
            ChessUtilities.ThrowIfNull(from, "from");
            List<MoreDetailedMove> validMoves = new List<MoreDetailedMove>();
            Piece piece = game.GetPieceAt(from);
            int l0 = game.BoardHeight;
            int l1 = game.BoardWidth;
            for (int i = -7; i < 8; i++)
            {
                if (i == 0)
                    continue;
                if (from.Rank + i > 0 && from.Rank + i <= l0)
                {
                    MoreDetailedMove move = new MoreDetailedMove(from, new Position(from.File, from.Rank + i), piece.Owner, null, piece, false, CastlingType.None, null, false, false, false);
                    if (gameMoveValidator(move))
                    {
                        move.Castling = CastlingType.None;
                        game.WouldBeInCheckOrCheckmatedAfter(move, ChessUtilities.GetOpponentOf(move.Player), out bool inCheck, out bool checkmated);
                        move.IsChecking = inCheck;
                        move.IsCheckmate = checkmated;
                        move.AssociatedGame = game;

                        validMoves.Add(move);
                        if (returnIfAny)
                            return new ReadOnlyCollection<MoreDetailedMove>(validMoves);
                    }
                }
                if ((int)from.File + i > -1 && (int)from.File + i < l1)
                {
                    MoreDetailedMove move = new MoreDetailedMove(from, new Position(from.File + i, from.Rank), piece.Owner, null, piece, false, CastlingType.None, null, false, false, false);
                    if (gameMoveValidator(move))
                    {
                        move.Castling = CastlingType.None;
                        game.WouldBeInCheckOrCheckmatedAfter(move, ChessUtilities.GetOpponentOf(move.Player), out bool inCheck, out bool checkmated);
                        move.IsChecking = inCheck;
                        move.IsCheckmate = checkmated;
                        move.AssociatedGame = game;

                        validMoves.Add(move);
                        if (returnIfAny)
                            return new ReadOnlyCollection<MoreDetailedMove>(validMoves);
                    }
                }
            }
            return new ReadOnlyCollection<MoreDetailedMove>(validMoves);
        }
    }
}
