using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum NotificationStatus { Deleted, Error };

namespace BookingApp.Domain.Models
{
    public class OwnerNotification : ISerializable
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public NotificationStatus Status { get; set; } 

        public OwnerNotification()
        {

        }
        public OwnerNotification(int id, int reservationId, NotificationStatus status)
        {
            Id = id;
            ReservationId = reservationId;
            Status = status;
        }
        public OwnerNotification(int reservationId, NotificationStatus status)
        {
            ReservationId = reservationId;
            Status = status;
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                ReservationId.ToString(),
                Status.ToString()
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            ReservationId = int.Parse(values[1]);
            if (values[2] == "Deleted")
            {
                Status = NotificationStatus.Deleted;
            }
            else
            {
                Status = NotificationStatus.Error;
            }
        }
    }
}
