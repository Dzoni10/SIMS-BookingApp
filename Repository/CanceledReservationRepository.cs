using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class CanceledReservationRepository : ICanceledReservationRepository
    {
        Repository<CanceledReservation> canceledReservationRepository;
        public CanceledReservationRepository()
        {
            canceledReservationRepository = new Repository<CanceledReservation>();
        }
        public List<CanceledReservation> GetAll()
        {
            return canceledReservationRepository.GetAll();
        }
        public CanceledReservation Save(CanceledReservation canceledReservation)
        {
            canceledReservationRepository.Save(canceledReservation);
            return canceledReservation;
        }
        public void Delete(CanceledReservation canceledReservation)
        {
            canceledReservationRepository.Delete(canceledReservation);
        }
        public CanceledReservation Update(CanceledReservation canceledReservation)
        {
            canceledReservationRepository.Update(canceledReservation);
            return canceledReservation;
        }
        public CanceledReservation GetById(int id)
        {
            return canceledReservationRepository.FindId(id);
        }
    }
}
