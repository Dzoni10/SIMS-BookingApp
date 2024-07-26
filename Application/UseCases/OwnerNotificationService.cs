using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class OwnerNotificationService
    {
        private IOwnerNotificationRepository ownerNotificationRepository { get; set; }
        public OwnerNotificationService()
        {
            ownerNotificationRepository = Injector.Injector.CreateInstance<IOwnerNotificationRepository>();
        }
        public List<OwnerNotification> GetAll()
        {
            return ownerNotificationRepository.GetAll();
        }
        public void Delete(OwnerNotification ownerNotification)
        {
            ownerNotificationRepository.Delete(ownerNotification);
        }
        public OwnerNotification Save(OwnerNotification ownerNotification)
        {
            return ownerNotificationRepository.Save(ownerNotification);
        }
        public void SaveAll(List<OwnerNotification> ownerNotifications)
        {
            ownerNotificationRepository.SaveAll(ownerNotifications);
        }
        public void Update(OwnerNotification ownerNotification)
        {
            ownerNotificationRepository.Update(ownerNotification);
        }
        public OwnerNotification GetById(int id)
        {
            return ownerNotificationRepository.GetById(id);
        }
        public int SavedOwnerNotificationId(OwnerNotification ownerNotification)
        {
            return ownerNotificationRepository.SavedOwnerNotificationId(ownerNotification);
        }

    }
}
