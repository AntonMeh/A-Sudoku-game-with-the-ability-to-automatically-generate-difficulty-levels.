using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sudoku.Enums;

namespace Sudoku.Interfaces
{
    interface ISudokuGenerator
    {
        int[,] Generate(DifficultyLevel level);
    }
}
