using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class UserRepository : IUserRepository
    {

        Repository<User> userRepository;

        private List<User> _users;
        public UserRepository()
        {
            userRepository = new Repository<User>();
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
        public User GetById(int id)
        {
            return userRepository.FindId(id);
        }

        public User Login(string password, string username)
        {
            foreach (var ceredential in GetAll())
            {
                if (ceredential.Username.Equals(username) && ceredential.Password.Equals(password))
                {
                    return ceredential;
                }
            }
            return null;
        }

    }

}