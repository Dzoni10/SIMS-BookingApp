using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ReservationStatus { Accepted, Pending, Rejected };

namespace BookingApp.Domain.Models
{
    public class EditedReservation : ISerializable
    {
        public int Id { get; set; }

        public int AccommodationReservationId { get; set; }
        public int GuestId { get; set; }
        public int AccommodationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ReservationStatus Status { get; set; }
        public Guest Guest { get; set; }
        public Accommodation Accommodation { get; set; }
        public EditedReservation()
        {

        }
        public EditedReservation(int accommodationReservationId, DateTime newStartDate, DateTime newEndDate, ReservationStatus status)
        {
            AccommodationReservationId = accommodationReservationId;
            StartDate = newStartDate;
            EndDate = newEndDate;
            Status = status;
            Accommodation = new Accommodation();
            Guest = new Guest();
        }
        public EditedReservation(int id, int accommodationReservationId, DateTime newStartDate, DateTime newEndDate, ReservationStatus status)
        {
            Id = id;
            AccommodationReservationId = accommodationReservationId;
            StartDate = newStartDate;
            EndDate = newEndDate;
            Status = status;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationReservationId.ToString(),
                AccommodationId.ToString(),
                GuestId.ToString(),
                StartDate.ToString(),
                EndDate.ToString(),
                Status.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AccommodationReservationId = int.Parse(values[1]);
            AccommodationId = int.Parse(values[2]);
            GuestId = int.Parse(values[3]);
            StartDate = DateTime.Parse(values[4]);
            EndDate = DateTime.Parse(values[5]);
            Status = (ReservationStatus)Enum.Parse(typeof(ReservationStatus), values[6]);
        }
    }
}
