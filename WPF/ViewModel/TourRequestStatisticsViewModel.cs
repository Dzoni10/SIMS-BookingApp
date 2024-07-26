using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApp.WPF.View;
using BookingApp.Utilities;
using iText.Signatures;
using BookingApp.Application.Injector;
using BookingApp.Domain.RepositoryInterfaces;
using System.Windows;
using Microsoft.Win32;
using System.Diagnostics;

namespace BookingApp.WPF.ViewModel
{
    public class TourRequestStatisticsViewModel : INotifyPropertyChanged
    {
        public List<string> SearchCategory { get; set; }
        private string selectedCategory;
        public string SelectedCategory
        {
            get { return selectedCategory; }
            set 
            {
                if (selectedCategory != value)
                {
                    selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                    SetVisibility();
                    UpdateRequestStatistics();
                }
            }
        }

        public List<int> Years { get; set; }
        private int selectedYear;
        public int SelectedYear
        {
            get { return selectedYear; }
            set
            {
                if(value != selectedYear)
                {
                    selectedYear = value;
                    OnPropertyChanged(nameof(SelectedYear));
                    UpdateRequestStatistics();
                }
            }
        }
        private string language;
        public string Language 
        {
            get { return language; }
            set
            {
                if(value != language)
                {
                    language = value;
                    OnPropertyChanged(nameof(Language));
                    UpdateRequestStatistics();
                }
            } 
        }

        private string selectedState;
        public string SelectedState
        {
            get { return selectedState; }
            set
            {
                if (selectedState != value)
                {
                    selectedState = value;
                    OnPropertyChanged(nameof(SelectedState));
                    Cities = searchTourRequestService.SetCities(SelectedState);
                }
            }
        }
        private string selectedCity;
        public string SelectedCity
        {
            get { return selectedCity; }
            set
            {
                if (selectedCity != value)
                {
                    selectedCity = value;
                    OnPropertyChanged(nameof(SelectedCity));
                    UpdateRequestStatistics();
                }
            }
        }

        public List<string> States { get; set; }
        private List<string> cities;
        public List<string> Cities
        {
            get { return cities; }
            set
            {
                cities = value;
                OnPropertyChanged(nameof(Cities));
            }
        }
        private System.Windows.Visibility languageVisisbility;
        public System.Windows.Visibility LanguageVisisbility
        {
            get { return languageVisisbility; }
            set
            {
                languageVisisbility = value;
                OnPropertyChanged(nameof(LanguageVisisbility));
            }
        }
        private System.Windows.Visibility locationVisisbility;
        public System.Windows.Visibility LocationVisisbility
        {
            get { return locationVisisbility; }
            set
            {
                locationVisisbility = value;
                OnPropertyChanged(nameof(LocationVisisbility));
            }
        }
        public ObservableCollection<TourRequestStatisticsDTO> RequestStatistics {  get; set; }
        private TourRequestService tourRequestService;
        private SearchTourRequestService searchTourRequestService;
        public RelayCommand OKCommand { get; }
        public RelayCommand PDFCommand { get; }

        public TourRequestStatisticsViewModel()
        {
            OKCommand = new RelayCommand(OK);
            PDFCommand = new RelayCommand(GeneratePDF);
            tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>()));
            searchTourRequestService = new SearchTourRequestService(new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>())),
                new LocationService(Injector.CreateInstance<ILocationRepository>()),
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()),
                new TourService(Injector.CreateInstance<ITourRepository>(), 
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), 
                new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>()))),
                new NotificationService(Injector.CreateInstance<INotificationRepository>()));
            RequestStatistics = new ObservableCollection<TourRequestStatisticsDTO>();
            SearchCategory = new List<string>{ "Location", "Language" };
            SelectedCategory = "Location";              
            Years = tourRequestService.GetAllRequestYears();
            Years.Add(0);
            selectedYear = 0;
            States = searchTourRequestService.SetStates();
            SetVisibility();
            UpdateRequestStatistics();
        }
        private void SetVisibility()
        {
            if (SelectedCategory == "Language")
            {
                LocationVisisbility = System.Windows.Visibility.Collapsed;
                LanguageVisisbility = System.Windows.Visibility.Visible;
                Language = null;
                SelectedState = null;
                SelectedCity = null;
            }
            else
            {
                LocationVisisbility = System.Windows.Visibility.Visible;
                LanguageVisisbility = System.Windows.Visibility.Collapsed;
                Language = null;
                SelectedState = null;
                SelectedCity = null;
            }

        }

        private void UpdateRequestStatistics()
        {
            if(selectedYear == 0)
            {
                UpdateByYear(GetRequestByCategory());
            }
            else
            {
                UpdateByMonthOfYear(GetRequestByCategory());
            }
        }

        public List<TourRequest> GetRequestByCategory()
        {
            if (selectedCity != null)
            {
                return tourRequestService.GetAllRequestByLocation(SelectedState, SelectedCity);
            }
            else if (Language != null && Language != "")
            {
                return tourRequestService.GetAllRequestByLanguage(Language);
            }
            else
            {
                return tourRequestService.GetAll();
            }
        }

        public void UpdateByYear(List<TourRequest> requests)
        {
            RequestStatistics.Clear();
            foreach(var statistic in tourRequestService.GetRequestByYears(requests))
            {
                RequestStatistics.Add(new TourRequestStatisticsDTO(statistic.Item1, statistic.Item2));
            }
        }

        public void UpdateByMonthOfYear(List<TourRequest> requests)
        {
            RequestStatistics.Clear();
            foreach (var statistic in tourRequestService.GetRequestByMonth(requests, SelectedYear))
            {
                RequestStatistics.Add(new TourRequestStatisticsDTO(statistic.Item1, statistic.Item2));
            }
        }
        private void OK()
        {
            System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive).Close();
        }
        private void GeneratePDF()
        {
            // Kreiramo i konfiguriramo SaveFileDialog
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                DefaultExt = "pdf",
                Title = "Save PDF"
            };

            // Prikazujemo SaveFileDialog korisniku
            bool? result = saveFileDialog.ShowDialog();

            // Ako je korisnik pritisnuo "Save" dugme
            if (result == true)
            {
                string filePath = saveFileDialog.FileName;

                // Generišemo PDF i čuvamo ga na odabranoj putanji
                tourRequestService.GeneratePDF(filePath);
                MessageBox.Show("PDF saved: " + filePath);
                OpenPdf(filePath);
            }
            
        }

        private void OpenPdf(string filePath)
        {
            try
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nije moguće otvoriti PDF fajl: " + ex.Message);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
