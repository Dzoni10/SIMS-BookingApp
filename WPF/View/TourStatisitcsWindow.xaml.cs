using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.WPF.ViewModel;
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
using System.Windows.Shapes;

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for TourStatisitcsWindow.xaml
    /// </summary>
    public partial class TourStatisitcsWindow : Window
    {

        public TourStatisitcsWindow(TourDTO selectedTour)
        {
            InitializeComponent();
            this.DataContext = new TourStatisitcsViewModel(selectedTour);
        }
    }
}
