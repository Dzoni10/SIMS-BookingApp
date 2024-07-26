using BookingApp.DTO;
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
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    public partial class ViewStatsPage : Page
    {
        public ViewStatsPage(OwnerWindow parent,AccommodationDTO SelectedAccommodation)
        {
            InitializeComponent();
            this.DataContext = new ViewStatsViewModel(this,parent, SelectedAccommodation);
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
            this.StatsBackground.ImageSource = bitmap;
        }

        public void EnableLight()
        {
            var uri = new Uri("../../../Resources/Images/rm222-mind-14.jpg", UriKind.Relative);
            var bitmap = new BitmapImage(uri);
            this.StatsBackground.ImageSource = bitmap;
        }
    }
}
