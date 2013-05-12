namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GameEngine
    {
        private const int MaxRows = 5;
        private const int MaxColumns = 10;
        private const int MaxMines = 15;
        private const int MaxTopPlayers = 5;
        private IRenderer gameRenderer;
        private Board board;
        private List<Player> topPlayers;

        public GameEngine(IRenderer renderer)
        {
            this.gameRenderer = renderer;
            this.topPlayers = new List<Player>();
            this.topPlayers.Capacity = MaxTopPlayers;
            this.GenerateNewBoard();
        }

        public static void Main(string[] args)
        {
            GameEngine ge = new GameEngine(new ConsoleRenderer());
            ge.StartGame();
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
                        this.gameRenderer.Draw("Welcome to the game “Minesweeper”. " +
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
                        Console.Read();
                        break;
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

                                if (this.IsHighScore(score))
                                {
                                    this.gameRenderer.Draw("Please enter your name for the top scoreboard: ");
                                    string name = Console.ReadLine();
                                    Player player = new Player(name, score);
                                    this.AddTopScore(player);
                                    this.DisplayTopScores();
                                }

                                command = "restart";
                                continue;
                            }
                            else if (status == Board.Status.FieldAlreadyOpened)
                            {
                                this.gameRenderer.Draw("Illegal move!");
                            }
                            else if (status == Board.Status.AllFieldsAreOpened)
                            {
                                this.gameRenderer.Draw(this.board.ToStringAllFieldsRevealed());
                                int score = this.board.CountOpenedFields();
                                this.gameRenderer.Draw("Congratulations! You win!!");
                                if (this.IsHighScore(score))
                                {
                                    this.gameRenderer.Draw("Please enter your name for the top scoreboard: ");
                                    string name = Console.ReadLine();
                                    Player player = new Player(name, score);
                                    this.AddTopScore(player);
                                    this.DisplayTopScores();
                                }

                                command = "restart";
                                continue;
                            }
                            else
                            {
                                this.gameRenderer.Draw(this.board.ToString());
                            }
                        }
                        catch (Exception)
                        {
                            this.gameRenderer.Draw("Illegal move");
                        }

                        break;
                    default:
                        break;
                }

                Console.Write(System.Environment.NewLine + "Enter row and column: ");

                string playerInput = Console.ReadLine();

                if (int.TryParse(playerInput, out chosenRow))
                {
                    command = "coordinates";
                    playerInput = Console.ReadLine();
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
    }
}
