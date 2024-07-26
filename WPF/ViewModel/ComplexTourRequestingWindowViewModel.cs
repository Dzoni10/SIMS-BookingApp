using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using Notifications.Wpf.Core;
using Notifications.Wpf.Core.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Effects;

namespace BookingApp.WPF.ViewModel
{
    public class ComplexTourRequestingWindowViewModel : INotifyPropertyChanged
    {
        public ComplexTourRequestingWindow parentWindow { get; set; }

        public User LoggedInUser { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string TourLanguage { get; set; }
        public string Description { get; set; }

        private DateTime dateFrom;

        public DateTime DateFrom
        {
            get { return dateFrom; }
            set
            {
                if (dateFrom != value)
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
                if (dateTo != value)
                {
                    dateTo = value;
                    OnPropertyChanged("DateTo");
                }
            }
        }

        public int NumberOfTourists { get; set; }
        public string TouristName { get; set; }
        public int TouristAge { get; set; }

        public int GuestCounter { get; set; }

        public ObservableCollection<TourRequestDTO> Requests { get; set; }
        public ObservableCollection<TourRequestGuestDTO> TourGuests { get; set; }
        private LanguageService LanguageService;
        private LocationService LocationService;
        private TourRequestService TourRequestService;
        private TourRequestGuestService TourRequestGuestService;
        private ComplexTourRequestService ComplexTourRequestService;

        public RelayCommand AddTourCommand { get; set; }
        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand FinishAddingCommand { get; set; }
        public RelayCommand AddTouristCommand { get; set; }
        public RelayCommand RequestCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ChangeMinDateCommand { get; set; }
        private readonly NotificationManager _notificationManager = new NotificationManager(NotificationPosition.TopRight);
        public ComplexTourRequestingWindowViewModel(ComplexTourRequestingWindow window, User user)
        {
            parentWindow = window;
            LoggedInUser = user;
            Requests = new ObservableCollection<TourRequestDTO>();
            TourGuests = new ObservableCollection<TourRequestGuestDTO>();
            LocationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            LanguageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());
            TourRequestGuestService = new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>());
            TourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), TourRequestGuestService);
            ComplexTourRequestService = new ComplexTourRequestService(Injector.CreateInstance<IComplexTourRequestRepository>());
            GuestCounter = 0;
            NumberOfTourists = 1;
            DateFrom = DateTime.Now;
            DateTo = DateFrom.AddDays(3);

            AddTourCommand = new RelayCommand(AddTourClick);
            NextPageCommand = new RelayCommand(NextPageClick);
            FinishAddingCommand = new RelayCommand(FinishAddingClick);
            AddTouristCommand = new RelayCommand(AddTouristClick);
            CancelCommand = new RelayCommand(CancelClick);
            RequestCommand = new RelayCommand(RequestClick);
            ChangeMinDateCommand = new RelayCommand(ChangeDate);
        }

        private void AddTourClick()
        {
            if (ShouldSystemSendNotification())
                return;              
            DateOnly dateFrom = new DateOnly(DateFrom.Year, DateFrom.Month, DateFrom.Day);
            DateOnly dateTo = new DateOnly(DateTo.Year, DateTo.Month, DateTo.Day);
            TourRequest request = new TourRequest(LoggedInUser, new Location(State, City), Description, new Language(TourLanguage), dateFrom, dateTo, 0, 0, 0);
            TourRequestDTO requestDTO = new TourRequestDTO(request);
            Requests.Add(requestDTO);
            if (Requests.Count > 1)
                parentWindow.FinishAddingButton.IsEnabled = true;
            SetFieldsBack();
        }

        private void ChangeDate()
        {
            parentWindow.DateToDatePicker.DisplayDateStart = DateFrom.AddDays(3);
            parentWindow.DateToDatePicker.IsEnabled = true;
        }

        private void SetFieldsBack()
        {
            parentWindow.StateTextBox.Text = "";
            parentWindow.CityTextBox.Text = "";
            parentWindow.LanguageTextBox.Text = "";
            parentWindow.DescriptionTextBox.Text = "";
            DateFrom = DateTime.Now;
            DateTo = DateFrom.AddDays(3);
            parentWindow.DateToDatePicker.DisplayDateStart = DateFrom;
        }

        private void NextPageClick()
        {
            parentWindow.GuestNumberLabel.Visibility = Visibility.Collapsed;
            parentWindow.GuestNumberTextBox.Visibility = Visibility.Collapsed;
            parentWindow.NextPageButton.Visibility = Visibility.Collapsed;
            parentWindow.TouristLabel.Visibility = Visibility.Visible;
            parentWindow.GuestsDataGrid.Visibility = Visibility.Visible;
            parentWindow.TouristNameLabel.Visibility = Visibility.Visible;
            parentWindow.TouristNameTextBox.Visibility = Visibility.Visible;
            parentWindow.TouristAgeLabel.Visibility = Visibility.Visible;
            parentWindow.TouristAgeTextBox.Visibility = Visibility.Visible;
            parentWindow.AddTouristButton.Visibility = Visibility.Visible;
            parentWindow.RequestButton.Visibility = Visibility.Visible;
            parentWindow.CancelButton.Visibility = Visibility.Visible;
        }

        private void ChangeTextBoxesAfterAdding()
        {
            parentWindow.TouristAgeTextBox.Value = 0;
            parentWindow.TouristNameTextBox.Text = "";
            parentWindow.TouristLabel.Content = $"{GuestCounter + 1}. tourist";
        }

        private void FinishAddingClick()
        {
            parentWindow.AddTourButton.IsEnabled = false;
            parentWindow.FinishAddingButton.Visibility = Visibility.Collapsed;
            parentWindow.ToursDataGrid.Visibility = Visibility.Collapsed;
            parentWindow.TourListLabel.Visibility = Visibility.Collapsed;
            parentWindow.GuestNumberLabel.Visibility = Visibility.Visible;
            parentWindow.GuestNumberTextBox.Visibility = Visibility.Visible;
            parentWindow.NextPageButton.Visibility = Visibility.Visible;
        }

        private bool IsTourFilled()
        {
            if (NumberOfTourists == GuestCounter)
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

        private void CancelClick()
        {
            parentWindow.MainFrame.Visibility = Visibility.Visible;
            parentWindow.MainFrame.Content = new ComplexTourRequestingWindow(LoggedInUser);
        }

        private void RequestClick()
        {
            ComplexTourRequest complexRequest = new ComplexTourRequest(NumberOfTourists, LoggedInUser.Id);
            complexRequest = ComplexTourRequestService.Save(complexRequest);
            foreach (var item in Requests)
            {
                Location loc = GetLocation(item.Location.State, item.Location.City);
                Language lang = GetLanguage(item.Language.Name);
                item.Location = loc;
                item.Language = lang;
                item.Capacity = NumberOfTourists;
                item.ComplexTourRequestId = complexRequest.Id;
                TourRequestService.Save(item.ToTourRequest());
            }
            foreach (TourRequestGuestDTO guestDTO in TourGuests)
            {
                TourRequestGuest guest = guestDTO.ToTourRequestGuest();
                guest.ComplexTourRequestId = complexRequest.Id;
                TourRequestGuestService.Save(guest);
            }
            ShowAcceptanceMessage();
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


        private Language GetLanguage(string languagee)
        {
            if (LanguageService.IsLanguageAlreadyWritten(languagee))
            {
                Language language1 = LanguageService.GetLanguageByName(languagee);
                return language1;
            }
            Language language = new Language(languagee);
            language = LanguageService.Save(language);
            return language;
        }

        private Location GetLocation(string state, string city)
        {
            if (LocationService.IsLocationAlreadyWritten(state, city))
            {
                Location location1 = LocationService.GetLocation(state, city);
                return location1;
            }
            Location location = new Location(state, city);
            location = LocationService.Save(location);
            return location;
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
            if (parentWindow.AddTourButton.IsEnabled)
            {
                if (parentWindow.StateTextBox.Text == "")
                    content.Message += "enter word(s) for state, ";
                if (parentWindow.CityTextBox.Text == "")
                    content.Message += "enter word(s) for city, ";
                if (parentWindow.LanguageTextBox.Text == "")
                    content.Message += "enter word(s) for language, ";
                if (parentWindow.DescriptionTextBox.Text == "")
                    content.Message += "enter word(s) for description.";
            }
            else if(parentWindow.AddTouristButton.IsEnabled)
            {
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
            if (Requests.Count < 2)
                parentWindow.FinishAddingButton.IsEnabled = false;

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
