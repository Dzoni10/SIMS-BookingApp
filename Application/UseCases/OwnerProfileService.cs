using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Application.Injector;

namespace BookingApp.Application.UseCases
{
    public class OwnerProfileService
    {
        private IOwnerAccommodationRateRepository ownerAccommodationRateRepository;
        public OwnerService ownerService;

        public OwnerProfileService() 
        { 
            ownerAccommodationRateRepository = Injector.Injector.CreateInstance<IOwnerAccommodationRateRepository>();
            ownerService = new OwnerService();
        }

        public double CalculateOwnerAverageRate()
        {
            int countOwnerRates = 0;
            double rateSum = 0;
            foreach (OwnerAccommodationRate ownerAccommodationRate in ownerAccommodationRateRepository.GetAll())
            {
                countOwnerRates++;
                rateSum += ownerAccommodationRate.OwnerHospitality;
            }
            return Math.Round( rateSum / countOwnerRates, 2);
        }

        public Owner GetAccommodationOwner()
        {
            return ownerService.GetOwner();
        }
        public void UpdateOwner(Owner owner)
        {
            ownerService.Update(owner);
        }
    }
}