using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class OwnerService
    {

        private IOwnerRepository ownerRepository;

        public OwnerService()
        {
            ownerRepository = Injector.Injector.CreateInstance<IOwnerRepository>();
        }

        public Owner GetById(int id)
        {
            return ownerRepository.GetById(id);
        }
        public Owner GetOwner()
        {
            return ownerRepository.GetOwnerByUsername("Milivoje");
        }

        public void Update(Owner owner)
        {
            ownerRepository.Update(owner);
        }

    }
}
