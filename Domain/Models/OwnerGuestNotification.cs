using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;

namespace BookingApp.Domain.Models
{
    public class OwnerGuestNotification:ISerializable
    {
        public int Id { get; set; }
        public int EditedReservationId { get; set; }
        public ReservationStatus Status { get; set; }
        
        public OwnerGuestNotification() { }
        
        public OwnerGuestNotification(int editedReservationId,ReservationStatus status)
        {
            EditedReservationId = editedReservationId;
            Status = status;
        }

        public OwnerGuestNotification(int id,int editedReservationId, ReservationStatus status)
        {
            Id = id;
            EditedReservationId = editedReservationId;
            Status = status;
        }

        public string[] ToCSV()
        {
            string editedReservationStatus;
            if (Status == ReservationStatus.Accepted)
            {
                editedReservationStatus = "Accepted";
            }
            else if (Status == ReservationStatus.Pending)
            {
                editedReservationStatus = "Pending";
            }
            else
            {
                editedReservationStatus = "Rejected";
            }

            string[] csvValues =
            {
                Id.ToString(),
                EditedReservationId.ToString(),
                editedReservationStatus
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            EditedReservationId = int.Parse(values[1]);
            if (values[2] == "Accepted")
            {
                Status = ReservationStatus.Accepted;
            }
            else if (values[2] == "Pending")
            {
                Status = ReservationStatus.Pending;
            }
            else
            {
                Status = ReservationStatus.Rejected;
            }

        }


    }
}
