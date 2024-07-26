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
using BookingApp.Domain.Models;

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for AnywhereAnytimePage.xaml
    /// </summary>
    public partial class AnywhereAnytimePage : Page, INotifyPropertyChanged
    {
        public AnywhereAnytimePage(GuestWindow parent, User user)
        {
            InitializeComponent();
            this.DataContext = new AnywhereAnytimeViewModel(parent, user, this);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
