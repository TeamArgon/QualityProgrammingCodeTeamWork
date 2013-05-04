namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Minesweeper.GameElements;
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
        private readonly List<Player> topPlayers;
        private Board board;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameEngine" /> class.
        /// </summary>
        /// <param name="renderer">The game renderer.</param>
        /// <param name="inputMethod">The user input method.</param>
        public GameEngine(IRenderer renderer, IInputMethod inputMethod)
        {
            this.gameRenderer = renderer;
            this.inputMethod = inputMethod;
            this.topPlayers = new List<Player>();
            this.topPlayers.Capacity = MaxTopPlayers;
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
                        this.DisplayTopScores();
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
        /// Determines whether a score is among the top scores.
        /// </summary>
        /// <param name="score">The score.</param>
        /// <returns>True if the score is one of the top scores.</returns>
        private bool IsHighScore(int score)
        {
            if (this.topPlayers.Capacity > this.topPlayers.Count)
            {
                return true;
            }

            if (score > this.topPlayers.Min().Score)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a top score.
        /// </summary>
        /// <param name="player">The player that achieved the score.</param>
        private void AddTopScore(Player player)
        {
            Debug.Assert(player != null, "The player cannot be null!");
            if (this.topPlayers.Capacity > this.topPlayers.Count)
            {
                this.topPlayers.Add(player);
                this.topPlayers.Sort();
            }
            else
            {
                this.topPlayers.RemoveAt(this.topPlayers.Capacity - 1);
                this.topPlayers.Add(player);
                this.topPlayers.Sort();
            }
        }

        /// <summary>
        /// Displays the top scores.
        /// </summary>
        private void DisplayTopScores()
        {
            this.gameRenderer.DisplayMessage("Scoreboard");
            for (int i = 0; i < this.topPlayers.Count; i++)
            {
                this.gameRenderer.DisplayMessage(string.Format("{0}. {1}", i + 1, this.topPlayers[i]));
            }
        }

        /// <summary>
        /// Processes the score by adding it to the top scores if necessary.
        /// </summary>
        /// <param name="score">The player score.</param>
        private void ProcessScore(int score)
        {
            Debug.Assert(score >= 0, "The score cannot be negative");
            Debug.Assert(score <= (MaxRows * MaxColumns) - MaxMines, "The score cannot be larger than the amount of empty fields");

            if (this.IsHighScore(score))
            {
                this.gameRenderer.DisplayMessage("Please enter your name for the top scoreboard: ");
                string name = this.inputMethod.GetUserInput();
                while (string.IsNullOrEmpty(name))
                {
                    this.gameRenderer.DisplayError("The name cannot be empty");
                    name = this.inputMethod.GetUserInput();
                }

                Player player = new Player(name, score);
                this.AddTopScore(player);
            }
        }

        /// <summary>
        /// Ends the game with a <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        private void EndGame(string message)
        {
            this.gameRenderer.DisplayMessage(this.board.ToStringAllFieldsRevealed());
            int score = this.board.CountOpenedFields();
            this.gameRenderer.DisplayMessage(message);
            this.ProcessScore(score);
            this.DisplayTopScores();
        }
    }
}