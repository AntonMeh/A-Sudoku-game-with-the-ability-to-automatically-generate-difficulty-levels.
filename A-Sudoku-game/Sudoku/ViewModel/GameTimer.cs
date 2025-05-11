using System;
using System.Timers;
using Sudoku.ViewModel.CustomEventArgs;

namespace Sudoku.ViewModel
{
    internal class GameTimer
    {
        private string _initialValue = "00:00:00";
        private int _interval = 1000;

        private DateTime _startTime;
        private System.Timers.Timer _timer;

        internal event EventHandler<GameTimerEventArgs> GameTimerEvent;

        internal GameTimer()
        {
            ElapsedTime = _initialValue;
        }

        internal string ElapsedTime { get; private set; }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RaiseEvent(ElapsedTime);
        }

        internal void StartTimer()
        {
            _startTime = DateTime.Now;                      
            if (_timer == null)                            
                _timer = new System.Timers.Timer(_interval);              
            _timer.Elapsed += _timer_Elapsed;               
            _timer.AutoReset = true;                        
            _timer.Enabled = true;                         
            RaiseEvent(_initialValue);                      
        }
        protected virtual void RaiseEvent(string value)
        {
            EventHandler<GameTimerEventArgs> handler = GameTimerEvent;
            if (handler != null)
            {
                GameTimerEventArgs e = new GameTimerEventArgs(value);
                handler(this, e);
            }
        }
    }

    
}
