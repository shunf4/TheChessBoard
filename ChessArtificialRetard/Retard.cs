using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessDotNet;
using ChessDotNet.Pieces;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ChessArtificialRetard
{
    class Retard
    {
       
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

        static readonly Piece kw = FenMappings['K'];
        static readonly Piece kb = FenMappings['k'];
        static readonly Piece qw = FenMappings['Q'];
        static readonly Piece qb = FenMappings['q'];
        static readonly Piece rw = FenMappings['R'];
        static readonly Piece rb = FenMappings['r'];
        static readonly Piece nw = FenMappings['N'];
        static readonly Piece nb = FenMappings['n'];
        static readonly Piece bw = FenMappings['B'];
        static readonly Piece bb = FenMappings['b'];
        static readonly Piece pw = FenMappings['P'];
        static readonly Piece pb = FenMappings['p'];
        static readonly Piece o = null;

        static ChessGame Game;
        static Player MyPlayer;

        static bool MyTurn()
        {
            ReadOnlyCollection<MoreDetailedMove> validMoves = Game.GetValidMoves(MyPlayer);
            if (validMoves.Count == 0)
            {
                // Lose or Draw
                //Environment.Exit(1);
                return false;
            }
            else
            {
                //System.Threading.Thread.Sleep(1000);
                var random = new Random();
                var k = random.Next(0, validMoves.Count);
                var move = validMoves[k];
                Console.Out.WriteLine(move.GenerateSANString(Game));
                Trace.WriteLine(move.StoredSANString);

                MoveType mt = Game.ApplyMove(move, true);
                if (mt == MoveType.Invalid)
                {
                    Trace.TraceError("My move san has errors.");
                    return false;
                }
            }
            return true;
        }

        static bool HisTurn()
        {
            string sanString = Console.In.ReadLine();
            if (sanString == null)
                return false;
            Trace.WriteLine(sanString);
            var move = PgnMoveReader.ParseMove(sanString, ChessUtilities.GetOpponentOf(MyPlayer), Game);
            MoveType mt = Game.ApplyMove(move, true);
            if(mt == MoveType.Invalid)
            {
                //throw new ArgumentException("His move san has errors.");
                return false;
            }
            return true;
        }

        static void Main(string[] args)
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Error));
            //Trace.Listeners.Clear();
            Trace.WriteLine("Retard");
            GameCreationData gcd = new GameCreationData
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

            Game = new ChessGame(gcd);

            // Read My Color
            string myColorStr;
            
            myColorStr = System.Console.In.ReadLine();
            switch(myColorStr)
            {
                case "white":
                    MyPlayer = Player.White;
                    break;
                case "black":
                    MyPlayer = Player.Black;
                    break;
                default:
                    throw new ArgumentException("Player color string invalid.");
            }

            bool valid = true;
            while (valid)
            {
                if(MyPlayer == Game.WhoseTurn)
                {
                    valid = valid && MyTurn();
                }
                else
                {
                    valid = valid && HisTurn();
                }
            }
        }
    }
}
