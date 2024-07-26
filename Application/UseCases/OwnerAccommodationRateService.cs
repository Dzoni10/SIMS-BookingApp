using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class OwnerAccommodationRateService
    {
        public IOwnerAccommodationRateRepository ownerAccommodationRateRepository { get; set; }

        public OwnerAccommodationRateService()
        {
            ownerAccommodationRateRepository = Injector.Injector.CreateInstance<IOwnerAccommodationRateRepository>();
        }
        public void Save(OwnerAccommodationRate ownerAccommodationRate)
        {
            ownerAccommodationRateRepository.Save(ownerAccommodationRate);
        }
        public void SaveAll(List<OwnerAccommodationRate> ownerAccommodationRates)
        {
            ownerAccommodationRateRepository.SaveAll(ownerAccommodationRates);
        }
        public void Update(OwnerAccommodationRate ownerAccommodationRate)
        {
            ownerAccommodationRateRepository.Update(ownerAccommodationRate);
        }
        public OwnerAccommodationRate GetById(int id)
        {
            return ownerAccommodationRateRepository.GetById(id);
        }
        public List<OwnerAccommodationRate> GetAll()
        {
            return ownerAccommodationRateRepository.GetAll();
        }
        public string CalculateRateDaysLeft(AccommodationReservation reservation)
        {
            if (reservation.OwnerRateStatus == RateStatus.Waiting)
            {
                return (5 - DateTime.Now.Subtract(reservation.EndDate).Days) + "  days leff";
            }
            else if (reservation.OwnerRateStatus == RateStatus.NoRate)
            {
                return "you are late";
            }
            else
            {
                return "rated";
            }
        }
        public double GetAverageGrade(int accommodationId)
        {
            double rates = 0;
            int ratesCount = 0;

            foreach(OwnerAccommodationRate accommodationRate in ownerAccommodationRateRepository.GetAll())
            {
                if(accommodationRate.AccommodationId == accommodationId)
                {
                    rates += accommodationRate.CleanlinessGrade + accommodationRate.OwnerHospitality;
                    ratesCount += 2;
                }
            }
            if(ratesCount > 0)
            {
                return rates / ratesCount;
            }
            else
            {
                return 0;
            }
        }
        public OwnerAccommodationRate GetRateByReservationId(int reservationId)
        {
            foreach(OwnerAccommodationRate rate in ownerAccommodationRateRepository.GetAll())
            {
                if(rate.AccommodationReservationId == reservationId)
                {
                    return rate;
                }
            }
            return null;
        }
    }
}
