namespace Minesweeper.Controller
{
    using System;

    /// <summary>
    /// A class that lets the user control the game using the console.
    /// </summary>
    public class ConsoleGameController : IGameController
    {
        /// <summary>
        /// Gets the user input.
        /// </summary>
        /// <returns>A string with the user command.</returns>
        public string GetUserInput()
        {
            string userCommand = Console.ReadLine();
            return userCommand;
        }
    }
}
