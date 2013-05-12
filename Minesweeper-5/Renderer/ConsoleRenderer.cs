namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ConsoleRenderer : IRenderer
    {
        public void Draw(string element)
        {
            Console.WriteLine(element);
        }
    }
}
