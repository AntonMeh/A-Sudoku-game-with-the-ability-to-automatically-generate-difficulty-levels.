using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sudoku.Actions;
using Sudoku.Enums;
using Sudoku.Cells;
using Sudoku.ViewModel;
using Sudoku.View;

namespace Sudoku
{
    public partial class MainWindow : Window
    {
        private readonly SudokuGenerator _generator = new SudokuGenerator();
        private readonly SudokuSolver _solver = new SudokuSolver();
        private SudokuCell[,] _board = new SudokuCell[9, 9];
        private ViewModelClass _viewModel;
        private SudokuCell[,] _solution;

        public MainWindow()
        {
            InitializeComponent();
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

                        cellTextBox.FontSize = 24;

                        cellTextBox.Tag = _board[row, col];

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

        private void Cell_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text[0]))
            {
                e.Handled = true;
            }
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (DifficultyComboBox.SelectedItem is DifficultyLevel selectedLevel)
            {
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
            }
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
                        }
                    }
                }
            }
            _board = board; 
        }

        private void DifficultyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void SolveButton_Click(object sender, RoutedEventArgs e)
        {
            SudokuCell[,] boardToSolve = GetCurrentBoardFromUI();

            bool solved = _solver.Solve(boardToSolve);

            if (solved)
            {
                DisplayBoard(boardToSolve);
            }
            else
            {
                MessageBox.Show("Ця головоломка не має розв'язку.", "Помилка");
            }
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
                            currentBoard[row, col] = new SudokuCell(value);
                        }
                        else
                        {
                            currentBoard[row, col] = new SudokuCell();
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
                            }
                        }
                        else
                        {
                            textBox.Background = Brushes.White;
                        }
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
    }
}
