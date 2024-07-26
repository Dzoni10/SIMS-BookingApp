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
using BookingApp.DTO;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    public partial class RenovatingPage : Page, INotifyPropertyChanged
    {
        public RenovatingPage(OwnerWindow parent)
        {
            InitializeComponent();
            this.DataContext = new RenovatingViewModel(this, parent);
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
            this.RenovatingBackground.ImageSource = bitmap;
            TitleLabel.Style = FindResource("DarkLabelStyle") as Style;
        }

        public void EnableLight()
        {
            var uri = new Uri("../../../Resources/Images/rm222-mind-14.jpg", UriKind.Relative);
            var bitmap = new BitmapImage(uri);
            this.RenovatingBackground.ImageSource = bitmap;
            TitleLabel.Style = FindResource("LightLabelStyle") as Style;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
