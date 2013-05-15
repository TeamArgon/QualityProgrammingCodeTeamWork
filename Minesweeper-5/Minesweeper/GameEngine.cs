namespace Minesweeper
{
    using System;
    using System.Linq;
    using System.Diagnostics;
    using Minesweeper.Common;
    using Minesweeper.InputMethods;
    using Minesweeper.Renderers;
    using Minesweeper.CommandExecutors;

    /// <summary>
    /// The game engine class, used to start a new game.
    /// </summary>
    public class GameEngine
    {

        private const int MaxTopPlayers = 5;
        private readonly IRenderer gameRenderer;
        private readonly IInputMethod inputMethod;
        private readonly HighScores scores;
        private readonly IGameCommandExecutor cmdExecutor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameEngine" /> class.
        /// </summary>
        /// <param name="renderer">The game renderer.</param>
        /// <param name="inputMethod">The user input method.</param>
        public GameEngine(IRenderer renderer, IInputMethod inputMethod)
        {
            this.gameRenderer = renderer;
            this.inputMethod = inputMethod;
            this.scores = new HighScores(MaxTopPlayers);
            this.cmdExecutor = new DefaultGameCommandExecutor(this.gameRenderer, this.inputMethod, this.scores);
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void StartGame()
        {
            cmdExecutor.Start();
        }
    }
}