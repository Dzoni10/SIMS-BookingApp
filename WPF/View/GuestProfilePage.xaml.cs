using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModel;
using LiveCharts;
using LiveCharts.Wpf;
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
    public partial class GuestProfilePage : Page, INotifyPropertyChanged
    {
        private readonly GuestWindow parentWindow;
        private readonly User user;
/*
        public string[] Months { get; set; }
        public string[] TravelsCount { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
*/
        public GuestProfilePage(GuestWindow parent, User user)
        {
            InitializeComponent();
            this.DataContext = new GuestProfileViewModel(parent, user, this);
            
            /*
            DataContext = this;

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "2022",
                    Values = new ChartValues<double>{3, 5, 6, 2, 5, 1, 6, 2, 2, 3, 6, 4},
                    ColumnPadding = 10
                }
            };

            SeriesCollection.Add(new ColumnSeries
            {
                Title = "2023",
                Values = new ChartValues<double> { 7, 8, 3, 6, 9, 2, 4, 5, 1, 2, 5, 8 },
                ColumnPadding = 6
            });

            SeriesCollection.Add(new ColumnSeries
            {
                Title = "2024",
                Values = new ChartValues<double> { 6, 2, 1, 4, 7, 8, 1, 7, 2, 9, 4, 5 },
                ColumnPadding = 3
            });

            Months = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            TravelsCount = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

            */

        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
