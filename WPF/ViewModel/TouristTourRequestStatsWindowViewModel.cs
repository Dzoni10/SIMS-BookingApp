using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel
{
    public class TouristTourRequestStatsWindowViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<string> _labelsLanguages;
        private ObservableCollection<string> _labelsLocations;
        private SeriesCollection _chartSeriesLanguages;
        private SeriesCollection _chartSeriesLocations;

        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsChecked { get; set; }
        private int year { get; set; }

        public int Year
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged("Year");

            }
        }
        public RelayCommand CheckCommand { get; set; }
        public RelayCommand ChangeStatsCommand { get; set; }

        public ObservableCollection<string> LabelsLanguages
        {
            get { return _labelsLanguages; }
            set
            {
                _labelsLanguages = value;
                OnPropertyChanged("LabelsLanguages");
            }
        }

        public ObservableCollection<string> LabelsLocations
        {
            get { return _labelsLocations; }
            set
            {
                _labelsLocations = value;
                OnPropertyChanged("LabelsLocations");
            }
        }
        public SeriesCollection ChartSeriesLanguages
        {
            get { return _chartSeriesLanguages; }
            set
            {
                _chartSeriesLanguages = value;
                OnPropertyChanged("ChartSeriesLanguages");
            }
        }

        public SeriesCollection ChartSeriesLocations
        {
            get { return _chartSeriesLocations; }
            set
            {
                _chartSeriesLocations = value;
                OnPropertyChanged("ChartSeriesLocations");
            }
        }

        public TouristTourRequestStatsWindow parentWindow { get; set; }

        private TourRequestService TourRequestService;

        public User LoggedInUser { get; set; }
        public RelayCommand TutorialCommand { get; set; }

        public TouristTourRequestStatsWindowViewModel(TouristTourRequestStatsWindow window,User user)
        {
            parentWindow = window;
            LoggedInUser = user;
            TourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>()));
            IsChecked = false;
            Year = 2024;
            parentWindow.AcceptedLabel.Content = $"{TourRequestService.GetPercentageOfAcceptedRequestsByYear(Year)}%";
            parentWindow.DeclinedLabel.Content = $"{TourRequestService.GetPercentageOfDeclinedOrPendingRequestsByYear(Year)}%";
            parentWindow.AverageAttendanceLabel.Content = $"{TourRequestService.GetAverageAttendanceOnAcceptedRequestsByYear(Year)}";
            CreateLanguageStat();
            CreateLocationStat();
            CheckCommand = new RelayCommand(CheckClick);
            ChangeStatsCommand = new RelayCommand(ChangeStatsClick);
            TutorialCommand = new RelayCommand(TutorialClick);

        }
        private void CreateLanguageStat()
        {
            var chartSeries = new SeriesCollection();
            var labels = new ObservableCollection<string>();

            foreach (var item in TourRequestService.GetStatsForLanguages())
            {
                var columnSeries = new ColumnSeries
                {
                    Title = item.Item1.Name,
                    Values = new ChartValues<int> { item.Item2 },
                    DataLabels = true
                };
                chartSeries.Add(columnSeries);
                labels.Add("");
            }
            ChartSeriesLanguages = chartSeries;
            LabelsLanguages = labels;
        }

        private void CreateLocationStat()
        {
            var chartSeries = new SeriesCollection();
            var labels = new ObservableCollection<string>();

            foreach (var item in TourRequestService.GetStatsForLocations())
            {
                var columnSeries = new ColumnSeries
                {
                    Title = item.Item1.City + ", " + item.Item1.State,
                    Values = new ChartValues<int> { item.Item2 },
                    DataLabels = true
                };
                chartSeries.Add(columnSeries);
                labels.Add("");
            }
            ChartSeriesLocations = chartSeries;
            LabelsLocations = labels;
        }

        private void ChangeStatsClick()
        {
            if (IsChecked)
            {
                parentWindow.AcceptedLabel.Content = $"{TourRequestService.GetPercentageOfAcceptedRequestsAllTime()}%";
                parentWindow.DeclinedLabel.Content = $"{TourRequestService.GetPercentageOfDeclinedOrPendingRequestsAllTime()}%";
                parentWindow.AverageAttendanceLabel.Content = $"{TourRequestService.GetAverageAttendanceOnAcceptedRequestsAllTime()}";
            }
            else
            {
                parentWindow.AcceptedLabel.Content = $"{TourRequestService.GetPercentageOfAcceptedRequestsByYear(Year)}%";
                parentWindow.DeclinedLabel.Content = $"{TourRequestService.GetPercentageOfDeclinedOrPendingRequestsByYear(Year)}%";
                parentWindow.AverageAttendanceLabel.Content = $"{TourRequestService.GetAverageAttendanceOnAcceptedRequestsByYear(Year)}";
            }
        }

        private void CheckClick()
        {
            ChangeBool();
            if (IsChecked)
            {
                parentWindow.YearTextBox.IsReadOnly = true;
                parentWindow.AcceptedLabel.Content = $"{TourRequestService.GetPercentageOfAcceptedRequestsAllTime()}%";
                parentWindow.DeclinedLabel.Content = $"{TourRequestService.GetPercentageOfDeclinedOrPendingRequestsAllTime()}%";
                parentWindow.AverageAttendanceLabel.Content = $"{TourRequestService.GetAverageAttendanceOnAcceptedRequestsAllTime()}";
            }
            else
            {
                parentWindow.YearTextBox.IsReadOnly = false;
                parentWindow.AcceptedLabel.Content = $"{TourRequestService.GetPercentageOfAcceptedRequestsByYear(Year)}%";
                parentWindow.DeclinedLabel.Content = $"{TourRequestService.GetPercentageOfDeclinedOrPendingRequestsByYear(Year)}%";
                parentWindow.AverageAttendanceLabel.Content = $"{TourRequestService.GetAverageAttendanceOnAcceptedRequestsByYear(Year)}";
            }
        }

        private void ChangeBool()
        {
            if (IsChecked == false)
                IsChecked = true;
            else
                IsChecked = false;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TutorialClick() 
        {
            NavigationService navigationService = NavigationService.GetNavigationService(parentWindow);
            navigationService.Navigate(new TutorialStatistics("C:\\Users\\nikol\\Desktop\\Nikola\\Random slike i videi xd\\Random slike i videi xd\\Tutorijali\\StatisticsTutorial.mkv"));
        }
    }


}
