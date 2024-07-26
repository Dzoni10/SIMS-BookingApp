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
using BookingApp.DTO;
using BookingApp.Model;
using System.Data;
using BookingApp.WPF.ViewModel;
using iText.Forms.Logs;
using BookingApp.Application.UseCases;


namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for FollowTourWindow.xaml
    /// </summary>
    public partial class FollowTourWindow : Window
    {
        private FollowTourViewModel followTourViewModel; 
        public FollowTourWindow(int userId)
        {
            InitializeComponent();
            followTourViewModel = new FollowTourViewModel(userId);
            DataContext = followTourViewModel;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (followTourViewModel.ActivatedDemo)
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
