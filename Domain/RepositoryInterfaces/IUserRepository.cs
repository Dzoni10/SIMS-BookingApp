using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IUserRepository
    {
        public List<User> GetAll();
        public void Save(User user);
        public void Delete(User user);
        public void Update(User user);
        public void SaveAll(List<User> users);
        public User Login(string password, string username);
        public User GetById(int id);
    }
}
