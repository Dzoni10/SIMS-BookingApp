using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.DTO;
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
    public partial class AccommodationPreviewPage : Page, INotifyPropertyChanged
    {
        public AccommodationPreviewPage(GuestWindow parent, User user, int accommodationId, AccommodationReservation reservation, string viewMode)
        {
            InitializeComponent();
            this.DataContext = new AccommodationPreviewViewModel(parent, user, accommodationId, reservation, viewMode);

            //DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
