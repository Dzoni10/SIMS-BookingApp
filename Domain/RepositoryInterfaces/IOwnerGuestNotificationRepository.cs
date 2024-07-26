using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IOwnerGuestNotificationRepository
    {
        public void Save(OwnerGuestNotification ownerGuestNotification);
        public void SaveAll(List<OwnerGuestNotification> ownerGuestNotifications);
        public void Update(OwnerGuestNotification ownerGuestNotification);
        public OwnerGuestNotification GetById(int id);
        public List<OwnerGuestNotification> GetAll();
        public void Delete(OwnerGuestNotification ownerGuestNotification);
    }
}
