using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class CreateTourService
    {
        private LocationService locationService;
        private StartTourDateService startTourDateService;
        private LanguageService languageService;
        private CheckpointService checkpointService;
        private ImageService imageService;
        private TourService tourService;
        private TourRequestService tourRequestService;
        private NotificationService notificationService;
        public CreateTourService(LocationService locationService, StartTourDateService startTourDateService, LanguageService languageService, CheckpointService checkpointService, ImageService imageService, TourService tourService, TourRequestService tourRequestService, NotificationService notificationService)
        {
            this.startTourDateService = startTourDateService;
            this.locationService = locationService;
            this.languageService = languageService;
            this.checkpointService = checkpointService;
            this.imageService = imageService;
            this.tourService = tourService;
            this.tourRequestService = tourRequestService;
            this.notificationService = notificationService;
        }

        public void Create(Tour tour, int userId, string SelectedState, string SelectedCity, string languageName, List<DateTime> dateTimes, List<string> checkpointsNames, List<string> imagesPaths) 
        {
            tour.Location = SetLocation(SelectedState, SelectedCity);
            tour.Language = SetLanguage(languageName);
            tour.UserId = userId;
            tour = tourService.Save(tour);

            SetStartTourDates(tour, dateTimes);
            SetCheckpoints(tour, checkpointsNames);
            SetImages(tour, imagesPaths);
            PossiblyNotifyOtherTourists(tour);
        }

        private void PossiblyNotifyOtherTourists(Tour request)
        {
            Tuple<bool, List<int>> tuple = tourService.ShouldOtherTouristsBeNotified(request);
            if (tuple.Item1 == false)
                return;
            foreach (int id in tuple.Item2)
            {
                notificationService.Save(new Notification(id, request.Id.ToString(), NotificationType.NewTour));
            }
        }

        private Location SetLocation(string SelectedState, string SelectedCity)
        {
            var location = locationService.GetAll().
                Where(location => location.State.Equals(SelectedState) && location.City.Equals(SelectedCity)).ToList();
            return location[0];
        }
        private Language SetLanguage(string languageName)
        {
            Language language = new Language(languageName);
            language = languageService.Save(language);
            return language;
        }
        private void SetStartTourDates(Tour tour, List<DateTime> dateTimes)
        {
            foreach (var date in dateTimes)
            {
                startTourDateService.Save(new StartTourDate(date, tour.Id, tour.Capacity));
            }
        }

        private void SetCheckpoints(Tour tour, List<string> checkpointsNames)
        {
            foreach (string check in checkpointsNames)
            {
                checkpointService.Save(new Checkpoint(check, tour.Id));
            }
        }
        private void SetImages(Tour tour, List<string> imagesPaths)
        {
            foreach (string image in imagesPaths)
            {
                imageService.Save(new Domain.Models.Image(image, tour.Id, EntityType.t));
            }
        }

        public List<string> SetStates()
        {
            List<string> states = new List<string>();
            foreach (var state in locationService.GetUniqueStates())
            {
                states.Add(state.State);
            }
            return states;
        }

        public List<string> SetCities(string selectedState)
        {
            List<string> cities = new List<string>();
            if (string.IsNullOrEmpty(selectedState))
            {
                return new List<string>();
            }
            else
            {
                return locationService.GetAll().Where(location => location.State.Equals(selectedState)).
                Select(location => location.City).Distinct().ToList();
            }
        }
    }
}
