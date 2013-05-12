namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public class GameEngine
    {
        private const int MaxRows = 5;
        private const int MaxColumns = 10;
        private const int MaxMines = 15;
        private const int MaxTopPlayers = 5;
        private IRenderer gameRenderer;
        private IGameController gameController;
        private Board board;
        private List<Player> topPlayers;

        public GameEngine(IRenderer renderer, IGameController controller)
        {
            this.gameController = controller;
            this.gameRenderer = renderer;
            this.topPlayers = new List<Player>();
            this.topPlayers.Capacity = MaxTopPlayers;
            this.GenerateNewBoard();
        }

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
                        this.gameRenderer.Draw("Welcome to the game �Minesweeper�. " +
                            "Try to reveal all cells without mines. " +
                            "Use 'top' to view the scoreboard, 'restart' to start a new game" +
                            "and 'exit' to quit the game.");
                        this.gameRenderer.Draw(this.board.ToString());
                        break;
                    case "top":
                        this.DisplayTopScores();
                        break;
                    case "exit":
                        this.gameRenderer.Draw("Good bye!");
                        return;
                    case "coordinates":
                        try
                        {
                            Board.Status status = this.board.OpenField(chosenRow, chosenColumn);
                            if (status == Board.Status.SteppedOnAMine)
                            {
                                this.gameRenderer.Draw(this.board.ToStringAllFieldsRevealed());
                                int score = this.board.CountOpenedFields();
                                this.gameRenderer.Draw("Booooom! You were killed by a mine. You revealed " +
                                    score +
                                    " cells without mines.");

                                this.AddToTopScores(score);
                                this.DisplayTopScores();
                                command = "restart";
                                continue;
                            }
                            else if (status == Board.Status.FieldAlreadyOpened)
                            {
                                this.gameRenderer.Draw("That field has already been opened!");
                            }
                            else if (status == Board.Status.AllFieldsAreOpened)
                            {
                                this.gameRenderer.Draw(this.board.ToStringAllFieldsRevealed());
                                int score = this.board.CountOpenedFields();
                                this.gameRenderer.Draw("Congratulations! You win!!");
                                this.AddToTopScores(score);
                                this.DisplayTopScores();
                                command = "restart";
                                continue;
                            }
                            else
                            {
                                this.gameRenderer.Draw(this.board.ToString());
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            this.gameRenderer.Draw("The row and column entered must be within the playing field!");
                        }

                        break;
                    default:
                        this.gameRenderer.Draw("Invalid input!");
                        break;
                }

                this.gameRenderer.Draw("Enter row and column: ");

                // TODO: extract this in a new method
                string playerInput = this.gameController.GetUserInput();
                if (int.TryParse(playerInput, out chosenRow))
                {
                    command = "coordinates";
                    playerInput = this.gameController.GetUserInput();
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

        private void GenerateNewBoard()
        {
            this.board = new Board(MaxRows, MaxColumns, MaxMines);
        }

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

        private void AddTopScore(Player player)
        {
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

        private void DisplayTopScores()
        {
            this.gameRenderer.Draw("Scoreboard");
            for (int i = 0; i < this.topPlayers.Count; i++)
            {
                this.gameRenderer.Draw(string.Format("{0}. {1}", i + 1, this.topPlayers[i]));
            }
        }

        private void AddToTopScores(int score)
        {
            Debug.Assert(score >= 0, "The score cannot be negative");
            Debug.Assert(score <= (MaxRows * MaxColumns) - MaxMines, "The score cannot be larger than the amount of empty fields");

            if (this.IsHighScore(score))
            {
                this.gameRenderer.Draw("Please enter your name for the top scoreboard: ");
                string name = this.gameController.GetUserInput();
                Player player = new Player(name, score);
                this.AddTopScore(player);
            }
        }
    }
}