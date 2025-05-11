using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sudoku.Cells;
using Sudoku.Interfaces;

namespace Sudoku.Actions
{
    internal class SudokuSolver : ISudokuSolver
    {
        public bool Solve(SudokuCell[,] board)
        {
            return SolveRecursive(board);
        }

        private bool SolveRecursive(SudokuCell[,] board)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col].Value == 0)
                    {
                        for (int number = 1; number <= 9; number++)
                        {
                            if (IsValidPlacement(board, number, row, col))
                            {
                                board[row, col].Value = number;

                                if (SolveRecursive(board))
                                {
                                    return true; 
                                }

                                board[row, col].Value = 0;
                            }
                        }
                        return false; 
                    }
                }
            }
            return true; 
        }

        private bool IsValidPlacement(SudokuCell[,] board, int number, int row, int col)
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[row, i].Value == number) return false;
            }

            for (int i = 0; i < 9; i++)
            {
                if (board[i, col].Value == number) return false;
            }

            int blockRow = row - row % 3;
            int blockCol = col - col % 3;
            for (int i = blockRow; i < blockRow + 3; i++)
            {
                for (int j = blockCol; j < blockCol + 3; j++)
                {
                    if (board[i, j].Value == number) return false;
                }
            }

            return true;
        }
        public SudokuCell[,] GetSolution(SudokuCell[,] board)
        {
            SudokuCell[,] boardToSolve = DeepCopyBoard(board); 

            if (SolveInternal(boardToSolve))
            {
                return boardToSolve;
            }
            else
            {
                return null; 
            }
        }

        private bool SolveInternal(SudokuCell[,] board)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col].Value == 0) 
                    {
                        for (int number = 1; number <= 9; number++)
                        {
                            if (IsValidPlacement(board, number, row, col))
                            {
                                board[row, col].Value = number;

                                if (SolveInternal(board))
                                {
                                    return true; 
                                }

                                board[row, col].Value = 0; 
                            }
                        }
                        return false; 
                    }
                }
            }
            return true;
        }

        public SudokuCell[,] DeepCopyBoard(SudokuCell[,] original)
        {
            SudokuCell[,] copy = new SudokuCell[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    copy[i, j] = new SudokuCell(original[i, j].Value, original[i, j].IsInitial) { IsError = original[i, j].IsError };
                }
            }
            return copy;
        }
    }
}
