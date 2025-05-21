using System;
using System.Timers;
using Sudoku.ViewModel.CustomEventArgs;

namespace Sudoku.ViewModel
{
    internal class GameTimer
    {
        private const string InitialValue = "00:00:00";
        private const string TimeFormat = "hh\\:mm\\:ss";
        private const int Interval = 1000;

        private DateTime _startTime;
        private System.Timers.Timer _timer;


        public event EventHandler<GameTimerEventArgs> GameTimerEvent;

        public string ElapsedTime { get; private set; } = InitialValue;

        private bool _isPaused = false;
        private TimeSpan _pausedTime = TimeSpan.Zero;

        public void StartTimer()
        {
            _startTime = DateTime.Now;
            _pausedTime = TimeSpan.Zero;
            _isPaused = false;

            if (_timer == null)
            {
                _timer = new System.Timers.Timer(Interval);
                _timer.Elapsed += OnTimerElapsed;
                _timer.AutoReset = true;
            }

            _timer.Start();
            RaiseEvent(InitialValue);
        }

        public void PauseTimer()
        {
            if (_timer?.Enabled == true)
            {
                _timer.Stop();
                _pausedTime += DateTime.Now - _startTime;
                _isPaused = true;
            }
        }

        public void ResumeTimer()
        {
            if (_timer != null && _isPaused)
            {
                _startTime = DateTime.Now;
                _timer.Start();
                _isPaused = false;
            }
        }

        public void StopTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
                UpdateElapsedTime();
                RaiseEvent(""); 
                _timer.Dispose();
                _timer = null;
            }
        }

        public void Reset()
        {
            ElapsedTime = InitialValue;
            _startTime = DateTime.Now;
            _pausedTime = TimeSpan.Zero;
            _isPaused = false;
            RaiseEvent(InitialValue);
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            UpdateElapsedTime();
            RaiseEvent(ElapsedTime);
        }

        private void UpdateElapsedTime()
        {
            TimeSpan diff = (DateTime.Now - _startTime) + _pausedTime;
            ElapsedTime = diff.ToString(TimeFormat);
        }

        protected virtual void RaiseEvent(string value)
        {
            GameTimerEvent?.Invoke(this, new GameTimerEventArgs(value));
        }
    }
}
