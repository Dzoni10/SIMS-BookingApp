using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BookingApp.Model;
using BookingApp.WPF.ViewModel;

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for CheckTouristForm.xaml
    /// </summary>
    public partial class CheckTouristForm : Window
    {
        private CheckTouristViewModel checkTouristViewModel;
        public CheckTouristForm(int checkpointId, int startTourDateId, bool demo)
        {
            InitializeComponent();
            checkTouristViewModel = new CheckTouristViewModel(checkpointId, startTourDateId, demo);
            this.DataContext = checkTouristViewModel;
        }

        public void CheckBoxChecked(object sender, RoutedEventArgs e)
        {
            checkTouristViewModel.CheckBoxChecked();
        }
        public void CheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            checkTouristViewModel.CheckBoxUnchecked();
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (checkTouristViewModel.ActivatedDemo)
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
