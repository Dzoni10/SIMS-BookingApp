using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Utilities;
using BookingApp.Repository;
using BookingApp.DTO;
using BookingApp.Domain.RepositoryInterfaces;
using System.Windows.Navigation;
using BookingApp.WPF.View.Owner;
using BookingApp.Domain.Models;
using System.Collections.ObjectModel;
using BookingApp.Application.UseCases;
using BookingApp.Application.Injector;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class RatingGuestViewModel
    {
        private readonly OwnerWindow parentWindow;
        public List<double> Grades { get; set; } = new List<double> { 1, 2, 3, 4, 5 };
        public RatnigGuestPage page;
        public GuestAccommodationRateDTO GuestRate { get; set; }
        public AccommodationReservationDTO SelectedAccommodationReservation;
        public RateGuestService rateGuestService;
        ObservableCollection<AccommodationReservationDTO> AccommodationReservations { get; set; }
        public RelayCommand RateGuestCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand RequestCommand { get; }
        public RelayCommand OwnerProfileCommand { get; }
        public RelayCommand RenovatingCommand { get; }
        public RelayCommand ReviewCommand { get; }
        public RelayCommand AccommodationsCommand { get; }
        public RelayCommand RateGuestPageCommand { get; }
        public RelayCommand RegistrationCommand { get; }
        public RelayCommand ForumCommand { get; }
        public RelayCommand AdvicesCommand { get; }
        public RelayCommand HomePageCommand { get; }
        public RatingGuestViewModel(RatnigGuestPage page,OwnerWindow parentWindow, AccommodationReservationDTO SelectedAccommodationReservation, ObservableCollection<AccommodationReservationDTO> AccommodationReservations)
        {
            this.page = page;
            this.parentWindow = parentWindow;
            this.SelectedAccommodationReservation = SelectedAccommodationReservation;
            page.NameTexBox.Text = SelectedAccommodationReservation.Guest.Name;
            GuestRate = new GuestAccommodationRateDTO();
            GuestRate.AccommodationReservationId = SelectedAccommodationReservation.Id;
            GuestRate.GuestId = SelectedAccommodationReservation.GuestId;
            GuestRate.AccommodationId = SelectedAccommodationReservation.AccommodationId;
            this.AccommodationReservations = AccommodationReservations;
            RateGuestCommand = new RelayCommand(RateGuestExecute,CanRate);
            CancelCommand = new RelayCommand(CancelExecute);
            RequestCommand = new RelayCommand(RequestExecute);
            OwnerProfileCommand = new RelayCommand(OwnerProfileExecute);
            RenovatingCommand = new RelayCommand(RenovatingExecute);
            ReviewCommand = new RelayCommand(ReviewExecute);
            RateGuestPageCommand = new RelayCommand(RateGuestPageExecute);
            AccommodationsCommand = new RelayCommand(AccommodationsExecute);
            RegistrationCommand = new RelayCommand(RegistrationExecute);
            ForumCommand = new RelayCommand(ForumExecute);
            AdvicesCommand = new RelayCommand(AdvicesExecute);
            HomePageCommand = new RelayCommand(HomePageExecute);
            rateGuestService = new RateGuestService();
        }
        private bool CanRate()
        {
            return GuestRate.IsValid;
        }
        public void RateGuestExecute()
        {
            rateGuestService.SaveGuestAccommodationRate(GuestRate.ToGuestAccommodationRate());
            SelectedAccommodationReservation.GuestRateStatus = RateStatus.Rated;
            rateGuestService.UpdateReservation(SelectedAccommodationReservation.ToAccommodationReservation());
            AccommodationReservations.Remove(SelectedAccommodationReservation);
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.GoBack();
        }
        public void CancelExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.GoBack();
        }

        public void RequestExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RequestPage(parentWindow));
        }
        public void OwnerProfileExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new OwnerProfilePage(parentWindow));
        }
        public void RenovatingExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RenovatingPage(parentWindow));
        }

        public void ReviewExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new ReviewPage(parentWindow));
        }

        public void AccommodationsExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new AccommodationsPage(parentWindow));
        }

        public void RateGuestPageExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RateGuestPage(parentWindow));
        }

        public void RegistrationExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RegistrationAccommodationForm(parentWindow));
        }
        public void ForumExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new ForumPage(parentWindow));
        }

        public void AdvicesExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new AdvicePage(parentWindow));
        }
        public void HomePageExecute()
        {
            parentWindow.Show();
            parentWindow.Activate();
            page.NavigationService.Navigate(null);
        }
    }
}
