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

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SudokuGenerator _generator = new SudokuGenerator();
        public MainWindow()
        {
            InitializeComponent();
            Grid sudokuGrid = (Grid)FindName("SudokuGrid");

            if (sudokuGrid != null)
            {
                int borderIndex = 0;
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        TextBox cell = new TextBox();
                        cell.Style = (Style)sudokuGrid.Resources["TextBoxStyle"];
                        cell.MaxLength = 1;
                        cell.TextAlignment = TextAlignment.Center;
                        cell.VerticalContentAlignment = VerticalAlignment.Center;

                        cell.PreviewTextInput += Cell_PreviewTextInput;

                        int gridRow = row < 3 ? row : (row < 6 ? row + 1 : row + 2);
                        int gridCol = col < 3 ? col : (col < 6 ? col + 1 : col + 2);

                        try
                        {
                            Border cellBorder = sudokuGrid.Children.OfType<Border>().FirstOrDefault(b => Grid.GetRow(b) == gridRow && Grid.GetColumn(b) == gridCol);

                            cellBorder.Child = cell;
                        }
                        catch (ArgumentNullException) { }               
                        
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
            DifficultyLevel selectedLevel = (DifficultyLevel)DifficultyComboBox.SelectedItem;
            int[,] newBoard = _generator.Generate(selectedLevel);
            DisplayBoard(newBoard);
        }

        private void DisplayBoard(int[,] board)
        {
            
        }

    }
}
