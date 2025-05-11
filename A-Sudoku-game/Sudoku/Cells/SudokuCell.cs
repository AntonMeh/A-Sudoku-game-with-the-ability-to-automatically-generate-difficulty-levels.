using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Cells
{
    public class SudokuCell
    {
        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                if (value >= 0 && value <= 9)
                {
                    _value = value;
                    IsError = false;
                }
            }
        }

        public bool IsInitial { get; }
        public bool IsError { get; set; }
        public List<int> PossibleValues { get; set; } = new List<int>(Enumerable.Range(1, 9));

        public SudokuCell(int initialValue = 0, bool isInitial = false) 
        {
            Value = initialValue;
            IsInitial = isInitial; 
            IsError = false;
            if (IsInitial && Value != 0)
            {
                PossibleValues.Clear();
                PossibleValues.Add(Value);
            }
        }

        public void Reset()
        {
            if (!IsInitial)
            {
                Value = 0;
                IsError = false;
                PossibleValues = new List<int>(Enumerable.Range(1, 9));
            }
        }

        public bool CanSetValue(int newValue, int row, int col, SudokuCell[,] board)
        {
            for (int c = 0; c < 9; c++)
            {
                if (c != col && board[row, c].Value == newValue)
                {
                    return false;
                }
            }

            for (int r = 0; r < 9; r++)
            {
                if (r != row && board[r, col].Value == newValue)
                {
                    return false;
                }
            }

            int blockStartRow = row - row % 3;
            int blockStartCol = col - col % 3;
            for (int r = blockStartRow; r < blockStartRow + 3; r++)
            {
                for (int c = blockStartCol; c < blockStartCol + 3; c++)
                {
                    if ((r != row || c != col) && board[r, c].Value == newValue)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void RemovePossibleValue(int valueToRemove)
        {
            PossibleValues.Remove(valueToRemove);
        }

    }

}
