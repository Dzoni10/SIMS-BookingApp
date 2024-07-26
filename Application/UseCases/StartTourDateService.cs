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
    public class StartTourDateService
    {
        IStartTourDateRepository startTourDateRepository;
        public StartTourDateService(IStartTourDateRepository startTourDateRepository)
        {
            this.startTourDateRepository = startTourDateRepository;
        }
        public StartTourDate Save(StartTourDate startTourDate)
        {
            return startTourDateRepository.Save(startTourDate);
        }

        public void SaveAll(List<StartTourDate> startTourDates)
        {
            startTourDateRepository.SaveAll(startTourDates);
        }

        public void Update(StartTourDate startTourDate)
        {
            startTourDateRepository.Update(startTourDate);
        }

        public StartTourDate GetById(int id)
        {
            return startTourDateRepository.GetById(id);
        }

        public List<StartTourDate> GetAll()
        {
            return startTourDateRepository.GetAll();
        }

        public List<StartTourDate> LoadTourDates(int id)
        {
            List<StartTourDate> tourDates = new List<StartTourDate>();
            foreach (StartTourDate tourDate in startTourDateRepository.GetAll())
            {
                if (tourDate.TourId == id && tourDate.Date > DateTime.Now)
                {
                    tourDates.Add(tourDate);
                }
            }
            return tourDates;
        }

        public StartTourDate? GetActiveTour()
        {
            return startTourDateRepository.GetAll().FirstOrDefault(t => t.Status == TourStatus.Active);
        }
        public List<StartTourDate> GetByTourId(int id)
        {
            List<StartTourDate> dates = new List<StartTourDate>();
            foreach(StartTourDate startTourDate in startTourDateRepository.GetAll())
            {
                if(startTourDate.TourId == id)
                {
                    dates.Add(startTourDate);
                }
            }
            return dates;
        }

        public List<int> GetYears()
        {
            return startTourDateRepository.GetAll().Select(startTourDate => startTourDate.Date.Year).Distinct().ToList();
        }

        public List<StartTourDate> GetToursToCancel()
        {
            List<StartTourDate> date = new List<StartTourDate>();
            foreach (StartTourDate startDate in startTourDateRepository.GetAll())
            {
                if ((startDate.Status == TourStatus.Created || startDate.Status == TourStatus.Reserved) && startDate.Date >= DateTime.Now.AddHours(48))
                {
                    date.Add(startDate);
                }
            }
            return date;
        }

        public List<StartTourDate> GetFollowingTours()
        {
            List<StartTourDate> date = new List<StartTourDate>();
            foreach (StartTourDate startDate in startTourDateRepository.GetAll())
            {
                if (startDate.Date.Date == DateTime.Now.Date && startDate.Status == TourStatus.Reserved || startDate.Status == TourStatus.Active)
                {
                    date.Add(startDate);
                }
            }
            return date;
        }

        public List<StartTourDate> GetFinishedTours()
        {
            List<StartTourDate> dates = new List<StartTourDate>();
            foreach (StartTourDate startDate in startTourDateRepository.GetAll())
            {
                if (startDate.Status == TourStatus.Finished)
                {
                    dates.Add(startDate);
                }
            }
            return dates;
        }

        public List<StartTourDate> GetForGuide(List<Tour> guidedTour)
        {
            List<StartTourDate> dates = new List<StartTourDate>();
            foreach(Tour tour in guidedTour)
            {
                dates.AddRange(GetToursDate(tour));
            }
            return dates;
        }
        private List<StartTourDate> GetToursDate(Tour tour)
        {
            List<StartTourDate> dates = new List<StartTourDate>();
            foreach (var date in startTourDateRepository.GetAll())
            {
                if (date.TourId == tour.Id)
                {
                    dates.Add(date);
                }
            }
            return dates;
        }

    }
}
