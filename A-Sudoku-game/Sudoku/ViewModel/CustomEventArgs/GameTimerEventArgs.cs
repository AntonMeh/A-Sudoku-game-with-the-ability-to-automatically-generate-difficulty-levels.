using System;

namespace Sudoku.ViewModel.CustomEventArgs
{
    internal class GameTimerEventArgs : EventArgs
    {
        internal GameTimerEventArgs(string value)
        {
            ElapsedTime = value;
        }

        internal string ElapsedTime { get; private set; }
    }
}
