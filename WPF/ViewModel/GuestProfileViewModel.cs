using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.DTO;
using BookingApp.WPF.View;
using iText.StyledXmlParser.Jsoup.Nodes;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel
{
    public class GuestProfileViewModel : INotifyPropertyChanged
    {
        public GuestProfilePage page;
        private readonly GuestWindow parentWindow;
        private readonly User user;
        private GuestService guestService;
        private UserService userService;
        private AccommodationReservationService accommodationReservationService;

        public GuestDTO Guest { get; set; }



        private SeriesCollection pieSeriesCollection;
        public SeriesCollection PieSeriesCollection
        {
            get { return pieSeriesCollection; }
            set
            {
                pieSeriesCollection = value;
                OnPropertyChanged(nameof(PieSeriesCollection));
            }
        }

        private SeriesCollection cartesianSeriesCollection;
        public SeriesCollection CartesianSeriesCollection
        {
            get { return cartesianSeriesCollection; }
            set
            {
                cartesianSeriesCollection = value;
                OnPropertyChanged(nameof(CartesianSeriesCollection));
            }
        }

        private ObservableCollection<string> months;
        public ObservableCollection<string> Months
        {
            get { return months; }
            set
            {
                months = value;
                OnPropertyChanged(nameof(Months));
            }
        }

        private ObservableCollection<int> reservationCount;
        public ObservableCollection<int> ReservationCount
        {
            get { return reservationCount; }
            set
            {
                reservationCount = value;
                OnPropertyChanged(nameof(ReservationCount));
            }
        }

        public GuestProfileViewModel(GuestWindow parentWindow, User user, GuestProfilePage page)
        {
            this.page = page;
            this.parentWindow = parentWindow;
            this.user = user;

            guestService = new GuestService();
            userService = new UserService();
            accommodationReservationService = new AccommodationReservationService();

            Guest guest = userService.GetGuestFromUser(user);
            Guest = new GuestDTO(guest);
            guestService.CheckSuperGuestStatus(guest.Id);

            LoadPieChart();
            LoadCartesianChart();
        }
        public void LoadPieChart()
        {
            var data = new ObservableCollection<PieSuperGuest>
        {
            new PieSuperGuest { Title = "Lef discounts", Value = Guest.Points },
            new PieSuperGuest { Title = "Used discounts", Value = 5     - Guest.Points }
        };

            PieSeriesCollection = new SeriesCollection();

            foreach (var item in data)
            {
                PieSeriesCollection.Add(new PieSeries
                {
                    Title = item.Title,
                    Values = new ChartValues<double> { item.Value },
                    DataLabels = true
                });
            }
        }
        public void LoadCartesianChart()
        {

            Dictionary<int, int> monthReservationlMap = accommodationReservationService.ReservationsByMonths(userService.GetGuestIdFromUser(user));

            var travelData = new List<CartesianMonthlyReservations>();

            for (int i = 11; i >= 0; i--)
            {
                travelData.Add(new CartesianMonthlyReservations { Month = DateTime.Now.AddMonths(-i).Month, ReservationCount = monthReservationlMap[DateTime.Now.AddMonths(-i).Month] });
            }

            /*
               var travelData = new List<CartesianMonthlyReservations>
               {
               new CartesianMonthlyReservations { Month = 1, ReservationCount = 3 },
                   new CartesianMonthlyReservations { Month = 2, ReservationCount = 5 },
                   new CartesianMonthlyReservations { Month = 3, ReservationCount = 5 },
                   new CartesianMonthlyReservations { Month = 4, ReservationCount = 3 },
                   new CartesianMonthlyReservations { Month = 5, ReservationCount = 5 },
                   new CartesianMonthlyReservations { Month = 6, ReservationCount = 6 },
                   new CartesianMonthlyReservations { Month = 7, ReservationCount = 3 },
                   new CartesianMonthlyReservations { Month = 8, ReservationCount = 2 },
                   new CartesianMonthlyReservations { Month = 9, ReservationCount = 1 },
                   new CartesianMonthlyReservations { Month = 10, ReservationCount = 7 },
                   new CartesianMonthlyReservations { Month = 11, ReservationCount = 5 },
                   new CartesianMonthlyReservations { Month = 12, ReservationCount = 2 }
        };
                               */

            Months = new ObservableCollection<string>();

            for (int i = 11; i >= 0; i--)
            {
                Months.Add(DateTime.Now.AddMonths(-i).ToString("MMMM"));
            }
            /*
            {
                "Januar", "Februar", "Mart", "April", "Maj", "Jun", "Jul", "Avgust", "Septembar", "Oktobar", "Novembar", "Decembar"
            };
            */

            ReservationCount = new ObservableCollection<int>(travelData.Select(x => x.ReservationCount));
        
            CartesianSeriesCollection = new SeriesCollection
        {
            new LineSeries
            {
                Title = "Putovanja",
                Values = new ChartValues<int>(travelData.Select(x => x.ReservationCount))
            }
        };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
