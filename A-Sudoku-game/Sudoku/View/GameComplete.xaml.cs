using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sudoku.View
{
    public partial class GameComplete : Window
    {
        public bool StartNewGameRequested { get; private set; } = false;

        public GameComplete(string completionTime, int totalHints, int remainingHints)
        {
            InitializeComponent();

            CompletionTimeTextBlock.Text = $"Час проходження: {completionTime}";
            int usedHints = totalHints - remainingHints;
            HintsUsedTextBlock.Text = $"Використано підказок: {usedHints} з {totalHints}";
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewGameRequested = true;
            Close();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
