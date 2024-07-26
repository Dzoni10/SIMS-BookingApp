using BookingApp.Domain.Models;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface INotificationRepository
    {
        public Notification Save(Notification notification);
        public void SaveAll(List<Notification> notifications);
        public void Update(Notification notifications);
        public Notification GetById(int id);
        public List<Notification> GetAll();
    }
}
