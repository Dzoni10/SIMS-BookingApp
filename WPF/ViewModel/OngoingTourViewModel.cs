using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel
{
    public class OngoingTourViewModel
    {

        public OngoingTour parentWindow { get; set; }
        public User LoggedInUser { get; set; }
        public StartTourDateDTO SelectedStartTourDateDTO { get; set; }
        private StartTourDateService startTourDateService;
        private TourService tourService;
        private CheckpointService checkpointService;
        private UserService userService;

        public string TourName { get; set; }
        public string TourDate { get; set; }
        public string TourGuide { get; set; }
        public string TourLanguage { get; set; }
        public double TourDuration { get; set; }
        public string TourCurrentCheckpoint { get; set; }

        public OngoingTourViewModel(OngoingTour ongoingTour, User user)
        {
            parentWindow = ongoingTour;
            LoggedInUser = user;
            tourService = new TourService(Injector.CreateInstance<ITourRepository>(), 
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), 
                new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>())));
            startTourDateService = new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>());
            checkpointService = new CheckpointService(Injector.CreateInstance<ICheckpointRepository>());
            userService = new UserService();
            SelectedStartTourDateDTO = new StartTourDateDTO(startTourDateService.GetActiveTour());
            InitializeFields();
        }

        private void InitializeFields()
        {
            TourName = tourService.GetById(SelectedStartTourDateDTO.TourId).Name;
            TourDate = SelectedStartTourDateDTO.Date.ToString("dd.MM.yyyy HH:mm");
            TourGuide = userService.GetById(tourService.GetById(SelectedStartTourDateDTO.TourId).UserId).Username;
            TourLanguage = tourService.GetById(SelectedStartTourDateDTO.TourId).Language.Name;
            TourDuration = tourService.GetById(SelectedStartTourDateDTO.TourId).Duration;
            TourCurrentCheckpoint = checkpointService.GetById(SelectedStartTourDateDTO.CurrentCheckpoint).Name;
        }

    }
}
