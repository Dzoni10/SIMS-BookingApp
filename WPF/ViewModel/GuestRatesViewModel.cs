using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.DTO;
using BookingApp.WPF.View;
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BookingApp.WPF.ViewModel
{
    class GuestRatesViewModel : INotifyPropertyChanged
    {
        public GuestRates page;
        private readonly GuestWindow parentWindow;
        private readonly User user;

        private GuestAccommodationRateService guestAccommodationRateService { get; set; }
        private UserService userService { get; set; }
        private AccommodationsService accommodationService { get; set; }


        public ObservableCollection<GuestAccommodationRateDTO> GuestRates { get; set; }

        private SeriesCollection _cartesianSeriesCollection;
        public SeriesCollection CartesianSeriesCollection
        {
            get { return _cartesianSeriesCollection; }
            set
            {
                _cartesianSeriesCollection = value;
                OnPropertyChanged(nameof(CartesianSeriesCollection));
            }
        }

        private ObservableCollection<string> _ratings;
        public ObservableCollection<string> Ratings
        {
            get { return _ratings; }
            set
            {
                _ratings = value;
                OnPropertyChanged(nameof(Ratings));
            }
        }

        private ObservableCollection<int> _ratingsCount;
        public ObservableCollection<int> RatingsCount
        {
            get { return _ratingsCount; }
            set
            {
                _ratingsCount = value;
                OnPropertyChanged(nameof(RatingsCount));
            }
        }
        private string cleanlinesRate;
        public string CleanlinesRate
        {
            get { return cleanlinesRate; }
            set
            {
                cleanlinesRate = value;
                OnPropertyChanged(nameof(CleanlinesRate));
            }
        }
        private string obeyingTheRulesRate;
        public string ObeyingTheRulesRate
        {
            get { return obeyingTheRulesRate; }
            set
            {
                obeyingTheRulesRate = value;
                OnPropertyChanged(nameof(ObeyingTheRulesRate));
            }
        }
        public GuestRatesViewModel(GuestWindow parentWindow, User user, GuestRates page)
        {
            this.page = page;
            this.parentWindow = parentWindow;
            this.user = user;

            guestAccommodationRateService = new GuestAccommodationRateService();
            userService = new UserService();
            accommodationService = new AccommodationsService();

            GuestRates = new ObservableCollection<GuestAccommodationRateDTO>();

            LoadGuestRates();
            LoadGuestRatesChart();
            LoadAverageRateTitles();
        }

        public void LoadGuestRates()
        {
            var Rates = guestAccommodationRateService.GetFilteredRates(userService.GetGuestIdFromUser(user));
            foreach(GuestAccommodationRate guestRate in Rates)
            {
                GuestAccommodationRateDTO guestAccommodationRateDTO = new GuestAccommodationRateDTO(guestRate);
                Accommodation accommodation = accommodationService.GetById(guestRate.AccommodationId);
                guestAccommodationRateDTO.Image = accommodation.Image;
                guestAccommodationRateDTO.AccommodationName = accommodation.Name;
                guestAccommodationRateDTO.Location = accommodation.Location;
                GuestRates.Add(guestAccommodationRateDTO);
            }
        }
        public void LoadGuestRatesChart()
        {
            Dictionary<int, int> summedRates = guestAccommodationRateService.GetRatesForGuest(userService.GetGuestIdFromUser(user));
            var ratingData = new List<CartesianRatingData>
            {
                new CartesianRatingData { Rating = 1, Count = summedRates[1] },
                new CartesianRatingData { Rating = 2, Count = summedRates[2] },
                new CartesianRatingData { Rating = 3, Count = summedRates[3] },
                new CartesianRatingData { Rating = 4, Count = summedRates[4] },
                new CartesianRatingData { Rating = 5, Count = summedRates[5] }
            };

            Ratings = new ObservableCollection<string> { "1", "2", "3", "4", "5" };
            RatingsCount = new ObservableCollection<int>(ratingData.Select(x => x.Count));

            CartesianSeriesCollection = new SeriesCollection
            {
                new ColumnSeries
            {
                Title = "Ocena 1",
                Values = new ChartValues<int> { ratingData.First(x => x.Rating == 1).Count },
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ADD8E6")) // Svetlo plava
            },
            new ColumnSeries
            {
                Title = "Ocena 2",
                Values = new ChartValues<int> { ratingData.First(x => x.Rating == 2).Count },
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#87CEEB")) // Plava
            },
            new ColumnSeries
            {
                Title = "Ocena 3",
                Values = new ChartValues<int> { ratingData.First(x => x.Rating == 3).Count },
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4682B4")) // Srednje plava
            },
            new ColumnSeries
            {
                Title = "Ocena 4",
                Values = new ChartValues<int> { ratingData.First(x => x.Rating == 4).Count },
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4169E1")) // Kraljevski plava
            },
            new ColumnSeries
            {
                Title = "Ocena 5",
                Values = new ChartValues<int> { ratingData.First(x => x.Rating == 5).Count },
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00008B")) // Tamno plava
            }
            };

        }

        public void LoadAverageRateTitles()
        {
            CleanlinesRate = "Cleanliness: " + guestAccommodationRateService.GetAverageCleanlinessRate(userService.GetGuestIdFromUser(user)).ToString();

            ObeyingTheRulesRate = "Obeying the rules: " + guestAccommodationRateService.GetAverageObeyingTheRulesRate(userService.GetGuestIdFromUser(user)).ToString();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
