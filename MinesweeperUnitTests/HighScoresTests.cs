using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Common;

namespace MinesweeperUnitTests
{
    [TestClass]
    public class HighScoresTests
    {        
        [TestMethod]
        public void TestIsHighScore()
        {
            HighScores higScoresList = new HighScores(2);
            bool actual = higScoresList.IsHighScore(12);
            bool expected = true;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestIsHighScoreWithFullScoreList1()
        {
            HighScores higScoresList = new HighScores(2);
            higScoresList.AddTopScore(new Player("a", 10));
            higScoresList.AddTopScore(new Player("b", 2));           
            bool actual = higScoresList.IsHighScore(12);
            bool expected = true;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestIsHighScoreWithFullScoreList2()
        {
            HighScores higScoresList = new HighScores(2);
            higScoresList.AddTopScore(new Player("a", 10));
            higScoresList.AddTopScore(new Player("b", 2));           
            bool actual = higScoresList.IsHighScore(1);
            bool expected = false;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestDisplayTopScores()
        {
            HighScores higScoresList = new HighScores(3);
            higScoresList.AddTopScore(new Player("e", 17));
            higScoresList.AddTopScore(new Player("e", 0));

            StringBuilder result = new StringBuilder();
            result.Append("Scoreboard\r\n");
            result.Append("1. e --> 17\r\n");
            result.Append("2. e --> 0\r\n");            
            string expected = result.ToString();
            string actual = higScoresList.DisplayTopScores();

            Assert.AreEqual(expected, actual);
        }
               
    }
}
