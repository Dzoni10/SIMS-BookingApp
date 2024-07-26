using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using Notifications.Wpf.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Image = BookingApp.Domain.Models.Image;

namespace BookingApp.WPF.ViewModel
{
    public class TourCardViewModel : UserControl
    {
        public TourCard parentWindow { get; set; }
        public ShowToursViewModel ParentViewModel { get; set; }
        public User LoggedInUser { get; set; }

        public string TourName { get; set; }
        public Image TourImage { get; set; }

        public StartTourDateDTO SelectedStartTourDate { get; set; }
        public ObservableCollection<StartTourDateDTO> TourDates { get; set; }
        private TourService TourService;
        private ImageService ImageService;
        private StartTourDateService StartTourDateService;
        private CheckpointService CheckpointService;
        public RelayCommand ReserveCommand { get; set; }
        public TourCardViewModel(TourCard window, User user, ShowToursViewModel parentViewModel)
        {
            parentWindow = window;
            ParentViewModel = parentViewModel;
            TourName = parentWindow.TourName;
            LoggedInUser = user;
            TourDates = new ObservableCollection<StartTourDateDTO>();
            TourService = new TourService(Injector.CreateInstance<ITourRepository>(), 
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), 
                new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>())));
            ImageService = new ImageService(Injector.CreateInstance<IImageRepository>());
            StartTourDateService = new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>());
            CheckpointService = new CheckpointService(Injector.CreateInstance<ICheckpointRepository>());
            ReserveCommand = new RelayCommand(ReserveClick);
            LoadDates();
            LoadImage();
            LoadLabels();
        }

        private void LoadDates()
        {
            Tour tour = TourService.GetByName(TourName);
            foreach (StartTourDate date in StartTourDateService.GetAll().Where(s => s.TourId == tour.Id && s.Date > DateTime.Now).ToList())
            {
                TourDates.Add(new StartTourDateDTO(date));
            }
        }

        private void LoadImage()
        {
            Tour tour = TourService.GetByName(TourName);
            Image image = ImageService.GetImageForTours(tour.Id);
            if (image != null)
            {
                TourImage = image;
            }
        }

        private void LoadLabels()
        {
            Tour tour = TourService.GetByName(TourName);
            parentWindow.LocationLabel.Content = $"{tour.Location.City}, {tour.Location.State}";
            parentWindow.DurationLabel.Content = $"{tour.Duration}h";
            parentWindow.LanguageLabel.Content = tour.Language.Name.ToString();
            List<Checkpoint> checkpoints = GetCheckpoints();
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoints[checkpoints.Count - 1] != checkpoint)
                {
                    parentWindow.CheckpointsLabel.Text += $"{checkpoint.Name}, ";
                }
                else
                {
                    parentWindow.CheckpointsLabel.Text += $"{checkpoint.Name}";
                }
            }
        }

        private List<Checkpoint> GetCheckpoints()
        {
            Tour tour = TourService.GetByName(TourName);
            List<Checkpoint> checkpoints = new List<Checkpoint>();
            checkpoints = CheckpointService.GetAll().Where(c => c.TourId == tour.Id).ToList();
            return checkpoints;
        }

        private NotificationContent MakeNotification()
        {
            var content = new NotificationContent
            {
                Title = "Oh no!",
                Message = "Select the tour date that you want to attend to."
            };
            return content;
        }

        private void ReserveClick()
        {
            if (SelectedStartTourDate == null)
            {
                ParentViewModel.SendValidationNotificationForTourControl(MakeNotification());
                return;
            }
            NavigationService navigationService = NavigationService.GetNavigationService(parentWindow);
            navigationService.Navigate(new ReserveTour(SelectedStartTourDate, LoggedInUser, ParentViewModel));
        }
    }
}
