using System;
using System.Collections.Generic;
using System.Linq;
using Sudoku.Actions;
using Sudoku.Enums;
using Sudoku.Interfaces;
using Sudoku.Cells;

namespace Sudoku.Actions
{
    public class SudokuGenerator : SudokuGeneratorBase
    {
        protected override int GetCellsToRemove(DifficultyLevel level)
        {
            switch (level)
            {
                case DifficultyLevel.Easy:
                    return 40;
                case DifficultyLevel.Medium:
                    return 50;
                case DifficultyLevel.Hard:
                    return 60;
                default:
                    return 40;
            }
        }

        public override SudokuCell[,] Generate(DifficultyLevel level)
        {
            SudokuCell[,] board = GenerateFullBoard();
            int cellsToRemove = GetCellsToRemove(level);
            return RemoveCells(board, cellsToRemove);
        }
    }

}
