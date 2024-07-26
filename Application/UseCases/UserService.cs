using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class UserService
    {
        public IUserRepository userRepository;
        private IGuestRepository guestRepository;
        public UserService()
        {
            userRepository = Injector.Injector.CreateInstance<IUserRepository>();
            guestRepository = Injector.Injector.CreateInstance<IGuestRepository>();
        }
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;

            guestRepository = Injector.Injector.CreateInstance<IGuestRepository>();
        }
        public List<User> GetAll()
        {
            return userRepository.GetAll();
        }
        public void Save(User user)
        {
            userRepository.Save(user);
        }
        public void Delete(User user)
        {
            userRepository.Delete(user);
        }
        public void Update(User user)
        {
            userRepository.Update(user);
        }
        public void SaveAll(List<User> users)
        {
            userRepository.SaveAll(users);
        }
        public User Login(string password, string username)
        {
            return userRepository.Login(password, username);
        }
        public int GetGuestIdFromUser(User user)
        {
            List<Guest> guests = guestRepository.GetAll();

            foreach (Guest guest in guests)
            {
                if (user.Id == guest.UserId)
                {
                    return guest.Id;
                }
            }
            return 0;
        }

        public Guest GetGuestFromUser(User user)
        {
            List<Guest> guests = guestRepository.GetAll();

            foreach (Guest guest in guests)
            {
                if (user.Id == guest.UserId)
                {
                    return guest;
                }
            }
            return new Guest();
        }

        public User GetById(int id)
        {
            return userRepository.GetById(id);
        }

        public void IncreaseReservationCount(int id)
        {
            User user = GetById(id);
            if (user.ReservationCount == 0)
                user.FirstReservationDate = DateTime.Now;
            user.ReservationCount++;
            Update(user);
        }

        public void RestoreOriginalFields(User user)
        {
            user.ReservationCount = 0;
            user.FirstReservationDate = DateTime.Now;
            Update(user);
        }
    }
}
