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
using BookingApp.Repository;
using BookingApp.DTO;
using BookingApp.Model;
using System.Collections.ObjectModel;
using System.Security.RightsManagement;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
   
    public partial class IgnorePage : Page, INotifyPropertyChanged
    {
        
        public IgnorePage(EditedReservationDTO SelectedEditedReservation,ObservableCollection<EditedReservationDTO> EditedReservations,OwnerWindow parent)
        {
            InitializeComponent();
            this.DataContext= new IgnoreViewModel(this,SelectedEditedReservation,EditedReservations);
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
            this.IgnoreBackground.ImageSource = bitmap;
        }

        public void EnableLight()
        {
            var uri = new Uri("../../../Resources/Images/rm222-mind-14.jpg", UriKind.Relative);
            var bitmap = new BitmapImage(uri);
            this.IgnoreBackground.ImageSource = bitmap;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
