using BookingApp.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Observer;

using System.Diagnostics.Eventing.Reader;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        Repository<Owner> ownerRepository;

        public Subject ownerSubject;
        public OwnerRepository() 
        { 
            ownerRepository = new Repository<Owner>();
            ownerSubject = new Subject();
        }
        public Owner GetById(int id)
        {
            return ownerRepository.FindId(id);
        }
        public List<Owner> GetAll()
        {
            return ownerRepository.GetAll();
        }

        public Owner GetOwnerByUsername(string username)
        {
            foreach(var owner in GetAll()) 
            { 
                return (owner.Username.Equals(username)) ? owner : null;
                   
            }
            return null;
        }

        public void Update(Owner owner)
        {
            ownerRepository.Update(owner);
            ownerSubject.NotifyObservers();
        }
    }
}
