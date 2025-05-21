using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Enums
{
    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }

    public static class DifficultySettings
    {
        public static int GetHintCount(DifficultyLevel level)
        {
            return level switch
            {
                DifficultyLevel.Easy => 5,
                DifficultyLevel.Medium => 3,
                DifficultyLevel.Hard => 1,
                _ => 0
            };
        }
    }

}
