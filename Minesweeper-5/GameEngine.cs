namespace Minesweeper
{
    using System;
    using System.Collections.Generic;

    public class GameEngine
    {
        private const int maxRows = 5;
        private const int maxColumns = 10;
        private const int maxMines = 15;
        private const int maxTopPlayers = 5;
        private IRenderer gameRenderer;
        private Board board;
        private List<Player> topPlayers;

        public GameEngine(IRenderer renderer)
        {
            this.gameRenderer = renderer;
            this.topPlayers = new List<Player>();
            this.topPlayers.Capacity = maxTopPlayers;
            this.GenerateNewBoard();
        }

        public static void Main(string[] args)
        {
            GameEngine ge = new GameEngine(new ConsoleRenderer());
            ge.StartGame();
        }

        public void GenerateNewBoard()
        {
            this.board = new Board(maxRows, maxColumns, maxMines);
        }

        private bool CheckHighScores(int score)
        {
            if (topPlayers.Capacity > topPlayers.Count)
            {
                return true;
            }

            foreach (Player currentPlayer in topPlayers)
            {
                if (currentPlayer.Score < score)
                {
                    return true;
                }
            }

            return false;
        }

        private void AddTopScore(Player player)
        {
            if (topPlayers.Capacity > topPlayers.Count)
            {
                topPlayers.Add(player);
                topPlayers.Sort();
            }
            else
            {
                topPlayers.RemoveAt(topPlayers.Capacity - 1);
                topPlayers.Add(player);
                topPlayers.Sort();
            }
        }

        private void DisplayTopScores()
        {
            Console.WriteLine("Scoreboard");
            for (int i = 0; i < topPlayers.Count; i++)
            {
                Console.WriteLine((int)(i + 1) + ". " + topPlayers[i]);
            }
        }

        private void StartGame()
        {
            string str = "restart";
            int choosenRow = 0;
            int chosenColumn = 0;

            while (str != "exit")
            {
                if (str == "restart")
                {
                    this.GenerateNewBoard();
                    Console.WriteLine("Welcome to the game “Minesweeper”. " +
                        "Try to reveal all cells without mines. " +
                        "Use 'top' to view the scoreboard, 'restart' to start a new game" +
                        "and 'exit' to quit the game.");
                    Console.WriteLine(board);
                }
                else if (str == "exit")
                {
                    Console.WriteLine("Good bye!");
                    Console.Read();
                }
                else if (str == "top")
                {
                    DisplayTopScores();
                }
                else if (str == "coordinates")
                {
                    try
                    {
                        Board.Status status = board.OpenField(choosenRow, chosenColumn);
                        if (status == Board.Status.SteppedOnAMine)
                        {
                            Console.WriteLine(board.ToStringAllFieldsRevealed());
                            int score = board.CountOpenedFields();
                            Console.WriteLine("Booooom! You were killed by a mine. You revealed " +
                                score +
                                " cells without mines.");

                            if (CheckHighScores(score))
                            {
                                Console.WriteLine("Please enter your name for the top scoreboard: ");
                                string name = Console.ReadLine();
                                Player player = new Player(name, score);
                                AddTopScore(player);
                                DisplayTopScores();
                            }

                            str = "restart";
                            continue;
                        }
                        else if (status == Board.Status.FieldAlreadyOpened)
                        {
                            Console.WriteLine("Illegal move!");
                        }
                        else if (status == Board.Status.AllFieldsAreOpened)
                        {
                            Console.WriteLine(board.ToStringAllFieldsRevealed());
                            int score = board.CountOpenedFields();
                            Console.WriteLine("Congratulations! You win!!");
                            if (CheckHighScores(score))
                            {
                                Console.WriteLine("Please enter your name for the top scoreboard: ");
                                string name = Console.ReadLine();
                                Player player = new Player(name, score);
                                AddTopScore(player);

                                // pokazvame klasiraneto
                                DisplayTopScores();
                            }

                            str = "restart";
                            continue;
                        }
                        else
                        {
                            Console.WriteLine(board);
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Illegal move");
                    }
                }

                Console.Write(System.Environment.NewLine + "Enter row and column: ");

                str = Console.ReadLine();
                try
                {
                    choosenRow = int.Parse(str);
                    str = "coordinates";
                }
                catch
                {
                    // niama smisal tuka
                    continue;
                }

                str = Console.ReadLine();
                try
                {
                    chosenColumn = int.Parse(str);
                    str = "coordinates";
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
    }
}
