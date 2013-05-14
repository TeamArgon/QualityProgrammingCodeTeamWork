namespace MinesweeperUnitTests
{
    using System;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Minesweeper.Common;

    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestBoardConstructor1_ThrowsExcepsion()
        {
            Board board = new Board(-1, 10, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestBoardConstructor2_ThrowsExcepsion()
        {
            Board board = new Board(10, 0, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestBoardConstructor3_ThrowsExcepsion()
        {
            Board board = new Board(10, 5, 51);
        }

        [TestMethod]
        public void TestBoardToString1()
        {
            Board board = new Board(5, 10, 10);
            StringBuilder expected = new StringBuilder();
            expected.Append("    0 1 2 3 4 5 6 7 8 9 \n");
            expected.Append("   _____________________\n");
            expected.Append("0 | ? ? ? ? ? ? ? ? ? ? |\n");
            expected.Append("1 | ? ? ? ? ? ? ? ? ? ? |\n");
            expected.Append("2 | ? ? ? ? ? ? ? ? ? ? |\n");
            expected.Append("3 | ? ? ? ? ? ? ? ? ? ? |\n");
            expected.Append("4 | ? ? ? ? ? ? ? ? ? ? |\n");
            expected.Append("   _____________________\n");

            Assert.AreEqual(expected.ToString(), board.ToString(), "Board string is wrong!");
        }

        [TestMethod]
        public void TestBoardToString2()
        {
            Board board = new Board(1, 1, 1);
            StringBuilder expected = new StringBuilder();
            expected.Append("    0 \n");
            expected.Append("   ___\n");
            expected.Append("0 | ? |\n");
            expected.Append("   ___\n");

            Assert.AreEqual(expected.ToString(), board.ToString(), "Board string is wrong!");
        }


    }
}
