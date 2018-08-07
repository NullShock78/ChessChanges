using System;
using System.Collections.Generic;
using ChessLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private string IntToChessStr(int num)
        {
            return ((char)('a' + num)).ToString();
        }

        [TestMethod]
        public void TestBlackKingBetweenRooks960()
        {
            string mode = "Chess960";
            Game game = new Game();
            game.Reset(mode);

            List<string> found = new List<string>();
            for (int j = 0; j < 8; j++)
            {
                var piece = game.Board[IntToChessStr(j) + "1"].piece;

                if (piece.IsRook())
                {
                    found.Add("Rook");
                }
                else if (piece.IsKing())
                {
                    found.Add("King");
                }
            }

            Assert.IsTrue(found[0] == "Rook" && found[1] == "King" && found[2] == "Rook");
        }

        [TestMethod]
        public void TestWhiteKingBetweenRooks960()
        {
            string mode = "Chess960";
            Game game = new Game();
            game.Reset(mode);

            List<string> found = new List<string>();
            for (int j = 0; j < 8; j++)
            {
                var piece = game.Board[IntToChessStr(j) + "8"].piece;

                if (piece.IsRook())
                {
                    found.Add("Rook");
                }
                else if (piece.IsKing())
                {
                    found.Add("King");
                }
            }

            Assert.IsTrue(found[0] == "Rook" && found[1] == "King" && found[2] == "Rook");
        }

        [TestMethod]
        public void TestBlackBishopOppositeColor960()
        {
            string mode = "Chess960";
            Game game = new Game();
            game.Reset(mode);

            bool firstFound = false;
            bool firstBishopOdd = false;
            bool differentColors = false;

            for (int j = 0; j < 8; j++)
            {
                var piece = game.Board[IntToChessStr(j) + "1"].piece;

                if (!firstFound)
                {
                    if (piece.IsBishop())
                    {
                        firstFound = true;
                        firstBishopOdd = ((j + 1) % 2 == 1);
                    }
                }
                else
                {
                    if (piece.IsBishop())
                    {
                        differentColors = (firstBishopOdd != ((j + 1) % 2 == 1));
                    }
                }
                
            }

            Assert.IsTrue(differentColors);
        }

        [TestMethod]
        public void TestWhiteBishopOppositeColor960()
        {
            string mode = "Chess960";
            Game game = new Game();
            game.Reset(mode);

            bool firstFound = false;
            bool firstBishopOdd = false;
            bool differentColors = false;

            for (int j = 0; j < 8; j++)
            {
                var piece = game.Board[IntToChessStr(j) + "8"].piece;

                if (!firstFound)
                {
                    if (piece.IsBishop())
                    {
                        firstFound = true;
                        firstBishopOdd = ((j + 1) % 2 == 1);
                    }
                }
                else
                {
                    if (piece.IsBishop())
                    {
                        differentColors = (firstBishopOdd != ((j + 1) % 2 == 1));
                    }
                }

            }

            Assert.IsTrue(differentColors);
        }

        [TestMethod]
        public void TestBothSidesMirrored960()
        {
            string mode = "Chess960";
            Game game = new Game();
            game.Reset(mode);

            for (int j = 0; j < 8; j++)
            {
                var pieceB = game.Board[IntToChessStr(j) + "1"].piece;
                var pieceW = game.Board[IntToChessStr(j) + "8"].piece;

                Assert.AreEqual(pieceB.Type, pieceW.Type);

            }

        }


        [TestMethod]
        public void TestNormalSetup()
        {
            string mode = "Normal";
            Game game = new Game();
            game.Reset(mode);

            Assert.IsTrue(game.Board["a1"].piece.Type == Piece.PieceType.Rook);
            Assert.IsTrue(game.Board["h1"].piece.Type == Piece.PieceType.Rook);
            Assert.IsTrue(game.Board["b1"].piece.Type == Piece.PieceType.Knight);
            Assert.IsTrue(game.Board["g1"].piece.Type == Piece.PieceType.Knight);
            Assert.IsTrue(game.Board["c1"].piece.Type == Piece.PieceType.Bishop);
            Assert.IsTrue(game.Board["f1"].piece.Type == Piece.PieceType.Bishop);
            Assert.IsTrue(game.Board["e1"].piece.Type == Piece.PieceType.King);
            Assert.IsTrue(game.Board["d1"].piece.Type == Piece.PieceType.Queen);
            Assert.IsTrue(game.Board["a1"].piece.Side.isBlack());
            Assert.IsTrue(game.Board["h1"].piece.Side.isBlack());
            Assert.IsTrue(game.Board["b1"].piece.Side.isBlack());
            Assert.IsTrue(game.Board["g1"].piece.Side.isBlack());
            Assert.IsTrue(game.Board["c1"].piece.Side.isBlack());
            Assert.IsTrue(game.Board["f1"].piece.Side.isBlack());
            Assert.IsTrue(game.Board["e1"].piece.Side.isBlack());
            Assert.IsTrue(game.Board["d1"].piece.Side.isBlack());

            for (int col = 1; col <= 8; col++)
            {
                Assert.IsTrue(game.Board[2, col].piece.Type == Piece.PieceType.Pawn);
                Assert.IsTrue(game.Board[2, col].piece.Side.isBlack());
            }

            Assert.IsTrue(game.Board["a8"].piece.Type == Piece.PieceType.Rook);
            Assert.IsTrue(game.Board["h8"].piece.Type == Piece.PieceType.Rook);
            Assert.IsTrue(game.Board["b8"].piece.Type == Piece.PieceType.Knight);
            Assert.IsTrue(game.Board["g8"].piece.Type == Piece.PieceType.Knight);
            Assert.IsTrue(game.Board["c8"].piece.Type == Piece.PieceType.Bishop);
            Assert.IsTrue(game.Board["f8"].piece.Type == Piece.PieceType.Bishop);
            Assert.IsTrue(game.Board["e8"].piece.Type == Piece.PieceType.King);
            Assert.IsTrue(game.Board["d8"].piece.Type == Piece.PieceType.Queen);
            Assert.IsTrue(game.Board["a8"].piece.Side.isWhite());
            Assert.IsTrue(game.Board["h8"].piece.Side.isWhite());
            Assert.IsTrue(game.Board["b8"].piece.Side.isWhite());
            Assert.IsTrue(game.Board["g8"].piece.Side.isWhite());
            Assert.IsTrue(game.Board["c8"].piece.Side.isWhite());
            Assert.IsTrue(game.Board["f8"].piece.Side.isWhite());
            Assert.IsTrue(game.Board["e8"].piece.Side.isWhite());
            Assert.IsTrue(game.Board["d8"].piece.Side.isWhite());

            for (int col = 1; col <= 8; col++)
            {
                Assert.IsTrue(game.Board[7, col].piece.Type == Piece.PieceType.Pawn);
                Assert.IsTrue(game.Board[7, col].piece.Side.isWhite());
            }

        }



    }
}
