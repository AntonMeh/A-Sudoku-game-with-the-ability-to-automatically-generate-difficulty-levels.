using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sudoku.Interfaces;
using Sudoku.Cells;
using Sudoku.Enums;

namespace Sudoku.Actions
{
    public abstract class SudokuGeneratorBase : ISudokuGenerator
    {
        protected readonly Random _random = new Random();

        protected SudokuCell[,] GenerateFullBoard()
        {
            SudokuCell[,] board = new SudokuCell[9, 9];
            SolveSudokuRecursive(board);
            return board;
        }

        private bool SolveSudokuRecursive(SudokuCell[,] board)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col] == null || board[row, col].Value == 0)
                    {
                        List<int> possibleNumbers = Enumerable.Range(1, 9).OrderBy(x => _random.Next()).ToList();
                        foreach (int number in possibleNumbers)
                        {
                            if (IsValidPlacement(board, number, row, col))
                            {
                                if (board[row, col] == null)
                                {
                                    board[row, col] = new SudokuCell(number, true);
                                }
                                else
                                {
                                    board[row, col].Value = number;
                                }

                                if (SolveSudokuRecursive(board))
                                {
                                    return true;
                                }

                                if (board[row, col] != null)
                                {
                                    board[row, col].Value = 0;
                                }
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        protected bool IsValidPlacement(SudokuCell[,] board, int number, int row, int col)
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[row, i] != null && board[row, i].Value == number) return false;
            }

            for (int i = 0; i < 9; i++)
            {
                if (board[i, col] != null && board[i, col].Value == number) return false;
            }

            int blockRow = row - row % 3;
            int blockCol = col - col % 3;
            for (int i = blockRow; i < blockRow + 3; i++)
            {
                for (int j = blockCol; j < blockCol + 3; j++)
                {
                    if (board[i, j] != null && board[i, j].Value == number) return false;
                }
            }
            return true;
        }

        public abstract SudokuCell[,] Generate(DifficultyLevel level);

        protected abstract int GetCellsToRemove(DifficultyLevel level);

        protected SudokuCell[,] RemoveCells(SudokuCell[,] board, int count)
        {
            SudokuCell[,] resultBoard = new SudokuCell[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    resultBoard[i, j] = new SudokuCell(board[i, j].Value, board[i, j].IsInitial); 
                }
            }

            int removedCount = 0;
            List<Tuple<int, int>> cells = Enumerable.Range(0, 9)
                .SelectMany(r => Enumerable.Range(0, 9).Select(c => Tuple.Create(r, c)))
                .OrderBy(x => _random.Next())
                .ToList();

            foreach (var cell in cells)
            {
                int row = cell.Item1;
                int col = cell.Item2;

                if (resultBoard[row, col].Value != 0 && resultBoard[row, col].IsInitial) 
                {
                    resultBoard[row, col].Value = 0;
                    resultBoard[row, col].PossibleValues = new List<int>(Enumerable.Range(1, 9)); 
                    removedCount++;

                    if (removedCount == count)
                    {
                        break;
                    }
                }
            }
            return resultBoard;
        }
    }
}
