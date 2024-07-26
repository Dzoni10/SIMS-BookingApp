using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using Notifications.Wpf.Core;
using Notifications.Wpf.Core.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using System.Windows.Navigation;
using Image = BookingApp.Domain.Models.Image;

namespace BookingApp.WPF.ViewModel
{
    public class ReserveTourViewModel
    {
        public ReserveTour parentWindow { get; set; }
        public ShowToursViewModel otherViewModel { get; set; }
        public User LoggedInUser { get; set; }
        public int NumberOfGuests { get; set; }
        public StartTourDateDTO SelectedStartTourDate { get; set; }
        public ObservableCollection<TourCard>Tours { get; set; }
        private TourService tourService;
        private StartTourDateService startTourDateService;
        private CheckpointService checkpointService;
        private ImageService imageService;
        public Image FirstImage { get; set; }
        public Image SecondImage { get; set; }
        public Image ThirdImage { get; set; }

        public RelayCommand CheckAvailableSpotsCommand { get; set; }
        public RelayCommand ReserveAnotherTourCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        private readonly NotificationManager _notificationManager = new NotificationManager(NotificationPosition.TopRight);

        public ReserveTourViewModel(ReserveTour reserveTour, User user, StartTourDateDTO selectedStartTourDate, ShowToursViewModel viewModel)
        {
            parentWindow = reserveTour;
            otherViewModel = viewModel;
            SelectedStartTourDate = selectedStartTourDate;
            NumberOfGuests = 1;
            Tours = new ObservableCollection<TourCard>();
            tourService = new TourService(Injector.CreateInstance<ITourRepository>(), 
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), 
                new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>())));
            imageService = new ImageService(Injector.CreateInstance<IImageRepository>());
            LoggedInUser = user;
            LoadImages();
            InitializeLabel();
            Update();

            CheckAvailableSpotsCommand = new RelayCommand(CheckAvailableSpots);
            CancelCommand = new RelayCommand(CancelClick);
        }

        private void Update()
        {
            Tours.Clear();
            string state = tourService.GetById(SelectedStartTourDate.TourId).Location.State;
            string city = tourService.GetById(SelectedStartTourDate.TourId).Location.City;
            foreach (TourDTO tour in SearchByLocation(state, city))
            {
                TourCard card = new TourCard(LoggedInUser, tour.Name, otherViewModel);
                Tours.Add(card);
            }
            if (Tours.Count == 0)
                LoadIfZeroRecommendations();
        }

        private void LoadIfZeroRecommendations()
        {
            List<Tour> availableTours = tourService.FindAvailableTours();
            foreach (Tour tour in availableTours)
            {
                if (CheckIfSameTours(tour))
                    continue;
                Image image = imageService.GetImageForTours(tour.Id);
                TourDTO tourDTO = new TourDTO(tour);
                tourDTO.Path = image.Path;
                Tours.Add(new TourCard(LoggedInUser, tourDTO.Name, otherViewModel));
            }
        }

        private bool CheckIfSameTours(Tour checkTour)
        {
            Tour tour = tourService.GetById(SelectedStartTourDate.TourId);
            if (tour.Name.Equals(checkTour.Name))
                return true;
            return false;
        }

        private void InitializeLabel()
        {
            Tour tour = tourService.GetById(SelectedStartTourDate.TourId);
            string amPm = SelectedStartTourDate.Date.ToString("tt");
            if (amPm.Equals("AM"))
                parentWindow.TourNameAndDateLabel.Content = $"{tour.Name} {SelectedStartTourDate.Date.Day}.{SelectedStartTourDate.Date.Month}.{SelectedStartTourDate.Date.Year}. {SelectedStartTourDate.Date.Hour}:{SelectedStartTourDate.Date.Minute}";
            else
            {
                SelectedStartTourDate.Date.AddHours(12);
                parentWindow.TourNameAndDateLabel.Content = $"{tour.Name} {SelectedStartTourDate.Date.Day}.{SelectedStartTourDate.Date.Month}.{SelectedStartTourDate.Date.Year}. {SelectedStartTourDate.Date.Hour}:{SelectedStartTourDate.Date.Minute}";
            }
        }

        public void CancelClick()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(parentWindow);
            navigationService.Navigate(new ShowTours(LoggedInUser));
        }

        private void CheckAvailableSpots()
        {
            int availableSpots = SelectedStartTourDate.Capacity;
            if (NumberOfGuests > availableSpots)
            {
                parentWindow.SeatsLeftLabel.Content = $"Capacity of this tour is {availableSpots}.";
                return;
            }
            ShowReserveForm();
        }

        private void ShowReserveForm()
        {
            ShowVacantSpotsLeft();
            parentWindow.ScrollViewerName.Visibility = Visibility.Collapsed;
            parentWindow.ItemsControlName.Visibility = Visibility.Collapsed;
            parentWindow.CheckOutLabel.Visibility = Visibility.Collapsed;
            parentWindow.ImageNumberTwo.Visibility = Visibility.Visible;
            parentWindow.ImageNumberOne.Visibility = Visibility.Visible;
            parentWindow.ImageNumberThree.Visibility = Visibility.Visible;
            parentWindow.CheckOutLabel.Visibility = Visibility.Collapsed;
            ObservableCollection<TourGuestDTO> SignedUpTourists = new ObservableCollection<TourGuestDTO>();
            int numberOfReservationsCounter = 1;
            parentWindow.ReserveForm.Content = new ReserveTourForm(SelectedStartTourDate, LoggedInUser, NumberOfGuests, numberOfReservationsCounter, SignedUpTourists, otherViewModel, this);
        }

        private void LoadImages()
        {
            List<Image> images = imageService.GetImagesForTour(SelectedStartTourDate.TourId);
            FirstImage = images[0];
            SecondImage = images[1];
            ThirdImage = images[2];
        }

        public void SendValidationNotificationForReserveControl(NotificationContent content)
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

        private void ShowVacantSpotsLeft()
        {
            parentWindow.SeatsLeftLabel.Content = $"{SelectedStartTourDate.Capacity - NumberOfGuests} vacant spots left";
            parentWindow.SeatsLeftLabel.Visibility = Visibility.Visible;
        }

        private List<TourDTO> SearchByLocation(string state, string city)
        {
            List<TourDTO> filtratedTours = new List<TourDTO>();
            foreach (Tour tour in tourService.FindAvailableTours())
            {
                if (tour.Location.State == state && tour.Location.City == city && tour.Id != SelectedStartTourDate.TourId)
                {
                    filtratedTours.Add(new TourDTO(tour));
                }
            }
            return filtratedTours;
        }
    }
}
