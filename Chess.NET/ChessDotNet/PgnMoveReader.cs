using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessDotNet.Pieces;

namespace ChessDotNet
{
    public static class PgnMoveReader
    {
        public static Move ParseMove(string moveInStr, Player player, ChessGame game)
        {
            try
            {
                string move = moveInStr.TrimEnd('#', '?', '!', '+').Trim();

                Position origin = null;
                Position destination = null;
                Piece piece = null;
                char? promotion = null;

                if (move.Length == 0)
                {
                    throw new PgnException("The length of Move string is zero.");
                }

                if (move.Length > 2)
                {
                    string possiblePromotionPiece = move.Substring(move.Length - 2).ToUpperInvariant();
                    if (possiblePromotionPiece[0] == '=')
                    {
                        promotion = possiblePromotionPiece[1];
                        move = move.Remove(move.Length - 2, 2);
                    }
                }

                if (move.ToUpperInvariant() == "O-O")
                {
                    int r = player == Player.White ? 1 : 8;
                    origin = new Position(File.E, r);
                    destination = new Position(File.G, r);
                    piece = new King(player);
                }
                else if (move.ToUpperInvariant() == "O-O-O")
                {
                    int r = player == Player.White ? 1 : 8;
                    origin = new Position(File.E, r);
                    destination = new Position(File.C, r);
                    piece = new King(player);
                }

                if (piece == null)
                {
                    piece = game.MapPgnCharToPiece(move[0], player);
                }
                if (!(piece is Pawn))
                {
                    move = move.Remove(0, 1);
                }

                int rankRestriction = -1;
                File fileRestriction = File.None;
                if (destination == null)
                {
                    if (move.Length == 0)
                    {
                        throw new PgnException("The position of Move is empty.");
                    }

                    if (move[0] == 'x')
                    {
                        move = move.Remove(0, 1);
                    }
                    else if (move.Length == 4 && move[1] == 'x')
                    {
                        move = move.Remove(1, 1);
                    }

                    if (move.Length == 2)
                    {
                        destination = new Position(move);
                    }
                    else if (move.Length == 3)
                    {
                        if (char.IsDigit(move[0]))
                        {
                            rankRestriction = int.Parse(move[0].ToString());
                        }
                        else
                        {
                            bool recognized = Enum.TryParse<File>(move[0].ToString(), true, out fileRestriction);
                            if (!recognized)
                            {
                                throw new PgnException("Invalid PGN: unrecognized origin file.");
                            }
                        }
                        destination = new Position(move.Remove(0, 1));
                    }
                    else if (move.Length == 4)
                    {
                        origin = new Position(move.Substring(0, 2));
                        destination = new Position(move.Substring(2, 2));
                    }
                    else
                    {
                        throw new PgnException("Invalid PGN.");
                    }
                }

                Move m;
                if (origin != null)
                {
                    m = new Move(origin, destination, player, promotion);
                    if (game.IsValidMove(m))
                    {
                        return m;
                    }
                    else
                    {
                        throw new PgnException("Invalid PGN: contains invalid moves.");
                    }
                }
                else
                {
                    Piece[][] board = game.GetBoard();
                    List<Move> validMoves = new List<Move>();
                    for (int r = 0; r < game.BoardHeight; r++)
                    {
                        if (rankRestriction != -1 && r != 8 - rankRestriction) continue;
                        for (int f = 0; f < game.BoardWidth; f++)
                        {
                            if (fileRestriction != File.None && f != (int)fileRestriction) continue;
                            if (board[r][f] != piece) continue;
                            m = new Move(new Position((File)f, 8 - r), destination, player, promotion);
                            if (game.IsValidMove(m, true))
                            {
                                validMoves.Add(m);
                            }
                        }
                    }
                    if (validMoves.Count == 0) throw new PgnException("Invalid PGN: contains invalid moves.");
                    if (validMoves.Count > 1) throw new PgnException("Invalid PGN: contains ambiguous moves.");
                    return validMoves[0];
                }
            }
            catch(Exception e)
            {
                if (e is PgnException eP)
                {
                    throw eP;
                }
                else
                    throw new PgnException(e.Message + Environment.NewLine + e.StackTrace);
            }
        }
    }
}
