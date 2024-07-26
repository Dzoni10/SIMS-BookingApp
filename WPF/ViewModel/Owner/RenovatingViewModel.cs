using System.Windows.Navigation;
using BookingApp.Utilities;
using BookingApp.WPF.View.Owner;
using BookingApp.DTO;
using BookingApp.Application.UseCases;
using System.Collections.ObjectModel;
using BookingApp.Domain.Models;
using BookingApp.Application.Injector;
using BookingApp.Domain.RepositoryInterfaces;
using System.ComponentModel;
using System;
namespace BookingApp.WPF.ViewModel.Owner
{
    public class RenovatingViewModel
    {
        private readonly int limitPeriod = 5;
        private readonly OwnerWindow parentWindow;
        public RenovatingPage page;
        public AccommodationsService accommodationsService;
        public RenovationService renovationService;
        public ObservableCollection<AccommodationDTO> Accommodations { get; set; }
        public ObservableCollection<RenovationDTO> Renovations { get; set; }
        public AccommodationDTO SelectedAccommodation { get; set; }
        public RenovationDTO SelectedRenovation { get; set; }
        public RelayCommand BackCommand { get; }
        public RelayCommand RequestCommand { get; }
        public RelayCommand ReviewCommand { get; }
        public RelayCommand RegistrationCommand { get; }
        public RelayCommand HomePageCommand { get; }
        public RelayCommand OwnerProfileCommand { get; }
        public RelayCommand AccommodationsCommand { get; }
        public RelayCommand RateGuestCommand { get; }
        public RelayCommand AddRenovationCommand { get; }
        public RelayCommand CancelRenovationCommand { get; }
        public RelayCommand AdvicesCommand { get; }
        public RelayCommand ForumCommand { get; }
        public RenovatingViewModel(RenovatingPage page, OwnerWindow parentWindow)
        {
            this.parentWindow = parentWindow;
            this.page = page;
            accommodationsService = new AccommodationsService();
            renovationService = new RenovationService();
            Accommodations = new ObservableCollection<AccommodationDTO>();
            Renovations = new ObservableCollection<RenovationDTO>();
            BackCommand = new RelayCommand(BackExecute);
            RequestCommand = new RelayCommand(RequestExecute);
            ReviewCommand = new RelayCommand(ReviewExecute);
            RegistrationCommand = new RelayCommand(RegistrationExecute);
            HomePageCommand = new RelayCommand(HomePageExecute);
            OwnerProfileCommand = new RelayCommand(OwnerProfileExceute);
            AccommodationsCommand = new RelayCommand(AccommodationsExecute);
            RateGuestCommand = new RelayCommand(RateGuestExecute);
            AddRenovationCommand = new RelayCommand(AddRenovationExecute, CanRenovate);
            CancelRenovationCommand = new RelayCommand(CancelRenovationExecute, CanCancel);
            AdvicesCommand = new RelayCommand(AdvicesExecute);
            ForumCommand = new RelayCommand(ForumExecute);
            FillAccommodations();
            FillRenovations();
        }

        private bool CanRenovate()
        {
            return SelectedAccommodation != null;
        }

        private bool CanCancel()
        {
            return SelectedRenovation != null && (SelectedRenovation.StartDate - DateTime.Now).TotalDays>limitPeriod ;
        }

        public void FillAccommodations()
        {
            Accommodations.Clear();
            Accommodations = new ObservableCollection<AccommodationDTO>(accommodationsService.GetAll());
        }

        public void FillRenovations()
        {
            var renovations = renovationService.GetAllRenovations();
            Renovations.Clear();
            foreach (RenovationDTO renovation in renovations)
            {
                if(!accommodationsService.GetById(renovation.AccommodationId).IsClosed)
                {
                    Renovations.Add(renovation);
                }
            }
        }
        public void BackExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new AccommodationsPage(parentWindow));
        }
        public void RequestExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RequestPage(parentWindow));
        }
        public void ReviewExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new ReviewPage(parentWindow));
        }
        public void RegistrationExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RegistrationAccommodationForm(parentWindow));
        }
        public void HomePageExecute()
        {
            parentWindow.Show();
            parentWindow.Activate();
            page.NavigationService.Navigate(null);
        }
        public void OwnerProfileExceute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new OwnerProfilePage(parentWindow));
        }
        public void AccommodationsExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new AccommodationsPage(parentWindow));
        }

        public void RateGuestExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RateGuestPage(parentWindow));
        }
        public void AddRenovationExecute()
        {

            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RenovatePage(parentWindow, SelectedAccommodation));

        }
        public void CancelRenovationExecute()
        {

            renovationService.CancelRenovation(SelectedRenovation);
            Renovations.Remove(SelectedRenovation);
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
    }
}
