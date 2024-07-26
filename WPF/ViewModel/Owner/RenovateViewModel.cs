using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Utilities;
using BookingApp.WPF.View.Owner;
using BookingApp.DTO;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Application.Injector;
using BookingApp.Domain.RepositoryInterfaces;
using System.Windows.Input;
using Org.BouncyCastle.Asn1.Mozilla;
namespace BookingApp.WPF.ViewModel.Owner
{
    class RenovateViewModel
    {
        public RenovatePage page;
        public readonly OwnerWindow parentWindow;
        public RenovationService renovationService;
        public AccommodationDTO SelectedAccommodation;
        public RenovationDTO Renovation { get; set; }
        DateTime selectedStartDate;
        DateTime selectedEndDate;
        public RelayCommand BackCommand { get; }
        public RelayCommand SearchCommand { get; }
        public RelayCommand RenovateCommand { get; }
        public ObservableCollection<DateRangeDTO> AvailableDates { get; set; }
        public RenovateViewModel(RenovatePage page, OwnerWindow parentWindow, AccommodationDTO SelectedAccommodation)
        {
            this.page = page;
            this.parentWindow = parentWindow;
            Renovation = new RenovationDTO();
            Renovation.StartDate = DateTime.Today;
            Renovation.EndDate = DateTime.Today;
            this.SelectedAccommodation = SelectedAccommodation;
            page.AccommodationNameTextBox.Text = SelectedAccommodation.Name;
            AvailableDates = new ObservableCollection<DateRangeDTO>();
            renovationService = new RenovationService();
            BackCommand = new RelayCommand(BackExecute);
            SearchCommand = new RelayCommand(SearchExecute, CanSearch);
            RenovateCommand = new RelayCommand(RenovateExecute, CanExecute);
        }
        private bool CanExecute()
        {
            return !string.IsNullOrWhiteSpace(page.DurationTextBox.Text) && !string.IsNullOrWhiteSpace(page.DescriptionTextBox.Text) && page.StartDatePicker.SelectedDate < page.EndDatePicker.SelectedDate && Renovation.IsValid;
        }
        private bool CanSearch()
        {
            return page.StartDatePicker.SelectedDate.HasValue && page.EndDatePicker.SelectedDate.HasValue && page.StartDatePicker.SelectedDate < page.EndDatePicker.SelectedDate;
        }
        public void BackExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RenovatingPage(parentWindow));
        }
        public void SearchExecute()
        {
            selectedStartDate = (DateTime)page.StartDatePicker.SelectedDate;
            selectedEndDate = (DateTime)page.EndDatePicker.SelectedDate;
            var availableDates = renovationService.AvailableDates(SelectedAccommodation, selectedStartDate, selectedEndDate);

            AvailableDates.Clear();
            foreach (var date in availableDates)
            {
                DateRangeDTO dateDTO = new DateRangeDTO(date.StartDate, date.EndDate);
                AvailableDates.Add(dateDTO);
            }
        }
        public void RenovateExecute()
        {
            int accommodationId = SelectedAccommodation.Id;
            DateTime startDate = (DateTime)page.StartDatePicker.SelectedDate;
            DateTime endDate = (DateTime)page.EndDatePicker.SelectedDate;
            int duration = int.Parse(page.DurationTextBox.Text);
            string description = page.DescriptionTextBox.Text;
            Renovation renovation = new Renovation(accommodationId, startDate, endDate, duration, description);
            renovationService.SaveRenovation(renovation);
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RenovatingPage(parentWindow));
        }
    }
}
