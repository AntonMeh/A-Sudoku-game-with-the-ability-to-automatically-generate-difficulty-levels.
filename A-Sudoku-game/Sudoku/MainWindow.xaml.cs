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

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SudokuGenerator _generator = new SudokuGenerator();
        private SudokuCell[,] _board = new SudokuCell[9, 9];

        public MainWindow()
        {
            InitializeComponent();
            InitializeSudokuGrid();
            InitializeDifficultyComboBox();
        }

        private void InitializeDifficultyComboBox()
        {
            DifficultyComboBox.Items.Add(DifficultyLevel.Easy);
            DifficultyComboBox.Items.Add(DifficultyLevel.Medium);
            DifficultyComboBox.Items.Add(DifficultyLevel.Hard);
            DifficultyComboBox.SelectedIndex = 0;
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
                        cellTextBox.TextChanged += CellTextBox_TextChanged;

                        cellTextBox.FontSize = 24;

                        cellTextBox.Tag = _board[row, col];

                        int gridRow = row < 3 ? row : (row < 6 ? row + 1 : row + 2);
                        int gridCol = col < 3 ? col : (col < 6 ? col + 1 : col + 2);

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

        private void CellTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Tag is SudokuCell cell && int.TryParse(textBox.Text, out int newValue))
            {
                cell.Value = newValue;
            }
            else if (textBox.Tag is SudokuCell cellReset)
            {
                cellReset.Value = 0;
                textBox.Text = "";
            }
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (DifficultyComboBox.SelectedItem is DifficultyLevel selectedLevel)
            {
                SudokuCell[,] newBoard = _generator.Generate(selectedLevel);
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
                        int gridRow = row < 3 ? row : (row < 6 ? row + 1 : row + 2);
                        int gridCol = col < 3 ? col : (col < 6 ? col + 1 : col + 2);
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
            // Можна додати додаткову логіку при зміні рівня складності, наприклад,
            // автоматичний початок нової гри. Наразі нічого не робимо.
        }
    }
}
