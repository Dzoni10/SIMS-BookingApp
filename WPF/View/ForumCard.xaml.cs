using BookingApp.Domain.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookingApp.WPF.ViewModel;

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for ForumCard.xaml
    /// </summary>
    public partial class ForumCard : UserControl
    {
        private readonly GuestWindow parentWindow;
        private readonly User user;
        public ForumCard(GuestWindow parentWindow, User user, int commentId)
        {
            InitializeComponent();
            this.DataContext = new ForumCardViewModel(parentWindow, user, this, commentId);
        }
    }
}
