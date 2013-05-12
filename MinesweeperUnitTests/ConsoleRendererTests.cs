namespace MinesweeperUnitTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Minesweeper.Renderer;
    using System.IO;

    [TestClass]
    public class ConsoleRendererTests
    {
        private static ConsoleRenderer renderer;
        private static StringWriter stringWriter;

        [ClassInitialize]
        public static void InitializeConsoleRenderer(TestContext context)
        {
            renderer = new ConsoleRenderer();
            stringWriter = new StringWriter();
        }

        [TestMethod]
        public void ConsoleRendererTestDisplayMessage1()
        {
            using (stringWriter)
            {
                string message = "command";
                Console.SetOut(stringWriter);
                renderer.DisplayMessage(message);
                string expected = message;
                string actual = stringWriter.ToString().Trim();
                Assert.AreEqual(expected, actual, "The message was displayed incorrectly!");
            }
        }
    }
}
