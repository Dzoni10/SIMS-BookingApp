using BookingApp.WPF.ViewModel;
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

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for TourRequestWindow.xaml
    /// </summary>
    public partial class TourRequestWindow : Window
    {
        private TourRequestViewModel tourRequestViewModel;
        public TourRequestWindow(int guideId)
        {
            InitializeComponent();
            tourRequestViewModel = new TourRequestViewModel(guideId);
            this.DataContext = tourRequestViewModel;
        }
        private void StartDatePickerSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var datePicker = sender as DatePicker;
            var viewModel = DataContext as TourRequestViewModel;
            viewModel.StartDateChanged(datePicker.SelectedDate);
        }
        
        private void EndDatePickerSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var datePicker = sender as DatePicker;
            var viewModel = DataContext as TourRequestViewModel;
            viewModel.EndDateChanged(datePicker.SelectedDate);
        }

        private void DateFromPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DateFrom.IsDropDownOpen = true;
            }
        }

        private void DateToPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DateTo.IsDropDownOpen = true;
            }
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (tourRequestViewModel.ActivatedDemo)
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
