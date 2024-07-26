using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BookingApp.Serializer;

namespace BookingApp.Domain.Models
{
    public class GuestAccommodationRate : ISerializable
    {

        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public int GuestId { get; set; }
        public int AccommodationReservationId { get; set; }
        public double CleanlinessGrade { get; set; }
        public double RuleGrade { get; set; }
        public string Comment { get; set; }

        public GuestAccommodationRate()
        {

        }

        public GuestAccommodationRate(int accommodationId, int guestId,int accommodationReservationId, double cleanlinessGrade, double ruleGrade, string comment)
        {
            AccommodationId = accommodationId;
            GuestId = guestId;
            AccommodationReservationId = accommodationReservationId;
            CleanlinessGrade = cleanlinessGrade;
            RuleGrade = ruleGrade;
            Comment = comment;
        }

        public GuestAccommodationRate(int id, int accommodationId, int guestId,int accommodationReservationId ,double cleanlinessGrade, double ruleGrade, string comment)
        {
            Id = id;
            AccommodationId = accommodationId;
            GuestId = guestId;
            AccommodationReservationId = accommodationReservationId;
            CleanlinessGrade = cleanlinessGrade;
            RuleGrade = ruleGrade;
            Comment = comment;
        }


        public string[] ToCSV()
        {


            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                GuestId.ToString(),
                AccommodationReservationId.ToString(),
                CleanlinessGrade.ToString(),
                RuleGrade.ToString(),
                Comment

            };
            return csvValues;

        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AccommodationId = int.Parse(values[1]);
            GuestId = int.Parse(values[2]);
            AccommodationReservationId = int.Parse(values[3]);
            CleanlinessGrade = double.Parse(values[4]);
            RuleGrade = double.Parse(values[5]);
            Comment = values[6];

        }


    }
}
