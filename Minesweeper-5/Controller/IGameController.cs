namespace Minesweeper.Controller
{
    /// <summary>
    /// An interface that provides a method to obtain the user input.
    /// </summary>
    public interface IGameController
    {
        /// <summary>
        /// Gets the user input.
        /// </summary>
        /// <returns>A string with the user input.</returns>
        string GetUserInput();
    }
}
