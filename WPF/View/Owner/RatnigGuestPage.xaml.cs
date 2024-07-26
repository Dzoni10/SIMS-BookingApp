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
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.DTO;
using System.Runtime.CompilerServices;
using BookingApp.Domain.Models;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.Core.Converters;
using BookingApp.Observer;
using BookingApp.WPF.ViewModel.Owner;
using System.Collections.ObjectModel;

namespace BookingApp.WPF.View.Owner
{
   
    public partial class RatnigGuestPage : Page,INotifyPropertyChanged
    {
        
        public RatnigGuestPage(AccommodationReservationDTO SelectedAccommodationReservation, ObservableCollection<AccommodationReservationDTO> AccommodationReservations,OwnerWindow parent)
        {
            InitializeComponent();
            this.DataContext = new RatingGuestViewModel(this,parent,SelectedAccommodationReservation,AccommodationReservations);
            ChangeTheme((bool)parent.ToggleThemeButton.IsChecked);
        }

        public void ChangeTheme(bool isChanged)
        {
            if (isChanged)
            {
                EnableDark();
            }
            else
            {
                EnableLight();
            }
        }

        public void EnableDark()
        {
            var uri = new Uri("../../../Resources/Images/nnn.jpg", UriKind.Relative);
            var bitmap = new BitmapImage(uri);

            var imageBrush = new ImageBrush();
            imageBrush.ImageSource = bitmap;
            this.Background = imageBrush;
        }

        public void EnableLight()
        {
            var uri = new Uri("../../../Resources/Images/rm222-mind-14.jpg", UriKind.Relative);
            var bitmap = new BitmapImage(uri);

            this.RatingBackground.ImageSource = bitmap;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)

        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
