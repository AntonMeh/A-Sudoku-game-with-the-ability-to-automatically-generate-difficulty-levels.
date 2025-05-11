using Sudoku.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Interfaces
{
    interface ISudokuSolver
    {
        bool Solve(SudokuCell[,] board);
    }
}
