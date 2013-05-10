namespace MinesweeperUnitTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Minesweeper;

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
    }
}
