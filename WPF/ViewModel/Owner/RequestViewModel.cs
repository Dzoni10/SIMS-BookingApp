using BookingApp.Domain.Models;
using BookingApp.Repository;
using System.Windows.Navigation;
using BookingApp.Utilities;
using BookingApp.WPF.View.Owner;
using BookingApp.DTO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using BookingApp.Application.UseCases;
using BookingApp.Application.Injector;
using BookingApp.Domain.RepositoryInterfaces;
namespace BookingApp.WPF.ViewModel.Owner
{
    public class RequestViewModel : INotifyPropertyChanged
    {
        private readonly OwnerWindow parentWindow;
        public RequestPage page;
        public AccommodationReservation accommodationReservation;
        public RequestService requestService;
        public AccommodationsService accommodationsService;
        private EditedReservationDTO selectedEditedReservation;
        public EditedReservationDTO SelectedEditedReservation
        {
            get { return selectedEditedReservation; }
            set
            {
                selectedEditedReservation = value;
                OnPropertyChanged(nameof(SelectedEditedReservation));
                UpdateRescheduleMessage();
            }
        }
        public string AccommodationName { set { OnPropertyChanged("AccommodationName"); } }
        public string GuestName { set { OnPropertyChanged("GuestName"); } }
        public ObservableCollection<EditedReservationDTO> EditedReservations { get; set; }
        public RelayCommand DenyCommand { get; }
        public RelayCommand AcceptCommand { get; }
        public RelayCommand BackCommand { get; }
        public RelayCommand RenovatingCommand { get; }
        public RelayCommand ReviewCommand { get; }
        public RelayCommand RegistrationCommand { get; }
        public RelayCommand HomePageCommand { get; }
        public RelayCommand OwnerProfileCommand { get; }
        public RelayCommand RateGuestCommand { get; }
        public RelayCommand AccommodationsCommand { get; }
        public RelayCommand ForumCommand { get; }
        public RelayCommand AdvicesCommand { get; }
        public RequestViewModel(RequestPage page, OwnerWindow parentWindow)
        {
            this.page = page;
            this.parentWindow = parentWindow;
            accommodationReservation = new AccommodationReservation();
            requestService = new RequestService();
            accommodationsService = new AccommodationsService();
            EditedReservations = new ObservableCollection<EditedReservationDTO>();
            DenyCommand = new RelayCommand(DenyExecute, CanExecute);
            AcceptCommand = new RelayCommand(AcceptExecute, CanExecute);
            BackCommand = new RelayCommand(BackExecute);
            RenovatingCommand = new RelayCommand(RenovatingExecute);
            ReviewCommand = new RelayCommand(ReviewExecute);
            RegistrationCommand = new RelayCommand(RegistrationExecute);
            HomePageCommand = new RelayCommand(HomePageExecute);
            OwnerProfileCommand = new RelayCommand(OwnerProfileExecute);
            AccommodationsCommand = new RelayCommand(AccommodationsExecute);
            RateGuestCommand = new RelayCommand(RateGuestExecute);
            ForumCommand = new RelayCommand(ForumExecute);
            AdvicesCommand = new RelayCommand(AdvicesExecute);
            ShowEditedReservations();
        }
        private bool CanExecute()
        {
            return SelectedEditedReservation != null;
        }
        public void ShowEditedReservations()
        {
            foreach (EditedReservation editedReservation in requestService.GetAllEditedReservations())
            {
                if (editedReservation.Status == ReservationStatus.Pending && !accommodationsService.GetById( editedReservation.AccommodationId).IsClosed)
                {
                    editedReservation.Accommodation = requestService.GetAccommodationById(editedReservation.AccommodationId);
                    editedReservation.Guest = requestService.GetGuestById(editedReservation.GuestId);
                    EditedReservations.Add(new EditedReservationDTO(editedReservation));
                }
            }
        }
        private void UpdateRescheduleMessage()
        {
            page.RescheduleMessage.Visibility= requestService.AllowWarningShow(SelectedEditedReservation) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }
        public void DenyExecute()
        {
            if (SelectedEditedReservation != null)
            {
                NavigationService navigationService = NavigationService.GetNavigationService(page);
                navigationService.Navigate(new IgnorePage(SelectedEditedReservation, EditedReservations, parentWindow));
            }
        }
        public void AcceptExecute()
        {
            EditedReservation editedReservation;
            OwnerGuestNotification ownerGuestNotification;
            accommodationReservation = requestService.GetAccommodationReservationById(SelectedEditedReservation.AccommodationReservationId);
            accommodationReservation.StartDate = SelectedEditedReservation.StartDate;
            accommodationReservation.EndDate = SelectedEditedReservation.EndDate;
            requestService.UpdateAccommodationReservation(accommodationReservation);
            ownerGuestNotification = new OwnerGuestNotification(SelectedEditedReservation.Id, ReservationStatus.Accepted);
            requestService.SaveNotification(ownerGuestNotification);
            editedReservation = requestService.GetEditedReservationById(SelectedEditedReservation.Id);
            editedReservation.Status = ReservationStatus.Accepted;
            requestService.UpdateEditedReservation(editedReservation);
            EditedReservations.Remove(SelectedEditedReservation);
        }
        public void BackExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new AccommodationsPage(parentWindow));
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
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
