﻿using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class CanceledReservation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationReservationId { get; set; }
        public int AccommodationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CanceledReservation()
        {

        }
        public CanceledReservation(int accommodationReservationId, int accommodationId, DateTime startDate, DateTime endDate)
        {
            AccommodationReservationId = accommodationReservationId;
            AccommodationId = accommodationId;
            StartDate = startDate;
            EndDate = endDate;
        }
        public string[] ToCSV()
        {
            string[] csvValues = {
                Id.ToString(),
                AccommodationReservationId.ToString(),
                AccommodationId.ToString(),
                StartDate.ToString(),
                EndDate.ToString()
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationReservationId = Convert.ToInt32(values[1]);
            AccommodationId = Convert.ToInt32(values[2]);
            StartDate = Convert.ToDateTime(values[3]);
            EndDate = Convert.ToDateTime(values[4]);
        }
    }
}
