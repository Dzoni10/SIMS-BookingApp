using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IGuestAccommodationRateRepository
    {
        public void Save(GuestAccommodationRate guestAccommodationRate);
        public void SaveAll(List<GuestAccommodationRate> guestAccommodationRates);
        public void Update(GuestAccommodationRate guestAccommodationRate);
        public GuestAccommodationRate GetById(int id);
        public List<GuestAccommodationRate> GetAll();
    }
}
