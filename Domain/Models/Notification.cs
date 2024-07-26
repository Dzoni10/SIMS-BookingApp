using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public enum NotificationType { Voucher, FinishedTour, AcceptedTourRequest, NewTour};
    public enum NotificationState { Read, New};
    public class Notification : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }

        public NotificationType Type { get; set; }

        public NotificationState Status { get; set; }

        public Notification() { }

        public Notification(int userId, string text, NotificationType type)
        {
            UserId = userId;
            Text = text;
            Type = type;
            Status = NotificationState.New;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), UserId.ToString(), Type.ToString(), Text, Status.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            Type = (NotificationType)Enum.Parse(typeof(NotificationType), values[2]);
            Text = values[3];
            Status = (NotificationState)Enum.Parse(typeof(NotificationState), values[4]);
        }
    }

}
