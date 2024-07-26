using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class GuestService
    {
        private IGuestRepository guestRepository;

        public AccommodationReservationService accommodationReservationService;

        public GuestService()
        {
            guestRepository = Injector.Injector.CreateInstance<IGuestRepository>();

            accommodationReservationService = new AccommodationReservationService();
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
        public void Delete(Guest guest)
        {
            guestRepository.Delete(guest);
        }
        public Guest GetById(int id)
        {
            return guestRepository.GetById(id);
        }
        public void CheckSuperGuestStatus(int guestId)
        {
            Guest guest = guestRepository.GetById(guestId);
            if (DateTime.Now > guest.SuperGuestExpirationDate)
            {
                UpdateSuperGuestStatus(guest);
            }
        }
        public void SuperGuestReservation(int guestId)
        {
            Guest guest = guestRepository.GetById(guestId);
            if(guest.SuperGuestStatus == true && guest.Points > 0)
            {
                guest.Points -= 1;
                guestRepository.Update(guest);
            }
        }
        public void UpdateSuperGuestStatus(Guest guest)
        {
            int reservationCount = accommodationReservationService.CountReservationsInLastYear(guest.Id);
            if (reservationCount < 10)
            {
                guest.SuperGuestStatus = false;
                guest.Points = 0;
                guest.SuperGuestExpirationDate = DateTime.Now;
            }
            else
            {
                guest.SuperGuestStatus = true;
                guest.Points = 5;
                guest.SuperGuestExpirationDate = DateTime.Now.AddMonths(12);
            }
            guestRepository.Update(guest);
        }
        
        public bool HasBeenOnLocation(int guestId, int locationId)
        {
            foreach(AccommodationReservation reservation in accommodationReservationService.GetFinishedReservationsForGuest(guestId))
            {
                if(reservation.LocationId == locationId)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetGuestNameById(int id)
        {
            string name = "";
            foreach(Guest guest in guestRepository.GetAll())
            {
                if(guest.Id == id)
                {
                    name = guest.Name;
                }
            }
            return name;
        }
        
    }
}
