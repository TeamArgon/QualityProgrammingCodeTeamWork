namespace MinesweeperUnitTests
{
    using System;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Minesweeper.Renderer;

    [TestClass]
    public class ConsoleRendererTests
    {
        private static ConsoleRenderer renderer;
        private static StringWriter stringWriter;

        [TestInitialize]
        public void InitializeConsoleRenderer()
        {
            stringWriter = new StringWriter();
            renderer = new ConsoleRenderer();
        }

        [TestCleanup]
        public void CloseStringWriter()
        {
            stringWriter.Close();
        }

        [TestMethod]
        public void ConsoleRendererTestDisplayMessage1()
        {
            string message = "command";
            Console.SetOut(stringWriter);
            renderer.DisplayMessage(message);
            string expected = message;
            string actual = stringWriter.ToString().Trim();
            Assert.AreEqual(expected, actual, "The message was displayed incorrectly!");
        }

        [TestMethod]
        public void ConsoleRendererTestDisplayMessage2()
        {
            string message = "exit";
            Console.SetOut(stringWriter);
            renderer.DisplayMessage(message);
            string expected = message;
            string actual = stringWriter.ToString().Trim();
            Assert.AreEqual(expected, actual, "The message was displayed incorrectly!");
        }

        [TestMethod]
        public void ConsoleRendererTestDisplayError1()
        {
            string message = "Invalid input!";
            Console.SetOut(stringWriter);
            renderer.DisplayMessage(message);
            string expected = message;
            string actual = stringWriter.ToString().Trim();
            Assert.AreEqual(expected, actual, "The message was displayed incorrectly!");
        }

        [TestMethod]
        public void ConsoleRendererTestDisplayError2()
        {
            string message = "The row and column entered must be within the playing field!";
            Console.SetOut(stringWriter);
            renderer.DisplayMessage(message);
            string expected = message;
            string actual = stringWriter.ToString().Trim();
            Assert.AreEqual(expected, actual, "The message was displayed incorrectly!");
        }

        [TestMethod]
        public void ConsoleRendererTestDisplayErrorEmptyString()
        {
            string message = string.Empty;
            Console.SetOut(stringWriter);
            renderer.DisplayMessage(message);
            string expected = message;
            string actual = stringWriter.ToString().Trim();
            Assert.AreEqual(expected, actual, "The message was displayed incorrectly!");
        }
    }
}
