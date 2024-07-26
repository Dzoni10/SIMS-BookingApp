using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class CanceledReservationService
    {
        private ICanceledReservationRepository canceledReservationRepository;
        public CanceledReservationService()
        {
            canceledReservationRepository = Injector.Injector.CreateInstance<ICanceledReservationRepository>();
        }
        public CanceledReservation Save(CanceledReservation canceledReservation)
        {
            return canceledReservationRepository.Save(canceledReservation);
        }
        public List<CanceledReservation> GetAll()
        {
            return canceledReservationRepository.GetAll();
        }
    }
}
