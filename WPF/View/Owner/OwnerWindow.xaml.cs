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
using System.Windows.Shapes;
using BookingApp.WPF.View.Owner;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BookingApp.Domain.Models;
using System.Windows.Navigation;
using BookingApp.WPF.ViewModel.Owner;
using System.Windows.Media.Imaging;
using BookingApp.Localization;
using PdfSharp.BigGustave;

namespace BookingApp.WPF.View.Owner
{
    public partial class OwnerWindow : Window
    {
        private string username;
        private App app;
        private const string SRB = "sr-Latn-RS";
        private const string ENG = "en-US";
        public OwnerWindow(string username)
        {
            InitializeComponent();
            this.DataContext = new OwnerWindowViewModel(this);
            this.username = username;
            app = (App)System.Windows.Application.Current;
            app.ChangeLanguage(ENG);
            //LanguageComboBox.ItemsSource = new string[] { "Serbian", "English" };
        }

        public void DarkThemeChange(object sender, RoutedEventArgs e)
        {
            Uri imageUri = new Uri("../../../Resources/Images/nnn.jpg", UriKind.Relative); 
            BitmapImage bitmapImage = new BitmapImage(imageUri);
            this.BackgroundImage.ImageSource = bitmapImage;
            LangLabel.Style = FindResource("DarkLabelStyle") as Style;
            ThemeLabel.Style = FindResource("DarkLabelStyle") as Style;
            RightsLabel.Style = FindResource("DarkLabelStyle") as Style;
        }

        public void LightThemeChange(object sender, RoutedEventArgs e)
        {
            Uri imageUri = new Uri("../../../Resources/Images/rm222-mind-14.jpg", UriKind.Relative);
            BitmapImage bitmapImage = new BitmapImage(imageUri);
            this.BackgroundImage.ImageSource = bitmapImage;
            LangLabel.Style = FindResource("LightLabelStyle") as Style;
            ThemeLabel.Style = FindResource("LightLabelStyle") as Style;
            RightsLabel.Style = FindResource("LightLabelStyle") as Style;
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ChangeLanguage(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageComboBox.SelectedIndex != -1)
            {
                if (LanguageComboBox.SelectedIndex == 0)
                {
                    app.ChangeLanguage(SRB);
                    Properties.Settings.Default.currentLanguage = "sr-Latn-RS";
                }
                else
                {
                    app.ChangeLanguage(ENG);
                    Properties.Settings.Default.currentLanguage = "en-US";
                }
            }
        }
    }
}
