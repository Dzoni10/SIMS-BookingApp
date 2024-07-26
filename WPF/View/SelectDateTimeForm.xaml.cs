using BookingApp.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for SelectDateTimeForm.xaml
    /// </summary>
    public partial class SelectDateTimeForm : Window
    {
        private SelectDateTimeViewModel selectDateTimeViewModel;

        public SelectDateTimeForm(DateTime dateFrom, DateTime dateTo, bool demo)
        {
            InitializeComponent();
            selectDateTimeViewModel = new SelectDateTimeViewModel(this, dateFrom, dateTo, demo);
            DataContext = selectDateTimeViewModel;
        }

        private void StartDate_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                StartDate.IsDropDownOpen = true;
            }
        }

        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            selectDateTimeViewModel.StartDateSelectedDateChanged();
        }

        private void StartTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            selectDateTimeViewModel.StartTimeValueChanged();
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (selectDateTimeViewModel.ActivatedDemo)
            {
                if (!(Keyboard.IsKeyDown(Key.G) || (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))) ||
                (e.Key != Key.G && e.Key != Key.LeftCtrl && e.Key != Key.RightCtrl))
                {
                    e.Handled = true;
                }
            }
        }
    }
}
