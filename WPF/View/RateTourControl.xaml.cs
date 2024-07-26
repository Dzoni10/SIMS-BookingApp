using BookingApp.Domain.Models;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.WPF.ViewModel;
using Microsoft.Win32;
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
using Image = BookingApp.Domain.Models.Image;

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for RateTourControl.xaml
    /// </summary>
    public partial class RateTourControl : UserControl
    {
        public RateTourControl(string name, User user, int startTourDateId)
        {
            InitializeComponent();
            this.DataContext = new RateTourControlViewModel(this, name, startTourDateId, user); ;
        }

    }
}
