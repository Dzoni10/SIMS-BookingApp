using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;
using BookingApp.Model;
using System.Globalization;

public enum RateStatus { Waiting, Rated, NoRate };

namespace BookingApp.Domain.Models
{
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public int LocationId { get; set; }
        public int GuestCount { get; set; }
        public int DaysReserved { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int GuestId { get; set; }
        public Guest Guest { get; set; }
        public Accommodation Accommodation { get; set; }
        public RateStatus GuestRateStatus { get; set; }
        public RateStatus OwnerRateStatus { get; set; }
        public AccommodationReservation() { }
        public AccommodationReservation(int accommodationId, int locationId, int guestId, int guestCount, int daysReserved, DateTime startDate, DateTime endDate, RateStatus guestRateStatus, RateStatus ownerRateStatus )
        {
            AccommodationId = accommodationId;
            LocationId = locationId;
            GuestId = guestId;
            GuestCount = guestCount;
            DaysReserved = daysReserved;
            StartDate = startDate;
            EndDate = endDate;
            GuestRateStatus = guestRateStatus;
            OwnerRateStatus = ownerRateStatus;
        }
        public AccommodationReservation(int id, int accommodationId, int locationId, int guestId, int guestCount, int daysReserved, DateTime startDate, DateTime endDate, RateStatus guestRateStatus, RateStatus ownerRateStatus)
        {
            Id = id;
            AccommodationId = accommodationId;
            LocationId = locationId;
            GuestId = guestId;
            GuestCount = guestCount;
            DaysReserved = daysReserved;
            StartDate = startDate;
            EndDate = endDate;
            GuestRateStatus = guestRateStatus;
            OwnerRateStatus = ownerRateStatus;
            Guest = new Guest();
            Accommodation = new Accommodation();
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                LocationId.ToString(),
                GuestId.ToString(),
                GuestCount.ToString(),
                DaysReserved.ToString(),
                StartDate.ToString(),
                EndDate.ToString(),
                GuestRateStatus.ToString(),
                OwnerRateStatus.ToString()
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AccommodationId = int.Parse(values[1]);
            LocationId = int.Parse(values[2]);
            GuestId = int.Parse(values[3]);
            GuestCount = int.Parse(values[4]);
            DaysReserved = int.Parse(values[5]);
            StartDate = DateTime.Parse(values[6]);
            EndDate = DateTime.Parse(values[7]);
            GuestRateStatus = (RateStatus)Enum.Parse(typeof(RateStatus), values[8]);
            OwnerRateStatus = (RateStatus)Enum.Parse(typeof(RateStatus), values[9]);
        }
    }
}
