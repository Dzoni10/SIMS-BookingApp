using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Repository
{
    public class GuestRepository : IGuestRepository
    {
        Repository<Guest> guestRepository;
        
        private List<Guest> _guests;
        public GuestRepository()
        {
            guestRepository = new Repository<Guest>();
        }

        public List<Guest> GetAll()
        {
            return guestRepository.GetAll();
        }
        public void Save(Guest guest)
        {
            guestRepository.Save(guest);
        }
        public void SaveAll(List<Guest> guests)
        {
            guestRepository.SaveAll(guests);
        }
        public void Update(Guest guest)
        {
            guestRepository.Update(guest);
        }
        
        public Guest GetById(int id)
        {
            return guestRepository.FindId(id);
        }
        public void Delete(Guest guest)
        {

                guestRepository.Delete(guest);
        }

    }
}
