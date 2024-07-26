using BookingApp.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.DTO;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Repository
{
    public class EditedReservationRepository : IEditedReservationRepository
    {
        Repository<EditedReservation> editedReservationRepository;
        public Subject editedReservationSubject;

        public EditedReservationRepository()
        {
            editedReservationRepository = new Repository<EditedReservation>();
            editedReservationSubject = new Subject();
        }

        public void Save(EditedReservation editedReservation)
        {
            editedReservationRepository.Save(editedReservation);
            editedReservationSubject.NotifyObservers();
        }
        public void SaveAll(List<EditedReservation> editedReservations)
        {
            editedReservationRepository.SaveAll(editedReservations);
            editedReservationSubject.NotifyObservers();
        }
        public void Update(EditedReservation editedReservation)
        {
            editedReservationRepository.Update(editedReservation);
            editedReservationSubject.NotifyObservers();
        }
        public EditedReservation GetById(int id)
        {
            return editedReservationRepository.FindId(id);
        }
        public List<EditedReservation> GetAll()
        {
            return editedReservationRepository.GetAll();
        }
        public void Delete(EditedReservation editedReservations)
        {
            editedReservationRepository.Delete(editedReservations);
            editedReservationSubject.NotifyObservers();
        }
    }
}
