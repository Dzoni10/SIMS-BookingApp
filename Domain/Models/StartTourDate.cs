using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public enum TourStatus {Created=0, Reserved, Active, Finished, Cancelled, Request};
    public class StartTourDate : ISerializable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int TourId { get; set; }
        public int Capacity { get; set; }
        public TourStatus Status { get; set; }
        public int CurrentCheckpoint { get; set; }
        
        public StartTourDate() { }

        public StartTourDate(DateTime date, int tourId, int capacity)
        {
            Date = date;
            TourId = tourId;
            Capacity = capacity;
            Status = TourStatus.Created;
            CurrentCheckpoint = 0;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Date = Convert.ToDateTime(values[1]);
            TourId = Convert.ToInt32(values[2]);
            Capacity = Convert.ToInt32(values[3]);
            Status = (TourStatus)Enum.Parse(typeof(TourStatus), values[4]);
            CurrentCheckpoint = Convert.ToInt32(values[5]);
        }

        public string[] ToCSV()
        {
            string[] values = 
            {
                Id.ToString(),
                Date.ToString(),
                TourId.ToString(),
                Capacity.ToString(),
                Status.ToString(),
                CurrentCheckpoint.ToString()
            };
            return values;
        }
    }
}
