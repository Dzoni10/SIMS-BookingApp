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
    public class TourService
    {
        private ITourRepository tourRepository;
        public StartTourDateService startTourDateService;
        public TourRequestService tourRequestService;
        public TourService(ITourRepository tourRepository, StartTourDateService tourDateSetvice, TourRequestService tourRequestService) 
        {
            this.tourRepository = tourRepository;
            startTourDateService = tourDateSetvice;
            this.tourRequestService = tourRequestService;
        }
        public Tour Save(Tour tour)
        {
            return tourRepository.Save(tour);
        }

        public void SaveAll(List<Tour> tours)
        {
            tourRepository.SaveAll(tours);
        }

        public void Update(Tour tour)
        {
            tourRepository.Update(tour);
        }

        public Tour GetById(int id)
        {
            return tourRepository.GetById(id);
        }

        public List<Tour> GetAll()
        {
            return tourRepository.GetAll();
        }

        public Tour? GetByName(string name)
        {
            return tourRepository.GetAll().FirstOrDefault(t => t.Name.Equals(name));
        }

        public List<Tour> FindAvailableTours()
        {
            List<Tour> availableTours = new List<Tour>();
            foreach (Tour tour in tourRepository.GetAll())
            {
                List<StartTourDate> toursDates = startTourDateService.GetAll().Where(s => s.TourId == tour.Id).ToList();
                int counter = 0;
                foreach (StartTourDate potentialTour in toursDates)
                {
                    if (potentialTour.Date > DateTime.Now && counter == 0)
                    {
                        availableTours.Add(tour);
                        counter++;
                    }
                }
            }
            return availableTours;
        }

        public List<Tour> SearchByDuration(List<Tour> tours, string duration)
        {
            if (!string.IsNullOrEmpty(duration))
            {
                tours = tours.Where(t => t.Duration <= Convert.ToDouble(duration)).ToList();
            }

            return tours;
        }

        public List<Tour> SearchByLanguage(List<Tour> tours, string language)
        {
            if (!string.IsNullOrEmpty(language))
            {
                tours = tours.Where(t => t.Language.Name.ToLower() == language.ToLower()).ToList();
            }

            return tours;
        }

        public List<Tour> SearchByVacantSeats(List<Tour> tours, string vacantSeats)
        {
            if (!string.IsNullOrEmpty(vacantSeats))
            {
                tours = tours.Where(t => t.Capacity >= Convert.ToInt32(vacantSeats)).ToList();
            }
            return tours;
        }
        public List<Tour> GetToursByUserAndTourId(int userId, int tourId)
        {
            List<Tour> tours = new List<Tour>();
            foreach (Tour tour in tourRepository.GetAll())
            {
                if (tour.UserId == userId && tour.Id == tourId)
                {
                    tours.Add(tour);
                }
            }
            return tours;
        }
        public List<Tour> GetToursByGuideId(int userId)
        {
            List<Tour> tours = new List<Tour>();
            foreach (Tour tour in tourRepository.GetAll())
            {
                if (tour.UserId == userId)
                {
                    tours.Add(tour);
                }
            }
            return tours;
        }

        public Tuple<bool, List<int>> ShouldOtherTouristsBeNotified(Tour tour)
        {
            List<TourRequest> invalidRequests = tourRequestService.GetInvalidRequests();
            Tuple<bool, List<int>> tuple = new Tuple<bool, List<int>>(false, new List<int>());
            foreach (TourRequest tr in invalidRequests)
            {
                if (AreStringsEqual(tr.Language.Name, tour.Language.Name))
                {
                    if (DoesIdAlreadyExists(tuple, tr.RequestingUser))
                        continue;
                    tuple.Item2.Add(tr.RequestingUser.Id);
                    tuple = new Tuple<bool, List<int>>(true, tuple.Item2);
                }
                else if (AreStringsEqual(tr.Location.City, tour.Location.City) && AreStringsEqual(tr.Location.State, tour.Location.State))
                {
                    if (DoesIdAlreadyExists(tuple, tr.RequestingUser))
                        continue;
                    tuple.Item2.Add(tr.RequestingUser.Id);
                    tuple = new Tuple<bool, List<int>>(true, tuple.Item2);
                }
            }
            return tuple;
        }

        public bool AreStringsEqual(string name1, string name2)
        {
            if (name1.Equals(name2))
                return true;
            return false;
        }

        public bool DoesIdAlreadyExists(Tuple<bool, List<int>> tuple, User user)
        {
            if (tuple.Item2.Any(id => id == user.Id))
                return true;
            return false;
        }
    }
}
