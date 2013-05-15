namespace Minesweeper
{
    using System;
    using System.Linq;
    using Minesweeper.Common;
    using Minesweeper.InputMethods;
    using Minesweeper.Renderers;

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
        private readonly HighScores scores;

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
                        this.RestartGame();
                        break;
                    case "top":
                        this.DisplayTopScores();
                        break;
                    case "coordinates":
                        this.CheckCoordinates(chosenRow, chosenColumn);
                        break;
                    case "exit":
                        this.ExitGame();
                        return;
                    default:
                        this.InvalidInput();
                        break;
                }

                ProcessUserInput(ref chosenRow, ref chosenColumn, ref command);
            }
        }
  
        private void ProcessUserInput(ref int chosenRow, ref int chosenColumn, ref string command)
        {
            this.gameRenderer.DisplayMessage("Enter row and column: ");
            string playerInput = this.inputMethod.GetUserInput();
            if (int.TryParse(playerInput, out chosenRow))
            {
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

        private void ExitGame()
        {
            this.gameRenderer.DisplayMessage("Good bye!");
        }

        private void InvalidInput()
        {
            this.gameRenderer.DisplayError("Invalid input!");
        }

        private void CheckCoordinates(int chosenRow, int chosenColumn)
        {
            try
            {
                BoardStatus boardStatus = this.board.OpenField(chosenRow, chosenColumn);
                if (boardStatus == BoardStatus.SteppedOnAMine)
                {
                    int score = this.board.CountOpenedFields();
                    this.EndGame(string.Format("Booooom! You were killed by a mine. You revealed" +
                                               " {0} cells without mines.",score));
                    this.RestartGame();
                }
                else if (boardStatus == BoardStatus.FieldAlreadyOpened)
                {
                    this.gameRenderer.DisplayMessage("That field has already been opened!");
                }
                else if (boardStatus == BoardStatus.AllFieldsAreOpened)
                {
                    this.EndGame("Congratulations! You win!!");
                    this.RestartGame();
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
        }

        private void DisplayTopScores()
        {
            this.gameRenderer.DisplayMessage("Scoreboard");
            string topScore = this.scores.GetTopScores();
            this.gameRenderer.DisplayMessage(topScore);
        }

        private void RestartGame()
        {
            this.GenerateNewBoard();
            this.gameRenderer.DisplayMessage(
                "Welcome to the game “Minesweeper”. " +
                "Try to reveal all cells without mines. " +
                "Use 'top' to view the scoreboard, 'restart' to start a new game" +
                "and 'exit' to quit the game.");

            this.gameRenderer.DrawBoard(this.board);
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
            this.scores.ProcessScore(playerName, score);
            this.gameRenderer.DisplayMessage("Scoreboard");
            string topScores = this.scores.GetTopScores();
            this.gameRenderer.DisplayMessage(topScores);
        }
    }
}