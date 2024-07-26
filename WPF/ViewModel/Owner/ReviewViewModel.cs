using BookingApp.DTO;
using BookingApp.Utilities;
using BookingApp.Repository;
using BookingApp.WPF.View.Owner;
using System.Windows.Navigation;
using BookingApp.Domain.Models;
using System.Collections.ObjectModel;
using BookingApp.Application.UseCases;
using BookingApp.Application.Injector;
using BookingApp.Domain.RepositoryInterfaces;
namespace BookingApp.WPF.ViewModel.Owner
{
    public class ReviewViewModel
    {
        private readonly OwnerWindow parentWindow;
        public ReviewPage page;
        public ReviewService reviewService;
        public ObservableCollection<OwnerAccommodationRateDTO> OwnerAccommodationRates { get; set; }
        public RelayCommand BackCommand { get; }
        public RelayCommand RequestCommand { get; }
        public RelayCommand RenovatingCommand { get; }
        public RelayCommand RegistrationCommand { get; }
        public RelayCommand HomePageCommand { get; }
        public RelayCommand OwnerProfileCommand { get; }
        public RelayCommand AccommodationsCommand { get; }
        public RelayCommand RateGuestCommand { get; }
        public RelayCommand ForumCommand { get; }
        public RelayCommand AdvicesCommand { get; }
        public ReviewViewModel(ReviewPage page, OwnerWindow parentWindow)
        {
            this.parentWindow = parentWindow;
            this.page = page;
            OwnerAccommodationRates = new ObservableCollection<OwnerAccommodationRateDTO>();
            reviewService = new ReviewService();
            BackCommand = new RelayCommand(BackExecute);
            RequestCommand = new RelayCommand(RequestExecute);
            RenovatingCommand = new RelayCommand(RenovatingExecute);
            RegistrationCommand = new RelayCommand(RegistrationExecute);
            HomePageCommand = new RelayCommand(HomePageExecute);
            OwnerProfileCommand = new RelayCommand(OwnerProfileExecute);
            AccommodationsCommand = new RelayCommand(AccommodationsExecute);
            RateGuestCommand = new RelayCommand(RateGuestExecute);
            ForumCommand = new RelayCommand(ForumExecute);
            AdvicesCommand = new RelayCommand(AdvicesExecute);
            FillOwnerAccommodationRates();
        }
        private void FillOwnerAccommodationRates()
        {
            OwnerAccommodationRates.Clear();
            OwnerAccommodationRates = new ObservableCollection<OwnerAccommodationRateDTO>(reviewService.GetAllOwnerAccommodationRates());
        }
        public void BackExecute()
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
        public void OwnerProfileExecute()
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
