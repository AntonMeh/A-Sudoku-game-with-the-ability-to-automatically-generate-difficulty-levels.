using Sudoku.Actions;
using Sudoku.Cells;
using Sudoku.Enums;
using Sudoku.View;
using Sudoku.ViewModel;
using System.Linq; 
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Sudoku
{
    public partial class MainWindow : Window
    {
        private readonly SudokuGenerator _generator = new SudokuGenerator();
        private readonly SudokuSolver _solver = new SudokuSolver();
        private SudokuCell[,] _board = new SudokuCell[9, 9]; 
        private ViewModelClass _viewModel;
        private SudokuCell[,] _solution;
        private string _completionTime;
        private bool _puzzleSolvedManually = false;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new ViewModelClass();
            DataContext = _viewModel;
            InitializeSudokuGrid();
        }

        private void InitializeSudokuGrid()
        {
            Grid sudokuGrid = (Grid)FindName("SudokuGrid");
            if (sudokuGrid != null)
            {
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        _board[row, col] = new SudokuCell();

                        TextBox cellTextBox = new TextBox();
                        cellTextBox.Style = (Style)sudokuGrid.Resources["TextBoxStyle"];
                        cellTextBox.MaxLength = 1;
                        cellTextBox.TextAlignment = TextAlignment.Center;
                        cellTextBox.VerticalContentAlignment = VerticalAlignment.Center;

                        cellTextBox.PreviewTextInput += Cell_PreviewTextInput;
                        cellTextBox.TextChanged += Cell_TextChanged;
                        cellTextBox.GotFocus += Cell_GotFocus; 

                        cellTextBox.Tag = new Tuple<int, int>(row, col); 

                        (var gridRow, var gridCol) = ToUIGridPosition(row, col);
                        Border cellBorder = sudokuGrid.Children
                            .OfType<Border>()
                            .FirstOrDefault(b => Grid.GetRow(b) == gridRow && Grid.GetColumn(b) == gridCol);

                        if (cellBorder != null)
                        {
                            cellBorder.Child = cellTextBox;
                        }
                    }
                }
            }
        }

        private void Cell_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Tag is Tuple<int, int> coords)
            {
                _viewModel.ActiveRow = coords.Item1;
                _viewModel.ActiveCol = coords.Item2;
            }
        }

        private void Cell_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!_viewModel.IsGameActive || !char.IsDigit(e.Text[0]) || e.Text == "0")
            {
                e.Handled = true;
            }
        }

        private void Cell_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Tag is SudokuCell cell)
                {
                    if (int.TryParse(textBox.Text, out int newValue) && newValue != 0)
                    {
                        if (!cell.IsInitial)
                        {
                            textBox.Foreground = Brushes.Blue; 
                            cell.Value = newValue;
                        }
                    }
                    else 
                    {
                        if (!cell.IsInitial) 
                        {
                            textBox.Foreground = Brushes.Blue; 
                            cell.Value = 0; 
                        }
                    }
                }
            }
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.IsGameActive || _viewModel.ElapsedTime != "00:00:00")
            {
                MessageBoxResult result = MessageBox.Show(
                    "Ви впевнені, що хочете почати нову гру? Весь прогрес поточної гри буде втрачено.",
                    "Нова гра",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            if (DifficultyComboBox.SelectedItem is not DifficultyLevel selectedLevel) 
            {
                MessageBox.Show("Будь ласка, виберіть рівень складності перед початком нової гри.", "Увага");
                return;
            }

            _viewModel.ResetGame();

            SudokuCell[,] newBoard = _generator.Generate(selectedLevel);

            SudokuCell[,] solutionCopy = _solver.DeepCopyBoard(newBoard);
            if (_solver.Solve(solutionCopy))
            {
                _solution = solutionCopy;
            }
            else
            {
                MessageBox.Show("Помилка при генерації розв’язку.");
                return;
            }

            DisplayBoard(newBoard); 
            _board = newBoard;

            _viewModel.IsNewGameGenerated = true; 
            _viewModel.IsGameActive = false;      
            _viewModel.StartButtonState = StartButtonStateEnum.Start; 

            _viewModel.RemainingHints = DifficultySettings.GetHintCount(selectedLevel);

            _puzzleSolvedManually = false; 
        }

        private void DisplayBoard(SudokuCell[,] board)
        {
            Grid sudokuGrid = (Grid)FindName("SudokuGrid");
            if (sudokuGrid != null)
            {
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        (var gridRow, var gridCol) = ToUIGridPosition(row, col);
                        Border cellBorder = sudokuGrid.Children
                            .OfType<Border>()
                            .FirstOrDefault(b => Grid.GetRow(b) == gridRow && Grid.GetColumn(b) == gridCol);

                        if (cellBorder?.Child is TextBox textBox)
                        {
                            textBox.Text = board[row, col].Value == 0 ? "" : board[row, col].Value.ToString();
                            textBox.IsReadOnly = board[row, col].IsInitial;

                            if (board[row, col].IsInitial)
                            {
                                textBox.Foreground = Brushes.Black; 
                            }
                            else
                            {
                                textBox.Foreground = Brushes.Blue; 
                            }
                            textBox.Background = Brushes.White; 
                        }
                    }
                }
            }
            _board = board;
        }

        private void SolveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.IsNewGameGenerated)
            {
                MessageBox.Show("Спочатку створіть нову гру.", "Увага");
                return;
            }

            if (!_viewModel.IsGameActive)
            {
                MessageBox.Show("Почніть гру, щоб отримати підказку.", "Увага");
                return;
            }

            if (_viewModel.RemainingHints <= 0)
            {
                MessageBox.Show("У вас закінчилися підказки.", "Підказка");
                return;
            }

            int activeRow = _viewModel.ActiveRow;
            int activeCol = _viewModel.ActiveCol;

            if (activeRow == -1 || activeCol == -1)
            {
                MessageBox.Show("Будь ласка, оберіть клітинку, щоб отримати підказку.", "Увага");
                return;
            }

            if (_board[activeRow, activeCol].Value != 0 || _board[activeRow, activeCol].IsInitial)
            {
                MessageBox.Show("Обрана клітинка вже заповнена або є початковим числом.", "Увага");
                return;
            }

            _board[activeRow, activeCol].Value = _solution[activeRow, activeCol].Value;
            _board[activeRow, activeCol].IsInitial = true; 

            (var uiRow, var uiCol) = ToUIGridPosition(activeRow, activeCol);
            Grid sudokuGrid = (Grid)FindName("SudokuGrid");

            Border cellBorder = sudokuGrid.Children
                .OfType<Border>()
                .FirstOrDefault(b => Grid.GetRow(b) == uiRow && Grid.GetColumn(b) == uiCol);

            if (cellBorder?.Child is TextBox textBox)
            {
                textBox.Text = _solution[activeRow, activeCol].Value.ToString();
                textBox.Background = Brushes.LightYellow; 
                textBox.IsReadOnly = true; 
                textBox.Foreground = Brushes.Black; 
            }

            _viewModel.RemainingHints--; 
        }

        private SudokuCell[,] GetCurrentBoardFromUI()
        {
            SudokuCell[,] currentBoard = new SudokuCell[9, 9];
            Grid sudokuGrid = (Grid)FindName("SudokuGrid");

            if (sudokuGrid != null)
            {
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        (var gridRow, var gridCol) = ToUIGridPosition(row, col);
                        Border cellBorder = sudokuGrid.Children
                            .OfType<Border>()
                            .FirstOrDefault(b => Grid.GetRow(b) == gridRow && Grid.GetColumn(b) == gridCol);

                        if (cellBorder?.Child is TextBox textBox && int.TryParse(textBox.Text, out int value))
                        {
                            currentBoard[row, col] = new SudokuCell(value, _board[row, col].IsInitial);
                        }
                        else
                        {
                            currentBoard[row, col] = new SudokuCell(0, _board[row, col].IsInitial);
                        }
                    }
                }
            }
            return currentBoard;
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (_solution == null)
            {
                MessageBox.Show("Немає збереженого розв’язку для перевірки.");
                return;
            }

            SudokuCell[,] currentBoard = GetCurrentBoardFromUI();
            Grid sudokuGrid = (Grid)FindName("SudokuGrid");
            if (sudokuGrid == null) return;

            bool allCorrect = true;

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    (int uiRow, int uiCol) = ToUIGridPosition(row, col);
                    Border cellBorder = sudokuGrid.Children
                        .OfType<Border>()
                        .FirstOrDefault(b => Grid.GetRow(b) == uiRow && Grid.GetColumn(b) == uiCol);

                    if (cellBorder?.Child is TextBox textBox)
                    {
                        if (int.TryParse(textBox.Text, out int playerValue))
                        {
                            if (playerValue == _solution[row, col].Value)
                            {
                                textBox.Background = Brushes.LightGreen;
                            }
                            else
                            {
                                textBox.Background = Brushes.LightCoral;
                                allCorrect = false;
                            }
                        }
                        else 
                        {
                            textBox.Background = Brushes.White; 
                            allCorrect = false;
                        }

                        if (_board[row, col].IsInitial) 
                        {
                            textBox.Foreground = Brushes.Black;
                        }
                        else
                        {
                            textBox.Foreground = Brushes.Blue;
                        }
                    }
                }
            }

            if (allCorrect)
            {
                if (DataContext is ViewModelClass viewModel)
                {
                    _completionTime = viewModel.ElapsedTime;
                    viewModel.Finish();

                    int totalHints = DifficultySettings.GetHintCount(viewModel.SelectedDifficulty);
                    int remaining = viewModel.RemainingHints;

                    GameComplete gameComplete = new GameComplete(_completionTime, totalHints, remaining);
                    gameComplete.Owner = this;
                    gameComplete.ShowDialog();

                    if (gameComplete.StartNewGameRequested)
                    {
                        NewGameButton_Click(null, null);
                    }
                }
            }
        }

        private (int uiRow, int uiCol) ToUIGridPosition(int row, int col)
        {
            int uiRow = row < 3 ? row : (row < 6 ? row + 1 : row + 2);
            int uiCol = col < 3 ? col : (col < 6 ? col + 1 : col + 2);
            return (uiRow, uiCol);
        }

        private void RulesButton_Click(object sender, RoutedEventArgs e)
        {
            RulesBox aboutBox;
            try
            {
                aboutBox = new RulesBox();
                aboutBox.Owner = this;
                aboutBox.ShowDialog();
            }
            finally
            {
                aboutBox = null;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

            if (_puzzleSolvedManually)
            {
                MessageBox.Show("Цей пазл вже розв’язано. Створіть нову гру, щоб почати.", "Неможливо почати гру");
                return;
            }

            if (DataContext is ViewModelClass viewModel)
            {
                viewModel.StartGame();
            }
        }

        public string GetCompletionTime()
        {
            return _completionTime;
        }
    }
}