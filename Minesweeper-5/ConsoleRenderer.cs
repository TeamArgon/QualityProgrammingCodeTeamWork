namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ConsoleRenderer : IRenderer
    {
        public void Draw(String element)
        {
            Console.WriteLine(element);
        }
    }
}
