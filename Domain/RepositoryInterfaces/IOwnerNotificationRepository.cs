using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IOwnerNotificationRepository
    {
        public List<OwnerNotification> GetAll();
        public void Delete(OwnerNotification ownerNotification);
        public OwnerNotification Save(OwnerNotification ownerNotification);
        public void SaveAll(List<OwnerNotification> ownerNotifications);
        public void Update(OwnerNotification ownerNotification);
        public OwnerNotification GetById(int id);
        public int SavedOwnerNotificationId(OwnerNotification ownerNotification);
    }
}
