﻿using NUnit.Framework;

namespace ChessDotNet.Variants.Tests
{
    using ThreeCheck;

    [TestFixture]
    public class ThreeCheckChessGameTests
    {
        [Test]
        public static void TestRecordedChecks()
        {
            ThreeCheckChessGame game = new ThreeCheckChessGame();
            game.ApplyMove(new Move("E2", "E4", Player.White), true);
            game.ApplyMove(new Move("E7", "E5", Player.Black), true);
            game.ApplyMove(new Move("D1", "H5", Player.White), true);
            game.ApplyMove(new Move("G7", "G6", Player.Black), true);
            game.ApplyMove(new Move("H5", "E5", Player.White), true);
            Assert.AreEqual(1, game.ChecksByWhite);
            Assert.AreEqual(0, game.ChecksByBlack);
            game.ApplyMove(new Move("D8", "E7", Player.Black), true);
            game.ApplyMove(new Move("E5", "H8", Player.White), true);
            game.ApplyMove(new Move("E7", "E4", Player.Black), true);
            Assert.AreEqual(1, game.ChecksByWhite);
            Assert.AreEqual(1, game.ChecksByBlack);
            game.ApplyMove(new Move("F1", "E2", Player.White), true);
            game.ApplyMove(new Move("F8", "G7", Player.Black), true);
            game.ApplyMove(new Move("H8", "G8", Player.White), true);
            Assert.AreEqual(2, game.ChecksByWhite);
            game.ApplyMove(new Move("E8", "E7", Player.Black), true);
            game.ApplyMove(new Move("G8", "E8", Player.White), true);
            Assert.AreEqual(3, game.ChecksByWhite);
            Assert.True(game.IsWinner(Player.White));
        }

        [Test]
        public static void TestPgnReaderForChecks()
        {
            PgnReader<ThreeCheckChessGame> reader = new PgnReader<ThreeCheckChessGame>();
            reader.ReadPgnFromString("1.e4 e5 2.Qh5 g6 3.Qxe5+ Qe7 4.Qxh8 Qxe4+ 5.Be2 Bg7 6.Qxg8+ Ke7 7.Qe8+");
            Assert.True(reader.Game.IsWinner(Player.White));
        }

        [Test]
        public static void TestFenCheckCounter()
        {
            ThreeCheckChessGame game = new ThreeCheckChessGame("rnb1kbnQ/pppp1p1p/6p1/8/8/8/PPPPqPPP/RNB1K1NR w KQq - 0 6 +1+2");
            Assert.AreEqual(1, game.ChecksByWhite);
            Assert.AreEqual(2, game.ChecksByBlack);
        }

        [Test]
        public static void TestFenGeneration()
        {
            ThreeCheckChessGame game = new ThreeCheckChessGame("rnb1kb1Q/pppp1p1p/5np1/8/8/8/PPPPKPPP/RNB3NR w q - 1 7 +1+2");
            game.ApplyMove(new Move("H8", "F8", Player.White), true);
            Assert.AreEqual("rnb1kQ2/pppp1p1p/5np1/8/8/8/PPPPKPPP/RNB3NR b q - 0 7 +2+2", game.GetFen());
        }
    }
}
