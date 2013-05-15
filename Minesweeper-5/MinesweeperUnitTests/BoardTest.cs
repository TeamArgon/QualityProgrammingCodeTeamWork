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
        [ExpectedException(typeof(ArgumentOutOfRangeException), "The field cannot have a negative side!")]
        public void TestBoardConstructor1_ThrowsExcepsion()
        {
            Board board = new Board(-1, 10, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "The field cannot have a side of 0!")]
        public void TestBoardConstructor2_ThrowsExcepsion()
        {
            Board board = new Board(10, 0, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "The number of mines cannot be larger than the number of fields!")]
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

        [TestMethod]
        public void TestCountOpenedFieldsWhenNoOpened()
        {
            Board board = new Board(2, 3, 1);
            int actual=board.CountOpenedFields();
            Assert.AreEqual(0,actual, "Wrong count of opened fields!");
        }

        [TestMethod]
        public void TestOpenFieldWhenMine()
        {
            Board board = new Board(1, 1, 1);
            board.OpenField(0, 0);
            int actual = board.CountOpenedFields();
            Assert.AreEqual(0, actual, "Wrong count of opened fields!");
        }

        [TestMethod]
        public void TestToStringAllFieldsRevealed()
        {
            Board board = new Board(2, 2, 4);
            StringBuilder result = new StringBuilder();
            result.Append("    0 1 \n");
            result.Append("   _____\n");
            result.Append("0 | * * |\n");
            result.Append("1 | * * |\n");
            result.Append("   _____\n");
            string expected = result.ToString();
            string actual = board.ToStringAllFieldsRevealed();
            Assert.AreEqual(expected, actual, "Wrong count of opened fields!");
        }
    }
}  

