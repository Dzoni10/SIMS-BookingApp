using BookingApp.Application.UseCases;
using BookingApp.DTO;
using BookingApp.Utilities;
using BookingApp.WPF.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Xceed.Wpf.Toolkit.Core.Converters;
namespace BookingApp.WPF.ViewModel.Owner
{
    public class MonthStatsViewModel
    {
        public MonthStatisticsPage page;
        public ObservableCollection<StatsDTO> MonthStats { get; set; }
        public StatsDTO SelectedStat { get; set; }
        public AccommodationDTO SelectedAccommodation { get; set; }
        public MonthlyStatsService monthlyStatsService;
        public RelayCommand BackCommand { get; }
        public MonthStatsViewModel(MonthStatisticsPage page, StatsDTO SelectedStat, AccommodationDTO SelectedAccommodation)
        {
            this.page = page;
            this.SelectedStat = SelectedStat;
            this.SelectedAccommodation = SelectedAccommodation;
            monthlyStatsService = new MonthlyStatsService();
            MonthStats = new ObservableCollection<StatsDTO>();
            BackCommand = new RelayCommand(BackExecute, CanExecute);
            ShowMonthStatistics();
        }
        public void ShowMonthStatistics()
        {
            List<StatsDTO> statistics = monthlyStatsService.GetMonthStats(SelectedStat.Year, SelectedAccommodation.Id);
            MonthStats.Clear();
            foreach (StatsDTO stats in statistics)
            {
                MonthStats.Add(stats);
            }
        }
        private bool CanExecute()
        {
            return true;
        }
        public void BackExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.GoBack();
        }
    }
}
