using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class TourRequestGuest : ISerializable
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public int TourRequestReservationId { get; set; }
        public int ComplexTourRequestId { get; set; }

        public TourRequestGuest() { }

        public TourRequestGuest(string fullName, int age, int tourRequestReservationId, int complexTourRequestId)
        {
            FullName = fullName;
            Age = age;
            TourRequestReservationId = tourRequestReservationId;
            ComplexTourRequestId = complexTourRequestId;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            FullName = values[1];
            Age = Convert.ToInt32(values[2]);
            TourRequestReservationId = Convert.ToInt32(values[3]);
            ComplexTourRequestId = Convert.ToInt32(values[4]);
        }

        public string[] ToCSV()
        {
            string[] values = {
                Id.ToString(),
                FullName,
                Age.ToString(),
                TourRequestReservationId.ToString(),
                ComplexTourRequestId.ToString()
            };
            return values;
        }
    }
}
