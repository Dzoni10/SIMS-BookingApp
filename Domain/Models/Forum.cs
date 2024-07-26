using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookingApp.Domain.Models
{
    public class Forum : ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public Guest Guest { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string Comment { get; set; }
        public string Report { get; set; }
        public bool Open { get; set; }

        public Forum() { }

        public Forum(int guestId,int locationId, string comment, string report, bool open)
        {
            
            GuestId = guestId;
            LocationId = locationId;
            Comment = comment;
            Report = report;
            Location = new Location();
            Guest = new Guest();
            Open = open;
        }

        public Forum(int id,int guestId,int locationId,string comment,string report, bool open)
        {
            Id = id;
            GuestId = guestId;
            LocationId = locationId;
            Comment = comment;
            Report = report;
            Open = open;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                GuestId.ToString(),
                LocationId.ToString(),
                Comment,
                Report,
                Open.ToString()
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            GuestId= int.Parse(values[1]);
            LocationId = int.Parse(values[2]);
            Comment = values[3];
            Report = values[4];
            Open = bool.Parse(values[5]);
        }

    }
}
