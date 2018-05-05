﻿using ChessDotNet; // the namespace of Chess.NET
using System;
using System.Collections.Generic;
using System.Linq;

namespace Samples
{
    class BasicUsageSample
    {
        static void Main(string[] args)
        {
            // Let's start by creating a chess game instance.
            ChessGame game = new ChessGame();

            // Now the game's board is in the start position and it's white's turn.
            Console.WriteLine("It's this color's turn: {0}", game.WhoseTurn);

            // This is how to find out which piece is at a certain position:
            Piece pieceAtA1 = game.GetPieceAt(new Position("A1")); // Or "a1", the casing doesn't matter
            /* There are other overloading options as well:
             * game.GetPieceAt(new Position(File.A, 1));
             * game.GetPieceAt(File.A, 1);
             * All those three options return the same. */
            Console.WriteLine("What piece is there at A1? {0}", pieceAtA1.GetFenCharacter());
            // GetFenCharacter() returns the FEN character for the given piece. White piece: uppercase, black piece: lowercase. The character is the first char of a piece's name (exception: Knight -> N/n because King -> K/k).
            // The Piece class is the abstract base class for pieces. All piece classes (e.g. Rook) derive from this class.

            // White has to make a turn. They want to move their E2 pawn to E4. Is that valid?
            Move e2e4 = new Move("E2", "E4", Player.White);
            bool isValid = game.IsValidMove(e2e4);
            Console.WriteLine("E2-E4 for white is valid: {0}", isValid);

            // Great, it's valid! So white wants to actually make that move.
            MoveType type = game.ApplyMove(e2e4, true);
            // The first argument is the move, the second argument indicates whether it's already validated. Here it is, so pass 'true'. If it's not validated yet, ApplyMove will do it. **Only pass `true` if it's really validated! If you pass `true`, ApplyMove won't do ANY validity checks.**
            // The return type is the MoveType enum. It holds one, or a combination, of these values: Invalid, Move, Capture, Castling, Promotion
            // Each valid move will always carry the 'Move' value. If it's also something else, it will carry both values (e.g. if the move is a capture, `type` will have the value MoveType.Move | MoveType.Capture).
            // MoveType is a flags enumeration. https://msdn.microsoft.com/en-us/library/ms229062%28v=vs.100%29.aspx
            // e4 is just a normal move, so `type` will just be MoveType.Move.
            Console.WriteLine("Move type: {0}", type);

            // When a move has been made, check the Status property. It will let you know if there is a special event: check, checkmate, stalemate ...
            GameStatus status = game.Status;
            Console.WriteLine("Special event? {0}", status.Event);
            // Here it just returns 'None' because nothing special happened.
            // GameStatus has two other properties: PlayerWhoCausedEvent (quite self-explanatory) and EventExplanation (used if Chess.NET needs to be more specific about an event, e.g. when it's a draw, explaining why).

            // Now it's black's turn.
            Console.WriteLine("It's this color's turn: {0}", game.WhoseTurn);

            // You can figure out all valid moves using GetValidMoves.
            IEnumerable<Move> validMoves = game.GetValidMoves(Player.Black);
            // Here it returns all valid moves for black, but you can also find all valid moves *from a certain position* by passing a Position instance as argument.
            Console.WriteLine("How many valid moves does black have? {0}", validMoves.Count());

            // It might happen that you don't really care about all valid moves, but just want to know if there are valid moves. Chess.NET also has a method for that:
            bool hasValidMoves = game.HasAnyValidMoves(Player.Black);
            // Again, you can also pass a Position instance here.
            Console.WriteLine("Black has any valid moves: {0}", hasValidMoves);

            // Congratulations! You have learned about the most important methods of Chess.NET. Enjoy using the library :)
            Console.ReadKey();
        }
    }
}
