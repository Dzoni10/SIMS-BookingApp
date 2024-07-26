using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace BookingApp.WPF.View
{
    public partial class AccommodationForumPage : Page, INotifyPropertyChanged
    {
        private readonly GuestWindow parentWindow;
        public AccommodationForumPage(GuestWindow parent, User user, int locationId, string forumViewMode)
        {
            InitializeComponent();
            this.DataContext = new AccommodationForumViewModel(parent, user, this, locationId, forumViewMode);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
