using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.Models;
using BookingApp.Repository;
using BookingApp.Utilities;
using BookingApp.DTO;
using System.Collections.ObjectModel;
using BookingApp.WPF.View.Owner;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using System.Windows.Controls;
using BookingApp.Application.Injector;
using BookingApp.Domain.RepositoryInterfaces;
namespace BookingApp.WPF.ViewModel.Owner
{
    public class RateGuestViewModel
    {
        public readonly int limitedPeriod = 5;
        private readonly OwnerWindow parentWindow;
        public RateGuestPage page;
        public RateGuestService rateGuestService;
        public AccommodationsService accommodationsService;
        public AccommodationReservationDTO SelectedAccommodationReservation { get; set; }
        public ObservableCollection<AccommodationReservationDTO> AccommodationReservations { get; set; }
        public RelayCommand BackCommand { get; }
        public RelayCommand HomePageCommand { get; }
        public RelayCommand RegistrationCommand { get; }
        public RelayCommand OwnerProfileCommand { get; }
        public RelayCommand RequestCommand { get; }
        public RelayCommand RenovatingCommand { get; }
        public RelayCommand ReviewCommand { get; }
        public RelayCommand AccommodationsCommand { get; }
        public RelayCommand RateGuestCommand { get; }
        public RelayCommand AdvicesCommand { get; }
        public RelayCommand ForumCommand { get; }
        public RateGuestViewModel(RateGuestPage page, OwnerWindow parentWindow)
        {
            this.parentWindow = parentWindow;
            this.page = page;
            AccommodationReservations = new ObservableCollection<AccommodationReservationDTO>();
            rateGuestService = new RateGuestService();
            accommodationsService = new AccommodationsService();
            BackCommand = new RelayCommand(BackExecute);
            HomePageCommand = new RelayCommand(HomePageExecute);
            RegistrationCommand = new RelayCommand(RegistrationExecute);
            OwnerProfileCommand = new RelayCommand(OwnerProfileExecute);
            RequestCommand = new RelayCommand(RequestExecute);
            RenovatingCommand = new RelayCommand(RenovatingExecute);
            ReviewCommand = new RelayCommand(ReviewExecute);
            AccommodationsCommand = new RelayCommand(AccommodationsExecute);
            RateGuestCommand = new RelayCommand(RateGuestExecute, CanExecute);
            AdvicesCommand = new RelayCommand(AdvicesExecute);
            ForumCommand = new RelayCommand(ForumExecute);
            ShowAttention();
            ShowReservations();
        }
        public void ShowReservations()
        {
            rateGuestService.EliminateUnratedGuest();
            var accommodationReservations = rateGuestService.FillReservations();
            AccommodationReservations.Clear();
            foreach (AccommodationReservationDTO reservation in accommodationReservations)
            {
                if (!accommodationsService.GetById(reservation.AccommodationId).IsClosed)
                { AccommodationReservations.Add(reservation); }  
            } 
        }
        public void ShowAttention()
        {
            if (rateGuestService.AllowAttentionShow())
            {
                if (AccommodationReservations.Count == 0)
                    page.AttentionLabel.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private bool CanExecute()
        {
            return SelectedAccommodationReservation != null;
        }
        public void BackExecute()
        {
            parentWindow.Show();
            parentWindow.Activate();
            page.NavigationService.Navigate(null);
        }
        public void HomePageExecute()
        {
            parentWindow.Show();
            parentWindow.Activate();
            page.NavigationService.Navigate(null);
        }
        public void RegistrationExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RegistrationAccommodationForm(parentWindow));
        }
        public void OwnerProfileExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new OwnerProfilePage(parentWindow));
        }
        public void RequestExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RequestPage(parentWindow));
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
        public void RateGuestExecute()
        {
            if (DateTime.Now.Month - SelectedAccommodationReservation.EndDate.Month <= 1 && DateTime.Now.Year - SelectedAccommodationReservation.EndDate.Year <= 1)
            {
                if (DateTime.Now.Day - SelectedAccommodationReservation.EndDate.Day < limitedPeriod)
                {
                    NavigationService navigationService = NavigationService.GetNavigationService(page);
                    navigationService.Navigate(new RatnigGuestPage(SelectedAccommodationReservation, AccommodationReservations, parentWindow));
                }
            }
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

    }
}
