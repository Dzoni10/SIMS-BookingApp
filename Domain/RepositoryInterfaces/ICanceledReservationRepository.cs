using BookingApp.Domain.Models;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ICanceledReservationRepository
    {
        public List<CanceledReservation> GetAll();
        public CanceledReservation Save(CanceledReservation canceledReservation);
        public void Delete(CanceledReservation canceledReservation);
        public CanceledReservation Update(CanceledReservation canceledReservation);
        public CanceledReservation GetById(int id);
    }
}
