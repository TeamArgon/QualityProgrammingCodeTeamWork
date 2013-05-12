namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MinesweeperGame
    {
        public static void Main()
        {
            GameEngine ge = new GameEngine(new ConsoleRenderer(), new ConsoleGameController());
            ge.StartGame();
        }
    }
}
