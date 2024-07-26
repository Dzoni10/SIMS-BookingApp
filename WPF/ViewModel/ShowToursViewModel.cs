using BookingApp.Domain.Models;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows;
using BookingApp.Utilities;
using System.ComponentModel;
using BookingApp.Application.UseCases;
using System.Windows.Controls;
using Notifications.Wpf.Core;
using BookingApp.Application.Injector;
using BookingApp.Domain.RepositoryInterfaces;
using Image = BookingApp.Domain.Models.Image;
using Org.BouncyCastle.Asn1.Crmf;
using Notifications.Wpf.Core.Controls;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Media.Effects;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel
{
    public class ShowToursViewModel
    {
        public ShowTours parentWindow;
        public User LoggedInUser { get; set; }
        public StartTourDateDTO SelectedStartTourDate { get; set; }
        public Image TourImage { get; set; }
        public ObservableCollection<TourCard>  TourCards { get; set; }
        public List<TourDTO> Tours { get; set; }
        public ObservableCollection<StartTourDateDTO> TourDates { get; set; }
        private TourService tourService;
        private LocationService LocationService;
        private ImageService ImageService;
        private StartTourDateService StartTourDateService { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand ClearSearchCommand { get; set; }
        public RelayCommand ReserveCommand { get; set; }
        public RelayCommand TutorialCommand { get; set; }
        private readonly NotificationManager _notificationManager = new NotificationManager(NotificationPosition.TopRight);
        public ShowToursViewModel(ShowTours showTours, User user)
        {
            parentWindow = showTours;
            LoggedInUser = user;
            tourService = new TourService(Injector.CreateInstance<ITourRepository>(), 
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), 
                new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>())));
            TourCards = new ObservableCollection<TourCard>();
            TourDates = new ObservableCollection<StartTourDateDTO>();
            Tours = new List<TourDTO>();
            StartTourDateService = new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>());
            LocationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            ImageService = new ImageService(Injector.CreateInstance<IImageRepository>());

            SearchCommand = new RelayCommand(SearchClick, CanExecute);
            ClearSearchCommand = new RelayCommand(ClearSearchClick, CanExecute);
            ReserveCommand = new RelayCommand(ReserveClick);
            TutorialCommand = new RelayCommand(TutorialClick);


            Update();
            SetUserControl();
        }

        private bool CanExecute()
        {
            return true;
        }

        public void TutorialClick()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(parentWindow);
            navigationService.Navigate(new TutorialStatistics("C:\\Users\\nikol\\Desktop\\Nikola\\Random slike i videi xd\\Random slike i videi xd\\Tutorijali\\ReserveTutorial.mkv"));

        }
        private void Update()
        {
            Tours.Clear();
            List<Tour> availableTours = tourService.FindAvailableTours();
            foreach (Tour tour in availableTours)
            {
                Image image = ImageService.GetImageForTours(tour.Id);
                TourDTO tourDTO = new TourDTO(tour);
                tourDTO.Path = image.Path;
                Tours.Add(tourDTO);
            }
        }

        /*private void HideGridsContent()
        {
            foreach (var child in parentWindow.ToursGrid.Children)
            {
                if (child is UIElement element && element != parentWindow.ShowToursFrame)
                {
                    element.Visibility = Visibility.Collapsed;
                }
            }
        }*/

        private void ClearSearchClick()
        {
            Update();
            SetUserControl();
            parentWindow.CityNameSearch.Text = "";
            parentWindow.StateNameSearch.Text = "";
            parentWindow.DurationSearch.Text = "";
            parentWindow.LanguageNameSearch.Text = "";
            parentWindow.VacantSeatsSearch.Text = "";
        }

        private void SetUserControl()
        {
            TourCards.Clear();
            foreach(TourDTO tour in Tours)
            {
                TourCard card = new TourCard(LoggedInUser, tour.Name, this);
                TourCards.Add(card);
            }
        }

        private void SearchClick()
        {
            Update();
            List<Tour> filtratedTours = tourService.FindAvailableTours();
            Tours.Clear();
            if (ShouldSystemSendNotification())
                return;
            //filtratedTours = SearchByCity(filtratedTours, parentWindow.CityNameSearch.Text);
            filtratedTours = LocationService.SearchByCity(filtratedTours, parentWindow.CityNameSearch.Text);
            filtratedTours = LocationService.SearchByState(filtratedTours, parentWindow.StateNameSearch.Text);
            filtratedTours = tourService.SearchByDuration(filtratedTours, parentWindow.DurationSearch.Text);
            filtratedTours = tourService.SearchByLanguage(filtratedTours, parentWindow.LanguageNameSearch.Text);
            filtratedTours = tourService.SearchByVacantSeats(filtratedTours, parentWindow.VacantSeatsSearch.Text);

            foreach(Tour tour in filtratedTours)
            {
                Tours.Add(new TourDTO(tour));
            }
            SetUserControl();

            //NotificationManager.ShowAsync(new NotificationForValidation("Notification", "This is a notification with a custom view."), "WindowArea");
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

        public void SendValidationNotificationForTourControl(NotificationContent content)
        {
            var duration = TimeSpan.FromSeconds(3);
            var blurEffect = new BlurEffect
            {
                Radius = 5
            };

            SendNotification(blurEffect, content, duration);
        }

        public async void SendNotification(BlurEffect blurEffect, NotificationContent content, TimeSpan duration)
        {
            SetAllToBlurExceptNotification(blurEffect);
            await _notificationManager.ShowAsync(content, "WindowArea", expirationTime: duration);
            SetAllBackToNormal();
        }

        private List<string> SearchFieldsToCheck()
        {
            List<string> list = new List<string>();
            if (parentWindow.StateNameSearch.Text != "")
                list.Add("State");
            if (parentWindow.CityNameSearch.Text != "")
                list.Add("City");
            if (parentWindow.DurationSearch.Text != "")
                list.Add("Duration");
            if (parentWindow.LanguageNameSearch.Text != "")
                list.Add("Language");
            if (parentWindow.VacantSeatsSearch.Text != "")
                list.Add("Seats");
            return list;
        }

        private NotificationContent SetNotificationMessage(NotificationContent content)
        {
            List<string> list = SearchFieldsToCheck();
            if (list.Count == 0)
                content.Message += "you have to enter at least one field!";
            else{
                if (!int.TryParse(parentWindow.DurationSearch.Text, out _) && list.Any(s => s.Equals("Duration")))
                    content.Message += "enter number for duration,";
                if (!int.TryParse(parentWindow.VacantSeatsSearch.Text, out _) && list.Any(s => s.Equals("Seats")))
                    content.Message += "enter number for vacant seats";
            }
            return content;
        }

        private void SetAllToBlurExceptNotification(BlurEffect blurEffect)
        {
            foreach (var element in parentWindow.ToursGrid.Children.OfType<UIElement>())
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
            foreach (var element in parentWindow.ToursGrid.Children.OfType<UIElement>())
            {
                if (element != parentWindow.WindowArea)
                {
                    element.Effect = null;
                    element.IsEnabled = true;
                }
            }
        }

        public void ReserveClick()
        {
            parentWindow.ShowToursFrame.Content = new ReserveTour(SelectedStartTourDate, LoggedInUser, this);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
