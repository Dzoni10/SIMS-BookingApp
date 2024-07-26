using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using Notifications.Wpf.Core.Controls;
using Notifications.Wpf.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Media.Effects;

namespace BookingApp.WPF.ViewModel
{
    public class CreateSimpleTourRequestViewModel : INotifyPropertyChanged
    {
        public CreateSimpleTourRequest parentWindow { get; set; }
        public User LoggedInUser { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Language { get; set; }

        private DateTime dateFrom;

        public DateTime DateFrom
        {
            get { return dateFrom; }
            set
            {
                if(dateFrom != value)
                {
                    dateFrom = value;
                    OnPropertyChanged("DateFrom");
                    DateTo = dateFrom.AddDays(3);
                } 
            }
        }

        private DateTime dateTo;

        public DateTime DateTo
        {
            get { return dateTo; }
            set
            {
                if(dateTo != value)
                {
                    dateTo = value;
                    OnPropertyChanged("DateTo");
                }
            }
        }

        public int NumberOfGuests { get; set; }
        public int GuestCounter { get; set; }
        public string Description { get; set; }
        public string TouristName { get; set; }
        public int TouristAge { get; set; }
        public RelayCommand EnterCommand { get; set; }
        public RelayCommand AddTouristCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand RequestCommand { get; set; }
        public RelayCommand ChangeMinDateCommand { get; set; }
        public RelayCommand TutorialCommand { get; set; }
        public ObservableCollection<TourRequestGuestDTO> TourGuests { get; set; }
        private TourRequestService TourRequestService;
        private TourRequestGuestService TourRequestGuestSerice;
        private LocationService LocationService;
        private LanguageService LanguageService;
        private readonly NotificationManager _notificationManager = new NotificationManager(NotificationPosition.TopRight);
        public CreateSimpleTourRequestViewModel(CreateSimpleTourRequest createSimpleTourRequest, User user)
        {
            parentWindow = createSimpleTourRequest;
            DateFrom = DateTime.Now;
            DateTo = DateFrom.AddDays(3);
            NumberOfGuests = 1;
            TouristAge = 12;
            LoggedInUser = user;
            TourGuests = new ObservableCollection<TourRequestGuestDTO>();
            GuestCounter = 0;
            TourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>()));
            TourRequestGuestSerice = new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>());
            LanguageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());
            LocationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            EnterCommand = new RelayCommand(EnterDetailsClick);
            AddTouristCommand = new RelayCommand(AddTouristClick);
            CancelCommand = new RelayCommand(CancelClick);
            RequestCommand = new RelayCommand(RequestClick);
            ChangeMinDateCommand = new RelayCommand(ChangeMinDate);
            TutorialCommand = new RelayCommand(TutorialClick);
        }

        private void TutorialClick()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(parentWindow);
            
        }

        public void EnterDetailsClick()
        {
            if (ShouldSystemSendNotification())
                return;
            parentWindow.StateTextBox.IsReadOnly = true;
            parentWindow.CityTextBox.IsReadOnly = true;
            parentWindow.LanguageTextBox.IsReadOnly = true;
            parentWindow.DateFromDatePicker.IsEnabled = false;
            parentWindow.DateToDatePicker.IsEnabled = false;
            parentWindow.NumberOfGuestsTextBox.IsReadOnly = true;
            parentWindow.DescriptionTextBox.IsReadOnly = true;
            parentWindow.EnterButton.IsEnabled = false;
            parentWindow.AddTouristButton.IsEnabled = true;
            parentWindow.TouristNameTextBox.IsReadOnly = false;
            parentWindow.TouristAgeTextBox.IsReadOnly = false;
        }

        private void ChangeMinDate()
        {
            parentWindow.DateToDatePicker.DisplayDateStart = DateFrom.AddDays(3);
            parentWindow.DateToDatePicker.IsEnabled = true;
        }

        private bool IsTourFilled()
        {
            if (NumberOfGuests == GuestCounter)
                return true;
            return false;
        }

        private void AddTouristClick()
        {
            if (ShouldSystemSendNotification())
                return;
            TourRequestGuest tourist = new TourRequestGuest(TouristName, TouristAge, 0, 0);
            TourRequestGuestDTO touristDTO = new TourRequestGuestDTO(tourist);
            TourGuests.Add(touristDTO);
            GuestCounter++;
            if (IsTourFilled())
            {
                parentWindow.AddTouristButton.IsEnabled = false;
                parentWindow.RequestButton.IsEnabled = true;
            }
            else
                ChangeTextBoxesAfterAdding();
        }

        private void ChangeTextBoxesAfterAdding()
        {
            parentWindow.TouristAgeTextBox.Value = 12;
            parentWindow.TouristNameTextBox.Text = "";
            parentWindow.TouristLabel.Content = $"{GuestCounter + 1}. tourist";
        }

        private void CancelClick()
        {
            parentWindow.MainFrame.Visibility = Visibility.Visible;
            parentWindow.MainFrame.Content = new CreateSimpleTourRequest(LoggedInUser);
        }

        private void RequestClick()
        {
            Location location = GetLocation();
            Language language = GetLanguage();
            DateOnly startingDate = new DateOnly(DateFrom.Year, DateFrom.Month, DateFrom.Day);
            DateOnly finishingDate = new DateOnly(DateTo.Year, DateTo.Month, DateTo.Day);
            TourRequest request = new TourRequest(LoggedInUser, location, Description, language, startingDate, finishingDate, NumberOfGuests, 0, 0);
            TourRequestService.Save(request);
            foreach (TourRequestGuestDTO guestDTO in TourGuests)
            {
                TourRequestGuest guest = guestDTO.ToTourRequestGuest();
                guest.TourRequestReservationId = request.Id;
                TourRequestGuestSerice.Save(guest);
            }
            ShowAcceptanceMessage();
        }

        private Location GetLocation()
        {
            Location location = new Location();
            if (LocationService.IsLocationAlreadyWritten(State, City))
            {
                location = LocationService.GetLocation(State, City);
                return location;
            }
            location = new Location(State, City);
            LocationService.Save(location);
            return location;
        }

        private Language GetLanguage()
        {
            Language language = new Language();
            if (LanguageService.IsLanguageAlreadyWritten(Language))
            {
                language = LanguageService.GetLanguageByName(Language);
                return language;
            }
            language = new Language(Language);
            LanguageService.Save(language);
            return language;
        }

        private bool ShouldSystemSendNotification()
        {
            var duration = TimeSpan.FromSeconds(3);
            var blurEffect = new BlurEffect
            {
                Radius = 5
            };

            var content = new NotificationContent
            {
                Title = "Oh no!",
                Message = "Incorrect: "
            };

            content = SetNotificationMessage(content);
            if (content.Message == "Incorrect: ")
                return false;

            SendNotification(blurEffect, content, duration);
            return true;

        }

        private async void SendNotification(BlurEffect blurEffect, NotificationContent content, TimeSpan duration)
        {
            SetAllToBlurExceptNotification(blurEffect);
            await _notificationManager.ShowAsync(content, "WindowArea", expirationTime: duration);
            SetAllBackToNormal();
        }

        private NotificationContent SetNotificationMessage(NotificationContent content)
        {
            if (parentWindow.EnterButton.IsEnabled) {
                if (parentWindow.StateTextBox.Text == "")
                    content.Message += "enter word(s) for state, ";
                if (parentWindow.CityTextBox.Text == "")
                    content.Message += "enter word(s) for city, ";
                if (parentWindow.LanguageTextBox.Text == "")
                    content.Message += "enter word(s) for language, ";
                if (parentWindow.NumberOfGuestsTextBox.Value == 0)
                    content.Message += "enter number for language,";
                if (parentWindow.DescriptionTextBox.Text == "")
                    content.Message += "enter word(s) for description.";
            }
            else {
                if (parentWindow.TouristNameTextBox.Text == "")
                    content.Message += "enter full name for tourist.";
            }
            return content;
        }

        private void SetAllToBlurExceptNotification(BlurEffect blurEffect)
        {
            foreach (var element in parentWindow.ThisGrid.Children.OfType<UIElement>())
            {
                if (element != parentWindow.WindowArea)
                {
                    element.Effect = blurEffect;
                    element.IsEnabled = false;
                }
            }
        }

        private void SetAllBackToNormal()
        {
            foreach (var element in parentWindow.ThisGrid.Children.OfType<UIElement>())
            {
                if (element != parentWindow.WindowArea)
                {
                    element.Effect = null;
                    element.IsEnabled = true;
                }
            }
        }

        private void ShowAcceptanceMessage()
        {
            parentWindow.TouristLabel.Visibility = Visibility.Collapsed;
            parentWindow.TouristNameLabel.Visibility = Visibility.Collapsed;
            parentWindow.TouristNameTextBox.Visibility = Visibility.Collapsed;
            parentWindow.TouristAgeLabel.Visibility = Visibility.Collapsed;
            parentWindow.TouristAgeTextBox.Visibility = Visibility.Collapsed;
            parentWindow.AddTouristButton.Visibility = Visibility.Collapsed;
            parentWindow.RequestButton.Visibility = Visibility.Collapsed;
            parentWindow.CancelButton.Visibility = Visibility.Collapsed;
            parentWindow.AcceptanceMessage.Visibility = Visibility.Visible;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

