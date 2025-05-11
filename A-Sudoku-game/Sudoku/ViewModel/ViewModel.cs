using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sudoku.Enums;


namespace Sudoku.ViewModel
{
    class ViewModelClass : INotifyPropertyChanged
    {
        private static ViewModelClass _instance;

        private GameTimer _timer;
        private MainWindow _view;

        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
