using BookingApp.Serializer;
using BookingApp.WPF.View;
using iText.StyledXmlParser.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class Renovation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public DateTime StartDate {  get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public Accommodation Accommodation { get; set; }

        public Renovation() { }

        public Renovation(int accommodationId,DateTime startDate,DateTime endDate,int duration,string description)
        {
            AccommodationId = accommodationId;
            StartDate = startDate;
            EndDate = endDate;
            Duration= duration;
            Description = description;
            Accommodation = new Accommodation();
        }
        public Renovation(int id,int accommodationId, DateTime startDate, DateTime endDate,int duration,string description)
        {
            Id = id;
            AccommodationId = accommodationId;
            StartDate = startDate;
            Duration = duration;
            Description = description;
            EndDate = endDate;
            
        }
        public string[] ToCSV()
        {
            
            
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                StartDate.ToString(),
                EndDate.ToString(),
                Duration.ToString(),
                Description

            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AccommodationId = int.Parse(values[1]);
            StartDate = DateTime.Parse(values[2]);
            EndDate = DateTime.Parse(values[3]);
            Duration = int.Parse(values[4]);
            Description = values[5];
            
        }



    }
}
