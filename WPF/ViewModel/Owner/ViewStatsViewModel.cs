using BookingApp.Application.UseCases;
using BookingApp.DTO;
using BookingApp.Utilities;
using BookingApp.WPF.View.Owner;
using PdfSharp.Charting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class ViewStatsViewModel
    {
        public readonly OwnerWindow parentWindow;
        public ViewStatsPage page;
        public AccommodationDTO SelectedAccommodation { get; set; }
        public StatsDTO SelectedStat { get; set; }
        public ObservableCollection<StatsDTO> Stats { get; set; }
        public StatsService statsService;
        public RelayCommand BackCommand { get; }
        public RelayCommand RegistrationCommand { get; }
        public RelayCommand RateGuestCommand { get; }
        public RelayCommand AccommodationsCommand { get; }
        public RelayCommand OwnerProfileCommand { get; }
        public RelayCommand RenovationCommand { get; }
        public RelayCommand ReviewCommand { get; }
        public RelayCommand RequestCommand { get; }
        public RelayCommand AdvicesCommand { get; }
        public RelayCommand HomePageCommand { get; }
        public RelayCommand ForumCommand { get; }
        public RelayCommand MonthStatsCommand { get; }
        public ViewStatsViewModel(ViewStatsPage page, OwnerWindow parentWinow, AccommodationDTO SelectedAccommodation)
        {
            this.page = page;
            parentWindow = parentWinow;
            this.SelectedAccommodation = SelectedAccommodation;
            Stats = new ObservableCollection<StatsDTO>();
            statsService = new StatsService();
            page.AccommodationNameTextBox.Text = SelectedAccommodation.Name;
            BackCommand = new RelayCommand(BackExecute);
            RegistrationCommand = new RelayCommand(RegistrationExecute);
            RateGuestCommand = new RelayCommand(RateGuestExecute);
            AccommodationsCommand = new RelayCommand(AccommodationsExecute);
            OwnerProfileCommand = new RelayCommand(OwnerProfileExecute);
            RenovationCommand = new RelayCommand(RenovationExecute);
            ReviewCommand = new RelayCommand(ReviewExecute);
            RequestCommand = new RelayCommand(RequestExecute);
            AdvicesCommand = new RelayCommand(AdvicesExecute);
            HomePageCommand = new RelayCommand(HomePageExecute);
            ForumCommand = new RelayCommand(ForumExecute);
            MonthStatsCommand = new RelayCommand(MonthStatsExecute);
            page.BusyYearTextBox.Text = MostBusyYear();
            ShowStatistics();
        }

        public void ShowStatistics()
        {
            List<StatsDTO> statistics = new List<StatsDTO>();
            statistics = statsService.GetStatistics(SelectedAccommodation);
            Stats.Clear();
            foreach (StatsDTO stats in statistics)
            {
                Stats.Add(stats);
            }
        }

        public string MostBusyYear()
        {
            Dictionary<int, double> busyness = statsService.GetBusyYears(SelectedAccommodation);
            double maxBusyness = busyness.Values.Max();
            int year = busyness.First(kvp => kvp.Value == maxBusyness).Key;
            return year.ToString();
        }

        public void BackExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new AccommodationsPage(parentWindow));
        }

        public void RegistrationExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RegistrationAccommodationForm(parentWindow));
        }

        public void RateGuestExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RateGuestPage(parentWindow));
        }

        public void AccommodationsExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new AccommodationsPage(parentWindow));
        }

        public void OwnerProfileExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new OwnerProfilePage(parentWindow));
        }

        public void RenovationExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RenovatingPage(parentWindow));
        }

        public void ReviewExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new ReviewPage(parentWindow));
        }
        public void RequestExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RequestPage(parentWindow));
        }
        public void AdvicesExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new AdvicePage(parentWindow));
        }

        public void ForumExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new ForumPage(parentWindow));
        }
        public void HomePageExecute()
        {
            parentWindow.Show();
            parentWindow.Activate();
            page.NavigationService.Navigate(null);
        }

        public void MonthStatsExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new MonthStatisticsPage(parentWindow, SelectedStat, SelectedAccommodation));
        }
    }
}
