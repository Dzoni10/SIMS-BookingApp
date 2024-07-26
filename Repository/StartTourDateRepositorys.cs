using System.Collections.Generic;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;

namespace BookingApp.Repository
{
    public class StartTourDateRepository : IStartTourDateRepository
    {
        Repository<StartTourDate> startTourDateRepository;

        public StartTourDateRepository()
        {
            startTourDateRepository = new Repository<StartTourDate>();
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
            return startTourDateRepository.FindId(id);
        }

        public List<StartTourDate> GetAll()
        {
            return startTourDateRepository.GetAll();
        }
    }
}
