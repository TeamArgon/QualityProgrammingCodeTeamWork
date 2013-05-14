namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Minesweeper.Common;
    using Minesweeper.InputMethods;
    using Minesweeper.Renderer;

    /// <summary>
    /// The game engine class, used to start a new game.
    /// </summary>
    public class GameEngine
    {
        private const int MaxRows = 5;
        private const int MaxColumns = 10;
        private const int MaxMines = 15;
        private const int MaxTopPlayers = 5;
        private readonly IRenderer gameRenderer;
        private readonly IInputMethod inputMethod;
        private Board board;
        private HighScores scores;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameEngine" /> class.
        /// </summary>
        /// <param name="renderer">The game renderer.</param>
        /// <param name="inputMethod">The user input method.</param>
        public GameEngine(IRenderer renderer, IInputMethod inputMethod)
        {
            this.gameRenderer = renderer;
            this.inputMethod = inputMethod;
            scores = new HighScores(MaxTopPlayers);
            this.GenerateNewBoard();
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void StartGame()
        {
            string command = "restart";
            int chosenRow = 0;
            int chosenColumn = 0;

            while (command != "exit")
            {
                switch (command)
                {
                    case "restart":
                        this.GenerateNewBoard();
                        this.gameRenderer.DisplayMessage(
                            "Welcome to the game “Minesweeper”. " +
                            "Try to reveal all cells without mines. " +
                            "Use 'top' to view the scoreboard, 'restart' to start a new game" +
                            "and 'exit' to quit the game.");

                        this.gameRenderer.DrawBoard(this.board);
                        break;
                    case "top":
                        string topScore = scores.DisplayTopScores();
                        this.gameRenderer.DisplayMessage(topScore);
                        break;
                    case "exit":
                        this.gameRenderer.DisplayMessage("Good bye!");
                        return;
                    case "coordinates":
                        try
                        {
                            BoardStatus boardStatus = this.board.OpenField(chosenRow, chosenColumn);
                            if (boardStatus == BoardStatus.SteppedOnAMine)
                            {
                                int score = this.board.CountOpenedFields();
                                this.EndGame(string.Format(
                                    "Booooom! You were killed by a mine. You revealed" +
                                    " {0} cells without mines.",
                                    score));
                                command = "restart";
                                continue;
                            }
                            else if (boardStatus == BoardStatus.FieldAlreadyOpened)
                            {
                                this.gameRenderer.DisplayMessage("That field has already been opened!");
                            }
                            else if (boardStatus == BoardStatus.AllFieldsAreOpened)
                            {
                                this.EndGame("Congratulations! You win!!");
                                command = "restart";
                                continue;
                            }
                            else
                            {
                                this.gameRenderer.DrawBoard(this.board);
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            this.gameRenderer.DisplayError("The row and column entered must be within the playing field!");
                        }

                        break;
                    default:
                        this.gameRenderer.DisplayError("Invalid input!");
                        break;
                }

                this.gameRenderer.DisplayMessage("Enter row and column: ");

                // TODO: extract this in a new method
                string playerInput = this.inputMethod.GetUserInput();
                if (int.TryParse(playerInput, out chosenRow))
                {
                    command = "coordinates";
                    playerInput = this.inputMethod.GetUserInput();
                    if (int.TryParse(playerInput, out chosenColumn))
                    {
                        command = "coordinates";
                    }
                    else
                    {
                        command = playerInput;
                    }
                }
                else
                {
                    command = playerInput;
                }
            }
        }

        /// <summary>
        /// Generates a new board.
        /// </summary>
        private void GenerateNewBoard()
        {
            this.board = new Board(MaxRows, MaxColumns, MaxMines);
        }

        /// <summary>
        /// Ends the game with a <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        private void EndGame(string message)
        {
            this.gameRenderer.DisplayMessage(this.board.ToStringAllFieldsRevealed());
            this.gameRenderer.DisplayMessage(message);

            this.gameRenderer.DisplayMessage("Please enter a name:");
            string playerName = this.inputMethod.GetUserInput();
            while (string.IsNullOrEmpty(playerName))
            {
                this.gameRenderer.DisplayMessage("Invalid name. Please enter a name that is not empty:");
                playerName = this.inputMethod.GetUserInput();
            }

            int score = this.board.CountOpenedFields();
            this.scores.ProcessScore(score, playerName);
            string topScores = this.scores.DisplayTopScores();
            gameRenderer.DisplayMessage(topScores);
        }
    }
}