using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class SearchTourRequestService
    {
        private TourRequestService tourRequestService;
        private LocationService locationService;
        private StartTourDateService startTourDateService;
        private TourService tourService;
        private NotificationService notificationService;
        public SearchTourRequestService(TourRequestService tourRequestService, LocationService locationService, StartTourDateService startTourDateService, TourService tourService, NotificationService notificationService)
        {
            this.tourRequestService = tourRequestService;
            this.locationService = locationService;
            this.startTourDateService = startTourDateService;
            this.tourService = tourService;
            this.notificationService = notificationService;
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

        public List<TourRequest> SearchByCapacity(List<TourRequest> requests, int capacity)
        {
            if (capacity == 0)
            {
                return requests;
            }
            List<TourRequest> tourRequests = new List<TourRequest>();
            foreach (var request in requests)
            {
                if (request.Capacity == capacity)
                {
                    tourRequests.Add(request);
                }
            }
            return tourRequests;
        }

        public List<TourRequest> SearchByLocation(List<TourRequest> requests, string selectedState, string selectedCity)
        {
            if (selectedState == null && selectedCity == null)
            {
                return requests;
            }
            List<TourRequest> tourRequests = new List<TourRequest>();
            foreach (var request in requests)
            {
                if (request.Location.State == selectedState && request.Location.City == selectedCity)
                {
                    tourRequests.Add(request);
                }
            }
            return tourRequests;
        }

        public List<TourRequest> SearchByLanguage(List<TourRequest> requests, string language)
        {
            if (language == "" || language == null)
            {
                return requests;
            }
            List<TourRequest> tourRequests = new List<TourRequest>();
            foreach (var request in requests)
            {
                if (request.Language.Name.ToLower() == language.ToLower())
                {
                    tourRequests.Add(request);
                }
            }
            return tourRequests;
        }
        
        public List<TourRequest> SearchByDate(List<TourRequest> requests, DateTime startDate, DateTime endDate)
        {
            if(endDate.Date == DateTime.Now.AddYears(10).Date)
            {
                return requests;
            }
            List<TourRequest> tourRequests = new List<TourRequest>();
            foreach (var request in requests)
            {
                if (DateOnly.FromDateTime(startDate) <= request.DateFrom && DateOnly.FromDateTime(endDate) >= request.DateTo)
                {
                    tourRequests.Add(request);
                }
            }
            return tourRequests;
        }

        public bool IsDateValid(DateTime date, int guideId)
        {
            foreach(var startTourDate in startTourDateService.GetForGuide(tourService.GetToursByGuideId(guideId)))
            {
                int duration = (int) (tourService.GetById(startTourDate.TourId).Duration + 0.5);
                if(startTourDate.Date < date && date < startTourDate.Date.AddHours(duration))
                {
                    return false;
                }
            }
            return true;
        }

        public void Accept(TourRequest tourRequest, int guideId, DateTime date) 
        {
            tourRequest.ComplexTourRequestId = tourRequest.ComplexTourRequestId;
            tourRequest.Status = TourRequestStatus.Accepted;
            tourRequest.GuideId = guideId;
            tourRequest.DateIfAccepted = date;
            tourRequestService.Update(tourRequest);
            CreateTourForRequest(tourRequest, date);
            notificationService.Save(new Notification(tourRequest.RequestingUser.Id, tourRequest.Id.ToString(), NotificationType.AcceptedTourRequest));
        }
        private void CreateTourForRequest(TourRequest tourRequest, DateTime date)
        {
            StartTourDate startTourDate = new StartTourDate(date, tourRequest.Id, tourRequest.Capacity);
            startTourDate.Status = TourStatus.Request;
            startTourDateService.Save(startTourDate);
        }
    }
}
