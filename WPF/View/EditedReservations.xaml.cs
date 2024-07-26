using BookingApp.Domain.Models;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.WPF.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.View
{
    public partial class EditedReservations : Page
    {
        private readonly GuestWindow parentWindow;
        private readonly User user;

        public EditedReservations(GuestWindow parent, User user)
        {
            InitializeComponent();
            this.DataContext = new EditedReservationsViewModel(parent, user, this);
            this.parentWindow = parent;

        }
    }
}
