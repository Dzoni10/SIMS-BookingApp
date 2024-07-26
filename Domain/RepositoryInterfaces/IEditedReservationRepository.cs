using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IEditedReservationRepository
    {
        public void Save(EditedReservation editedReservation);
        public void SaveAll(List<EditedReservation> editedReservations);
        public void Update(EditedReservation editedReservation);
        public EditedReservation GetById(int id);
        public List<EditedReservation> GetAll();
        public void Delete(EditedReservation editedReservations);

    }
}
