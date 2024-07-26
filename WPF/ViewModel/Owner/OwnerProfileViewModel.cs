using BookingApp.WPF.View.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Utilities;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Application.UseCases;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class OwnerProfileViewModel
    {
        private readonly double superOwnerLimit = 4.5;
        private bool isSuperOwner = false;
        public OwnerProfilePage page;
        private readonly OwnerWindow parentWindow;
        public OwnerProfileService ownerProfileService;
        public RelayCommand BackCommand { get; }
        public RelayCommand RequestCommand { get; }
        public RelayCommand RenovatingCommand { get; }
        public RelayCommand ReviewCommand { get; }
        public RelayCommand RegistrationCommand { get; }
        public RelayCommand HomePageCommand { get; }
        public RelayCommand AccommodationsCommand { get; }
        public RelayCommand RateCommand { get; }
        public RelayCommand AdvicesCommand { get; }
        public RelayCommand ForumCommand { get; }
        public OwnerProfileViewModel(OwnerProfilePage page, OwnerWindow parentWindow)
        {
            this.page = page;
            this.parentWindow = parentWindow;
            ownerProfileService = new OwnerProfileService();
            BackCommand = new RelayCommand(BackExecute);
            RequestCommand = new RelayCommand(RequestExecute);
            RenovatingCommand = new RelayCommand(RenovatingExecute);
            ReviewCommand = new RelayCommand(ReviewExecute);
            RegistrationCommand = new RelayCommand(RegistrationExecute);
            HomePageCommand = new RelayCommand(HomePageExecute);
            AccommodationsCommand = new RelayCommand(AccommodationsExecute);
            RateCommand = new RelayCommand(RateExecute);
            AdvicesCommand = new RelayCommand(AdvicesExecute);
            ForumCommand = new RelayCommand(ForumExecute);
            Update();
        }
        public void Update()
        {
            page.RateTextBox.Text = ownerProfileService.CalculateOwnerAverageRate().ToString();
            isSuperOwner = ownerProfileService.CalculateOwnerAverageRate() > superOwnerLimit ? true : false;
            page.RoleTextBox.Text = isSuperOwner ? ((Properties.Settings.Default.currentLanguage == "en-US") ? "SuperOwner": "Super vlasnik") : ((Properties.Settings.Default.currentLanguage == "en-US") ?"Owner":"Vlasnik");
            page.NameTextBox.Text = ownerProfileService.GetAccommodationOwner().Name;
            ownerProfileService.GetAccommodationOwner().AverageGrade = ownerProfileService.CalculateOwnerAverageRate();
            ownerProfileService.GetAccommodationOwner().IsSuperOwner = isSuperOwner;
            ownerProfileService.UpdateOwner(ownerProfileService.GetAccommodationOwner());
        }
        public void BackExecute()
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

        public void AccommodationsExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new AccommodationsPage(parentWindow));
        }

        public void RateExecute()
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
