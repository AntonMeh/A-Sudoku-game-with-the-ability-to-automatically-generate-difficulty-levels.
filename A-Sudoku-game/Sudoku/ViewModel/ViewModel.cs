using System.Linq; 
using System.Text;
using System.Threading.Tasks; 
using Sudoku.Enums;
using Sudoku.ViewModel.CustomEventArgs;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Sudoku.ViewModel
{
    public class ViewModelClass : INotifyPropertyChanged
    {
        private readonly GameTimer _gameTimer = new GameTimer();
        private bool _isNewGameGenerated = false;

        public event PropertyChangedEventHandler PropertyChanged;

        private StartButtonStateEnum _startButtonState = StartButtonStateEnum.Start;
        private bool _isGameActive = false;
        private int _remainingHints;
        private DifficultyLevel _selectedDifficulty;
        private string _startButtonContent;

        public ViewModelClass()
        {
            _gameTimer.GameTimerEvent += OnGameTimerEvent;
            IsNewGameGenerated = false;
            IsGameActive = false;
            SelectedDifficulty = DifficultyLevel.Easy;
            RemainingHints = 0;
            ElapsedTime = "00:00:00";
            StartButtonState = StartButtonStateEnum.Disable;
            UpdateStartButtonContent();
        }
        public int RemainingHints
        {
            get => _remainingHints;
            set
            {
                _remainingHints = value;
                OnPropertyChanged(nameof(RemainingHints));
                OnPropertyChanged(nameof(HintMessage));
            }
        }

        public string HintMessage => $"Підказок залишилось: {RemainingHints}";

        public bool IsGameActive 
        {
            get => _isGameActive;
            set
            {
                if (_isGameActive != value)
                {
                    _isGameActive = value;
                    OnPropertyChanged(nameof(IsGameActive));
                }
            }
        }

        public DifficultyLevel SelectedDifficulty
        {
            get => _selectedDifficulty;
            set
            {
                if (_selectedDifficulty != value)
                {
                    _selectedDifficulty = value;
                    OnPropertyChanged(nameof(SelectedDifficulty));
                }
            }
        }

        public StartButtonStateEnum StartButtonState
        {
            get => _startButtonState;
            set
            {
                if (_startButtonState != value)
                {
                    _startButtonState = value;
                    OnPropertyChanged();
                    UpdateStartButtonContent(); 
                }
            }
        }

        public string StartButtonContent
        {
            get => _startButtonContent;
            set
            {
                if (_startButtonContent != value)
                {
                    _startButtonContent = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _elapsedTime;
        public string ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                if (_elapsedTime != value)
                {
                    _elapsedTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsNewGameGenerated 
        {
            get => _isNewGameGenerated;
            set
            {
                if (_isNewGameGenerated != value)
                {
                    _isNewGameGenerated = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _activeRow = -1;
        public int ActiveRow
        {
            get { return _activeRow; }
            set
            {
                if (_activeRow != value)
                {
                    _activeRow = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _activeCol = -1;
        public int ActiveCol
        {
            get { return _activeCol; }
            set
            {
                if (_activeCol != value)
                {
                    _activeCol = value;
                    OnPropertyChanged();
                }
            }
        }

        private void UpdateStartButtonContent()
        {
            switch (StartButtonState)
            {
                case StartButtonStateEnum.Start:
                    StartButtonContent = "Почати";
                    break;
                case StartButtonStateEnum.Pause:
                    StartButtonContent = "Пауза";
                    break;
                case StartButtonStateEnum.Resume:
                    StartButtonContent = "Продовжити";
                    break;
                case StartButtonStateEnum.Disable:
                    StartButtonContent = "Почати"; 
                    break;
            }
        }

        private void OnGameTimerEvent(object sender, GameTimerEventArgs e)
        {
            ElapsedTime = e.ElapsedTime;
        }

        public void StartGame()
        {
            if (!_isNewGameGenerated)
            {
                return;
            }

            switch (StartButtonState)
            {
                case StartButtonStateEnum.Start:
                    _gameTimer.StartTimer();
                    IsGameActive = true; 
                    StartButtonState = StartButtonStateEnum.Pause;
                    break;
                case StartButtonStateEnum.Pause:
                    _gameTimer.PauseTimer();
                    IsGameActive = false; 
                    StartButtonState = StartButtonStateEnum.Resume;
                    break;
                case StartButtonStateEnum.Resume:
                    _gameTimer.ResumeTimer();
                    IsGameActive = true; 
                    StartButtonState = StartButtonStateEnum.Pause;
                    break;
                case StartButtonStateEnum.Disable:
                    // Нічого не робимо, кнопка відключена
                    break;
            }
        }

        public void Finish()
        {
            _gameTimer.StopTimer();
            StartButtonState = StartButtonStateEnum.Start; 
            IsNewGameGenerated = false;
            IsGameActive = false;
        }

        public void ResetGame()
        {
            _gameTimer.StopTimer();    
            _gameTimer.Reset();   
            ElapsedTime = "00:00:00";  
            StartButtonState = StartButtonStateEnum.Start; 
            IsNewGameGenerated = false; 
            IsGameActive = false;      
            RemainingHints = 0;       
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}