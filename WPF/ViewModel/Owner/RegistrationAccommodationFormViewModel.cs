using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Utilities;
using BookingApp.WPF.View.Owner;
using BookingApp.Repository;
using BookingApp.DTO;
using System.Windows.Navigation;
using System.Windows;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Application.UseCases;
using BookingApp.Application.Injector;
namespace BookingApp.WPF.ViewModel.Owner
{
    public class RegistrationAccommodationFormViewModel
    {
        public List<string> Types { get; set; } = new List<string> { "House", "Apartment", "Cottage" };
        private readonly OwnerWindow parentWindow;
        public RegistrationAccommodationForm page;
        public LocationService locationService;
        public AccommodationsService accommodationsService;
        public ImageService imageService;
        public AccommodationDTO Accommodation { get; set; }
        public Location Location { get; set; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand RequestCommand { get; }
        public RelayCommand RenovatingCommand { get; }
        public RelayCommand ReviewCommand { get; }
        public RelayCommand OwnerProfileCommand { get; }
        public RelayCommand HomePageCommand { get; }
        public RelayCommand AccommodationsCommand { get; }
        public RelayCommand RateGuestCommand { get; }
        public RelayCommand RegistrationCommand { get; }
        public RelayCommand ForumCommand { get; }
        public RelayCommand AdvicesCommand { get; }
        public RegistrationAccommodationFormViewModel(RegistrationAccommodationForm page, OwnerWindow parentWindow)
        {
            this.page = page;
            this.parentWindow = parentWindow;
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            accommodationsService = new AccommodationsService();
            imageService = new ImageService(Injector.CreateInstance<IImageRepository>());
            Accommodation = new AccommodationDTO();
            Location = new Location();
            CancelCommand = new RelayCommand(CancelExecute);
            RequestCommand = new RelayCommand(RequestExecute);
            RenovatingCommand = new RelayCommand(RenovatingExecute);
            ReviewCommand = new RelayCommand(ReviewExecute);
            OwnerProfileCommand = new RelayCommand(OwnerProfileExecute);
            HomePageCommand = new RelayCommand(HomePageExecute);
            AccommodationsCommand = new RelayCommand(AccommodationsExecute);
            RateGuestCommand = new RelayCommand(RateGuestExecute);
            RegistrationCommand = new RelayCommand(RegistrationExecute,CanExecute);
            ForumCommand = new RelayCommand(ForumExecute);
            AdvicesCommand = new RelayCommand(AdvicesExecute);
        }
        private bool CanExecute()
        {
            return Accommodation.IsValid&&Location.IsValid;
        }
        private void ResetTextBoxes()
        {
            page.NameTextBox.Text = "";
            page.CityTextBox.Text = "";
            page.StateTextBox.Text = "";
            page.CancelationPeriodTextBox.Text = "0";
            page.MinStayDaysTextBox.Text = "0";
            page.CapacityTextBox.Text = "0";
            page.TypesComboBox.SelectedIndex = 1;
            page.PictureUrlTextBox.Text = "";
        }
        public void RegistrationExecute()
        {
                string city = page.CityTextBox.Text;
                string state = page.StateTextBox.Text;
                string path = page.PictureUrlTextBox.Text;
                Location location = new Location(state, city);
                Location.State = state;
                Location.City = city;
                int locationId = locationService.SavedLocationId(location);
                Accommodation.LocationId = locationId;
                Accommodation.IsClosed = false;
                int accommodationId = accommodationsService.SavedAccommodationId(Accommodation.ToAccommodation());
                Image image = new Image(path, accommodationId, EntityType.a);
                imageService.Save(image);
                ResetTextBoxes();
                page.Success.Visibility = Visibility.Visible; 
        }
        public void CancelExecute()
        {
            parentWindow.Show();
            parentWindow.Activate();
            page.NavigationService.Navigate(null);
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
        public void OwnerProfileExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new OwnerProfilePage(parentWindow));
        }
        public void HomePageExecute()
        {
            parentWindow.Show();
            parentWindow.Activate();
            page.NavigationService.Navigate(null);
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
