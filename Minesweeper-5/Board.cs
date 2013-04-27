//-----------------------------------------------------------------------
// <copyright file="Board.cs" company="TelerikAcademy">
// All rights reserved © Telerik Academy 2012-2013
// </copyright>
//-----------------------------------------------------------------------

namespace Minesweeper
{
    using System;
    using System.Text;
    
    /// <summary>
    /// Represents a game board for the minesweeper game
    /// </summary>
    public class Board
    {
        /// <summary>
        /// Number of rows on the boards
        /// </summary>
        private int rows;

        /// <summary>
        /// Number of columns on the board
        /// </summary>
        private int columns;

        /// <summary>
        /// Number of mines placed on the board
        /// </summary>
        private int minesCount;

        /// <summary>
        /// The body of the board
        /// </summary>
        private Field[,] fields;

        /// <summary>
        /// Initializes a new instance of the <see cref="Board" /> class.
        /// </summary>
        /// <param name="rows">Number of rows on the board</param>
        /// <param name="columns">Number of columns on the board</param>
        /// <param name="minesCount">Number of mines on the board</param>
        public Board(int rows, int columns, int minesCount)
        {
            this.rows = rows;
            this.columns = columns;
            this.minesCount = minesCount;
            this.fields = new Field[rows,columns];

            for (int row = 0; row < this.rows; row++)
            {
                for (int column = 0; column < this.columns; column++)
                {
                    this.fields[row,column] = new Field();
                }
           } 

            this.SetMines();
        }

        /// <summary>
        /// Board status after field opening
        /// </summary>
        public enum Status 
        { 
            /// <summary>
            /// Mined filed has been opened
            /// </summary>
            SteppedOnAMine,

            /// <summary>
            /// The opened field had been already opened
            /// </summary>
            FieldAlreadyOpened,

            /// <summary>
            /// The field has been successfully opened
            /// </summary>
            FieldSuccessfullyOpened,

            /// <summary>
            /// The last non-mined field has been successfully opened
            /// </summary>
            AllFieldsAreOpened
        }

        /// <summary>
        /// Prints the game board,  marking the unopened fields with '?'
        /// </summary>
        public void PrintGameBoardCurrentState()
        {
            this.PrintColumnIndexes();

            string horizontalLine = new string('_', (this.columns * 2) + 1);
            Console.WriteLine("   {0}", horizontalLine);

            for (int row = 0; row < this.rows; row++)
            {
                Console.Write(row);
                Console.Write(" | ");
                for (int column = 0; column < this.columns; column++)
                {
                    Field currentField = this.fields[row, column];
                    if (currentField.Status == Field.FieldStatus.Opened)
                    {
                        Console.Write(this.fields[row, column].Value);
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("? ");
                    }
                }

                Console.WriteLine("|");
            }

            Console.WriteLine("   {0}", horizontalLine);
        }

        /// <summary>
        /// Prints the game board, revealing all the fields. It is used when the game is over.
        /// </summary>
        public void PrintGameBoardAllFieldsRevealed()
        {
            this.PrintColumnIndexes();

            string horizontalLine = new string('_', (this.columns * 2) + 1);
            Console.WriteLine("   {0}", horizontalLine);

            for (int row = 0; row < this.rows; row++)
            {
                Console.Write(row);
                Console.Write(" | ");
                for (int column = 0; column < this.columns; column++)
                {
                    Field currentField = this.fields[row, column];
                    if (currentField.Status == Field.FieldStatus.Opened)
                    {
                        Console.Write(this.fields[row, column].Value + " ");
                    }
                    else if (currentField.Status == Field.FieldStatus.IsAMine)
                    {
                        Console.Write("* ");
                    }
                    else
                    {
                        currentField.Value = this.ScanSurroundingFields(row, column);
                        Console.Write(this.fields[row, column].Value + " ");
                    }
                }

                Console.WriteLine("|");
            }

            Console.WriteLine("   {0}", horizontalLine);
        }

        /// <summary>
        /// Opens a field on the board
        /// </summary>
        /// <param name="row">Row index of the field</param>
        /// <param name="column">Column index of the field</param>
        /// <returns>Status after field opening</returns>
        public Status OpenField(int row, int column)
        {
            Field field = this.fields[row, column];
            Status status;

            if (field.Status == Field.FieldStatus.IsAMine)
            {
                status = Status.SteppedOnAMine;
            }
            else if (field.Status == Field.FieldStatus.Opened)
            {
                status = Status.FieldAlreadyOpened;
            }
            else
            {
                field.Value = this.ScanSurroundingFields(row, column);
                field.Status = Field.FieldStatus.Opened;
                if (this.CheckIfWin())
                {
                    status = Status.AllFieldsAreOpened;
                }
                else
                {
                    status = Status.FieldSuccessfullyOpened;
                }
            }

            return status;
        }

        /// <summary>
        /// Counts how many fields are opened
        /// </summary>
        /// <returns>Number of opened fields</returns>
        public int CountOpenedFields()
        {
            int count = 0;
            for (int row = 0; row < this.rows; row++)
            {
                for (int column = 0; column < this.columns; column++)
                {
                    if (this.fields[row, column].Status == Field.FieldStatus.Opened)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Scans the surrounding fields of certain field in order to find 
        /// the number of the mines placed on the surrounding fields.
        /// </summary>
        /// <param name="row">Row index of the field</param>
        /// <param name="column">Column index of the field</param>
        /// <returns>Number of mines placed on the surrounding fields</returns>
        private int ScanSurroundingFields(int row, int column)
        {
            int mines = 0;
            if ((row > 0) &&
                (column > 0) &&
                (this.fields[row - 1, column - 1].Status == Field.FieldStatus.IsAMine))
            {
                mines++;
            }

            if ((row > 0) &&
                (this.fields[row - 1, column].Status == Field.FieldStatus.IsAMine))
            {
                mines++;
            }

            if ((row > 0) &&
                (column < this.columns - 1) &&
                (this.fields[row - 1, column + 1].Status == Field.FieldStatus.IsAMine))
            {
                mines++;
            }

            if ((column > 0) &&
                (this.fields[row, column - 1].Status == Field.FieldStatus.IsAMine))
            {
                mines++;
            }

            if ((column < this.columns - 1) &&
                (this.fields[row, column + 1].Status == Field.FieldStatus.IsAMine))
            {
                mines++;
            }

            if ((row < this.rows - 1) &&
                (column > 0) &&
                (this.fields[row + 1, column - 1].Status == Field.FieldStatus.IsAMine))
            {
                mines++;
            }

            if ((row < this.rows - 1) &&
                (this.fields[row + 1, column].Status == Field.FieldStatus.IsAMine))
            {
                mines++;
            }

            if ((row < this.rows - 1) &&
                (column < this.columns - 1) &&
                (this.fields[row + 1, column + 1].Status == Field.FieldStatus.IsAMine))
            {
                mines++;
            }

            return mines;
        }

        /// <summary>
        /// Prints the header of the game board containing the column's indexes and a horizontal line
        /// </summary>
        private void PrintColumnIndexes()
        {
            Console.Write("    ");
            for (int column = 0; column < this.columns; column++)
            {
                Console.Write(column + " ");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Places certain number of mines in randomly chosen fields on the board
        /// </summary>
        private void SetMines()
        {
            for (int mine = 0; mine < this.minesCount; mine++)
            {
                int row = this.GenerateRandomNumber(0, this.rows);
                int column = this.GenerateRandomNumber(0, this.columns);
                if (this.fields[row, column].Status == Field.FieldStatus.IsAMine)
                {
                    mine--;
                }
                else
                {
                    this.fields[row, column].Status = Field.FieldStatus.IsAMine;
                }
            }
        }

        /// <summary>
        /// Generates random number in certain interval
        /// </summary>
        /// <param name="minValue">Lower limit of the interval</param>
        /// <param name="maxValue">Upper limit of the interval</param>
        /// <returns>Generated random number</returns>
        private int GenerateRandomNumber(int minValue, int maxValue)
        {
            Random random = new Random();
            int number = random.Next(minValue, maxValue);
            return number;
        }

        /// <summary>
        /// Verifies if the game is successfully finished, checking if all the fields without mines placed on them are opened.
        /// </summary>
        /// <returns>
        /// True - if all the mined fields are opened
        /// False - if there are some mined fields unopened 
        /// </returns>
        private bool CheckIfWin()
        {
            int openedFields = this.CountOpenedFields();

            if ((openedFields + this.minesCount) == (this.rows * this.columns))
            {
                return true;
            }

            return false;
        }
    }
}
