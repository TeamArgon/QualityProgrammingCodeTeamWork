namespace Minesweeper.CommandExecutors
{
    using System;
    using System.Diagnostics;
    using Minesweeper.Common;
    using Minesweeper.InputMethods;
    using Minesweeper.Renderers;
    
    public class DefaultGameCommandExecutor : IGameCommandExecutor
    {
        private readonly IRenderer gameRenderer;
        private readonly IInputMethod inputMethod;
        private readonly HighScores scores;
        private Board board;
        private const int MaxRows = 5;
        private const int MaxColumns = 10;
        private const int MaxMines = 15;

        public DefaultGameCommandExecutor(IRenderer renderer, IInputMethod inputMethod, HighScores scores)
        {
            this.scores = scores;
            this.gameRenderer = renderer;
            this.inputMethod = inputMethod;
            GenerateNewBoard(MaxRows, MaxColumns, MaxMines);
        }

        public void Start()
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

                this.ProcessUserInput(ref chosenRow, ref chosenColumn, ref command);
            }
        }

        public void ProcessUserInput(ref int chosenRow, ref int chosenColumn, ref string command)
        {
            Debug.Assert(chosenRow != null, "The row cannot be null!");
            Debug.Assert(chosenColumn != null, "The column cannot be null!");
            Debug.Assert(chosenRow >= 0, "The row cannot be negative!");
            Debug.Assert(chosenColumn >= 0, "The column cannot be negative!");
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

        public void ExitGame()
        {
            Debug.Assert(this.gameRenderer != null, "The game renderer cannot be null!");
            this.gameRenderer.DisplayMessage("Good bye!");
        }

        public void InvalidInput()
        {
            Debug.Assert(this.gameRenderer != null, "The game renderer cannot be null!");
            this.gameRenderer.DisplayError("Invalid input!");
        }

        public void CheckCoordinates(int chosenRow, int chosenColumn)
        {
            Debug.Assert(chosenRow != null, "The row cannot be null!");
            Debug.Assert(chosenColumn != null, "The column cannot be null!");
            Debug.Assert(chosenRow >= 0, "The row cannot be negative!");
            Debug.Assert(chosenColumn >= 0, "The column cannot be negative!");
            try
            {
                BoardStatus boardStatus = this.board.OpenField(chosenRow, chosenColumn);
                if (boardStatus == BoardStatus.SteppedOnAMine)
                {
                    int score = this.board.CountOpenedFields();
                    this.EndGame(
                        string.Format(
                        "Booooom! You were killed by a mine. You revealed" + " {0} cells without mines.", score));
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

        public void DisplayTopScores()
        {
            Debug.Assert(this.gameRenderer != null, "The game renderer cannot be null!");
            Debug.Assert(this.scores != null, "The scores cannot be null!");
            this.gameRenderer.DisplayMessage("Scoreboard");
            string topScore = this.scores.GetTopScores();
            this.gameRenderer.DisplayMessage(topScore);
        }

        public void RestartGame()
        {
            Debug.Assert(this.gameRenderer != null, "The game renderer cannot be null!");

            this.GenerateNewBoard(MaxRows, MaxColumns, MaxMines);
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
        public void GenerateNewBoard(int maxRows, int maxColumns, int maxMines)
        {
            this.board = new Board(maxRows, maxColumns, maxMines);
        }

        /// <summary>
        /// Ends the game with a <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        public void EndGame(string message)
        {
            Debug.Assert(this.gameRenderer != null, "The game renderer cannot be null!");
            Debug.Assert(this.inputMethod != null, "The input method cannot be null!");
            Debug.Assert(this.scores != null, "The scores cannot be null!");
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
