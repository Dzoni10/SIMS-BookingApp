using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IGuestRepository
    {
        public List<Guest> GetAll();
        public void Save(Guest guest);
        public void SaveAll(List<Guest> guests);
        public void Update(Guest guest);
        public void Delete(Guest guest);
        public Guest GetById(int id);
    }
}
