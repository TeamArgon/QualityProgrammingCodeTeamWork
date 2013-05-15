using Minesweeper.InputMethods;

namespace MinesweeperUnitTests.MockClasses
{
    public class MockInputMethod : IInputMethod
    {
        string mockInput;

        public void SetInput(string mockInput)
        {
            this.mockInput = mockInput;
        }

        public string GetUserInput()
        {
            return mockInput;
        }
    }
}
