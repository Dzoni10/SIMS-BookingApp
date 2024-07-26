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
    /// Interaction logic for ComplexTourRequestWindow.xaml
    /// </summary>
    public partial class ComplexTourRequestWindow : Window
    {
        private ComplexTourRequestViewModel complexTourRequestViewModel;
        public ComplexTourRequestWindow(int userId)
        {
            InitializeComponent();
            complexTourRequestViewModel= new ComplexTourRequestViewModel(userId);
            DataContext = complexTourRequestViewModel;
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (complexTourRequestViewModel.ActivatedDemo)
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
