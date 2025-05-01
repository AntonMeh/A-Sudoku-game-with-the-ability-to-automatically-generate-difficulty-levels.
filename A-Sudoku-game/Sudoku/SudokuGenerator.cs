using System;
using System.Collections.Generic;
using System.Linq;
using Sudoku.Enums;

namespace Sudoku
{
    public class SudokuGenerator
    {
        private readonly Random _random = new Random();

        public int[,] Generate(DifficultyLevel level)
        {
            int[,] board = GenerateFullBoard();
            
            return board;
        }

        private int[,] GenerateFullBoard()
        {
            int[,] board = new int[9, 9];
            SolveSudoku(board); 
            return board;
        }

        private bool SolveSudoku(int[,] board)
        {
            return true;
        }

       
    }
}