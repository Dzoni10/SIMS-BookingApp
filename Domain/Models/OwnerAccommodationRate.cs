using BookingApp.Serializer;
using iText.StyledXmlParser.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class OwnerAccommodationRate : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationReservationId { get; set; }
        public int AccommodationId { get; set; }
        public int GuestId { get; set; }
        public double CleanlinessGrade { get; set; }
        public double OwnerHospitality { get; set; }
        public string AdditionalComment { get; set; }
        public Accommodation Accommodation { get; set; }

        public Guest Guest { get; set; }

        public OwnerAccommodationRate()
        {

        }
        public OwnerAccommodationRate(int accommodationReservationId, int accommodationId, int guestId, double cleanlinessGrade, double ownerHospitality, string additionalComment)
        {
            AccommodationReservationId = accommodationReservationId;
            AccommodationId = accommodationId;
            GuestId = guestId;
            CleanlinessGrade = cleanlinessGrade;
            OwnerHospitality = ownerHospitality;
            AdditionalComment = additionalComment;
        }
        public OwnerAccommodationRate(int accommodationReservationId, int id, int accommodationId, int guestId, double cleanlinessGrade, double ownerHospitality, string additionalComment)
        {
            Id = id;
            AccommodationReservationId = accommodationReservationId;
            AccommodationId = accommodationId;
            GuestId = guestId;
            CleanlinessGrade = cleanlinessGrade;
            OwnerHospitality = ownerHospitality;
            AdditionalComment = additionalComment;
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AccommodationReservationId = int.Parse(values[1]);
            AccommodationId = int.Parse(values[2]);
            GuestId = int.Parse(values[3]);
            CleanlinessGrade = double.Parse(values[4]);
            OwnerHospitality = double.Parse(values[5]);
            AdditionalComment = values[6];
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationReservationId.ToString(),
                AccommodationId.ToString(),
                GuestId.ToString(),
                CleanlinessGrade.ToString(),
                OwnerHospitality.ToString(),
                AdditionalComment,
            };
            return csvValues;

        }
    }
}
