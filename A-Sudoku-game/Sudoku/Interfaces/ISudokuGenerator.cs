using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sudoku.Cells;
using Sudoku.Enums;

namespace Sudoku.Interfaces
{
    interface ISudokuGenerator
    {
        SudokuCell[,] Generate(DifficultyLevel level);
    }
}
