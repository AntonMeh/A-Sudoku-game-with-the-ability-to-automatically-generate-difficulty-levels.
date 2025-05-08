using Sudoku.Cells;
using Sudoku.Enums;
using Sudoku.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Difficulties
{
    class MediumSudokuGenerator : SudokuGeneratorBase
    {
        protected override int GetCellsToRemove(DifficultyLevel level) => 50;

        public override SudokuCell[,] Generate(DifficultyLevel level)
        {
            SudokuCell[,] board = GenerateFullBoard();
            int cellsToRemove = GetCellsToRemove(level);
            return RemoveCells(board, cellsToRemove);
        }
    }
}
