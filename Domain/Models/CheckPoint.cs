using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Checkpoint : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TourId { get; set; }

        public Checkpoint() { }

        public Checkpoint(string name, int tourId)
        {
            Name = name;
            TourId = tourId;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            TourId= Convert.ToInt32(values[2]);
        }

        public string[] ToCSV()
        {
            string[] values =
            {
                Id.ToString(), 
                Name,
                TourId.ToString()
            };
            return values;
        }
    }
}
