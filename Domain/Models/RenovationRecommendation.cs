using BookingApp.Serializer;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class RenovationRecommendation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public int AccommodationReservationId { get; set; }
        public string RenovationComment { get; set; }
        public double RenovationEmergencyGrade { get; set; }
        public DateTime RateDate { get; set; }

        public RenovationRecommendation() { }

        public RenovationRecommendation(int accommodationId, int reservationId, string renovationComment, double renovationEmergencyGrade,DateTime rateDate)
        {
            AccommodationId = accommodationId;
            AccommodationReservationId = reservationId;
            RenovationComment = renovationComment;
            RenovationEmergencyGrade = renovationEmergencyGrade;
            RateDate = rateDate;
        }
        public RenovationRecommendation(int Id, int accommodationId, int reservationId, string renovationComment, double renovationEmergencyGrade,DateTime rateDate)
        {
            Id = Id;
            AccommodationId = accommodationId;
            AccommodationReservationId = reservationId;
            RenovationComment = renovationComment;
            RenovationEmergencyGrade = renovationEmergencyGrade;
            RateDate = rateDate;
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AccommodationId = int.Parse(values[1]);
            AccommodationReservationId = int.Parse(values[2]);
            RenovationComment = values[3];
            RenovationEmergencyGrade = double.Parse(values[4]);
            RateDate = DateTime.Parse(values[5]);
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                AccommodationReservationId.ToString(),
                RenovationComment,
                RenovationEmergencyGrade.ToString(),
                RateDate.ToString()
            };
            return csvValues;

        }
    }
}
