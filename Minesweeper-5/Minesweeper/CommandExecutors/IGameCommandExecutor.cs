namespace Minesweeper.CommandExecutors
{
    public interface IGameCommandExecutor
    {
        void Start();

        void ProcessUserInput(ref int chosenRow, ref int chosenColumn, ref string command);

        void InvalidInput();

        void CheckCoordinates(int chosenRow, int chosenColumn);

        void DisplayTopScores();

        void RestartGame();

        void EndGame(string message);

        void ExitGame();

    }
}
