using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessDotNet;
using ChessDotNet.Pieces;

namespace EightQueen
{
    class Program
    {
        static List<Piece[][]> PossibleBoard;
        static int [][] arr = new int[][] {};
        static void Search(ChessGame game, int depth = 0)
        {
            //Console.Out.WriteLine("depth : {0}", depth);
            bool[,] validSquares = new bool[8,8];
            var validMoves = game.GetValidMoves(Player.White, false, false);
            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    validSquares[i,j] = true;
                }
            }
            foreach(var m in validMoves)
            {
                //Console.Out.WriteLine(m);
                validSquares[8 - m.NewPosition.Rank,(int)(m.NewPosition.File)] = false;
            }
            for (int i = 0; i < 8; i++)
            {
                if(validSquares[i,depth] && game.GetPieceAt((File)(depth), 8 - i) == null)
                {
                    //Console.Out.WriteLine("valid : {0}", depth);
                    var newGame = new ChessGame(game);
                    newGame.SetPieceAt((File)(depth), 8 - i, new Queen(Player.White));
                    if(depth == 7)
                    {
                        var b = newGame.GetBoard();
                        PossibleBoard.Add(b);
                    }
                    else
                        Search(newGame, depth + 1);
                }
            }
            }


        static void Main(string[] args)
        {
            PossibleBoard = new List<Piece[][]>();
            ChessGame game = new ChessGame(
                new Piece[][]
                {
                    new Piece [] { null, null,null, null, null,null,null,null},
                    new Piece [] { null, null,null, null, null,null,null,null},
                    new Piece [] { null, null,null, null, null,null,null,null},
                    new Piece [] { null, null,null, null, null,null,null,null},
                    new Piece [] { null, null,null, null, null,null,null,null},
                    new Piece [] { null, null,null, null, null,null,null,null},
                    new Piece [] { null, null,null, null, null,null,null,null},
                    new Piece [] { null, null,null, null, null,null,null,null},
                }, Player.White
                );
            game.careWhoseTurnItIs = false;
            PossibleBoard = new List<Piece[][]>();
            Search(game, 0);
            Console.Out.WriteLine(PossibleBoard.Count);
        }
    }
}
