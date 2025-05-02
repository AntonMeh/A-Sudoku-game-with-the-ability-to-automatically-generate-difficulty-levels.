using System;
using System.Collections.Generic;
using System.Linq;
using Sudoku.Enums;
using Sudoku.Interfaces;
using Sudoku.Actions;

namespace Sudoku.Actions
{
    public class SudokuGenerator : ISudokuGenerator
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
            var solver = new SudokuSolver();
            solver.Solve(board);
            return board;
        }
       
    }
}