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
        public bool Solve(int[,] board)
        {
            return true;
        }
        public bool Solve(SudokuCell[,] board)
        {
            return SolveSudokuRecursive(board);
        }

        private bool SolveSudokuRecursive(SudokuCell[,] board)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col].Value == 0) 
                    {
                        for (int number = 1; number <= 9; number++)
                        {
                            if (board[row, col].CanSetValue(number, row, col, board))
                            {
                                board[row, col].Value = number;

                                if (SolveSudokuRecursive(board)) 
                                   return true; 
                                

                                board[row, col].Value = 0; 
                            }
                        }
                        return false; 
                    }
                }
            }
            return true; 
        }
    }
}
