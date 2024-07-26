using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IOwnerAccommodationRateRepository
    {
        public void Save(OwnerAccommodationRate ownerAccommodationRate);
        public void SaveAll(List<OwnerAccommodationRate> ownerAccommodationRates);
        public void Update(OwnerAccommodationRate ownerAccommodationRate);
        public OwnerAccommodationRate GetById(int id);
        public List<OwnerAccommodationRate> GetAll();
    }
}
