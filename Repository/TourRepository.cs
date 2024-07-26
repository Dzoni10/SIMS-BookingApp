using System;
using System.Collections.Generic;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;

namespace BookingApp.Repository
{
    public class TourRepository : ITourRepository
    {
        Repository<Tour> tourRepository;

        public TourRepository()
        {
            tourRepository = new Repository<Tour>();
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
            return tourRepository.FindId(id);
        }

        public List<Tour> GetAll()
        {
            return tourRepository.GetAll();
        }

        public Tour? GetByName(string name)
        {
            // Pravljenje lambda izraza za selektor imena
            Func<Tour, string> nameSelector = tour => tour.Name;

            // Pozivanje FindByName funkcije iz tourRepository sa pravilno definisanim selektorom imena i prosleđenim imenom
            return tourRepository.FindByName(nameSelector, name);
        }
    }
}
