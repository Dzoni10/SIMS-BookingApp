using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class Ignore : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public int GuestId { get; set; }
        public int EditedReservationId { get; set; }
        public string Explanation { get; set; }

        public Ignore() { }

        public Ignore(int accommodationId, int guestId, int editedReservationId, string explanation)
        {
            AccommodationId = accommodationId;
            GuestId = guestId;
            EditedReservationId = editedReservationId;
            Explanation = explanation;
        }

        public Ignore(int id, int accommodationId, int guestId, int editedReservationId, string explanation)
        {
            Id = id;
            AccommodationId = accommodationId;
            GuestId = guestId;
            EditedReservationId = editedReservationId;
            Explanation = explanation;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                GuestId.ToString(),
                EditedReservationId.ToString(),
                Explanation
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AccommodationId = int.Parse(values[1]);
            GuestId = int.Parse(values[2]);
            EditedReservationId = int.Parse(values[3]);
            Explanation = values[4];
        }
    }
}
