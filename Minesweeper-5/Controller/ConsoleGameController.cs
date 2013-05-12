namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ConsoleGameController : IGameController
    {
        public string GetUserInput()
        {
            string userCommand = Console.ReadLine();
            return userCommand;
        }
    }
}
