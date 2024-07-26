using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IAccommodationRepository
    {
        public int SavedAccommodationId(Accommodation accommodation);
        public void SaveAll(List<Accommodation> accommodations);
        public void Update(Accommodation accommodation);
        public Accommodation GetById(int id);
        public List<Accommodation> GetAll();
        public void Delete(Accommodation accommodation);
    }
}
