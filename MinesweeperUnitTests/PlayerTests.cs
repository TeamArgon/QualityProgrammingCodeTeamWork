using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Common;

namespace MinesweeperUnitTests
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void PlayerTestCompareTo1()
        {
            Player first = new Player("a", 12);
            Player second = new Player("b", 1);
            Assert.AreEqual(-1, first.CompareTo(second));
        }

        [TestMethod]
        public void PlayerTestCompareTo2()
        {
            Player first = new Player("a", 12);
            Player second = new Player("b", 120);
            Assert.AreEqual(1, first.CompareTo(second));
        }

        [TestMethod]
        public void PlayerTestToString()
        {
            Player player = new Player("mimi", 123);
            string expected = "mimi --> 123";
            string actual = player.ToString();
            Assert.AreEqual(expected,actual);
        }
    }
}
