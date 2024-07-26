using BookingApp.WPF.View.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Utilities;
using System.Windows.Data;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using System.Collections.ObjectModel;
using BookingApp.DTO;
using BookingApp.Domain.Models;
using System.Security.Cryptography.X509Certificates;
using BookingApp.Application.Injector;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class AdviceViewModel
    {
        public readonly double coeficient = 0.53;
        public readonly double maxBusy = -1.0;
        private readonly OwnerWindow parentWindow;
        public AdvicePage page;
        public AccommodationReservationService accommodationReservationService;
        public LocationService locationService;
        public ObservableCollection<LocationDTO> AdviceClosingLocations { get; set; }
        public ObservableCollection<LocationDTO> AdviceOpeningLocations { get; set; }

        public RelayCommand BackCommand { get; }
        public RelayCommand RequestCommand { get; }
        public RelayCommand OwnerProfileCommand { get; }
        public RelayCommand RenovatingCommand { get; }
        public RelayCommand ReviewCommand { get; }
        public RelayCommand AccommodationsCommand { get; }
        public RelayCommand RateGuestCommand { get; }
        public RelayCommand RegistrationCommand { get; }
        public RelayCommand ForumCommand { get; }
        public RelayCommand HomePageCommand { get;}
        public AdviceViewModel(AdvicePage page, OwnerWindow parentWindow)
        {
            this.parentWindow = parentWindow;
            this.page = page;
            accommodationReservationService = new AccommodationReservationService();
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            AdviceClosingLocations = new ObservableCollection<LocationDTO>();
            AdviceOpeningLocations = new ObservableCollection<LocationDTO>();
            BackCommand = new RelayCommand(BackExecute);
            RequestCommand = new RelayCommand(RequestExecute);
            OwnerProfileCommand = new RelayCommand(OwnerProfileExecute);
            RenovatingCommand = new RelayCommand(RenovatingExecute);
            ReviewCommand = new RelayCommand(ReviewExecute);
            AccommodationsCommand = new RelayCommand(AccommodationsExecute);
            RateGuestCommand = new RelayCommand(RateGuestExecute);
            RegistrationCommand = new RelayCommand(RegistrationExecute);
            ForumCommand = new RelayCommand(ForumExecute);
            HomePageCommand = new RelayCommand(HomePageExecute);
            FillAdviceAccommodations();
        }

        public void FillAdviceAccommodations()
        {
            var locationsClosing = accommodationReservationService.GetClosingAccommodation();
            AdviceClosingLocations.Clear();
            AdviceOpeningLocations.Clear();
            foreach (var location in locationsClosing)
            {
                Location givenLocation = locationService.GetById(location.Key);

                    
                if((location.Value >coeficient || location.Value==maxBusy)&&location.Value!=0)
                {
                    AdviceOpeningLocations.Add(new LocationDTO(givenLocation.City, givenLocation.State));
                }
                else
                {
                    AdviceClosingLocations.Add(new LocationDTO(givenLocation.City, givenLocation.State));
                }  
            }
        }

        public void BackExecute()
        {
            try
            {
                NavigationService navigationService = NavigationService.GetNavigationService(page);
                navigationService.GoBack();
            }
            catch(InvalidOperationException ex)
            {
                parentWindow.Show();
                parentWindow.Activate();
                page.NavigationService.Navigate(null);
            }
           
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

        public void RateGuestExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RateGuestPage(parentWindow));
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
        public void ForumExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new ForumPage(parentWindow));
        }

    }
}
