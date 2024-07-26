using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IOwnerRepository
    {
        public Owner GetById(int id);
        public List<Owner> GetAll();
        public Owner GetOwnerByUsername(string username);
        public void Update(Owner owner);
    }
}
