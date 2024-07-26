using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public enum ComplexTourRequestStatus { Pending_request, Invalid, Accepted };
    public class ComplexTourRequest : ISerializable
    {
        public int Id { get; set; }
        public int TouristId {get; set;}
        public int Capacity { get; set; }
        public ComplexTourRequestStatus Status { get; set; }
        public DateOnly StartingDate { get; set; }

        public ComplexTourRequest()
        {

        }
        public ComplexTourRequest(int capacity, int touristId)
        {
            Capacity = capacity;
            Status = ComplexTourRequestStatus.Pending_request;
            StartingDate = DateOnly.FromDateTime(DateTime.Now);
            TouristId = touristId;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Capacity = Convert.ToInt32(values[1]);
            TouristId = Convert.ToInt32(values[2]);
            Status = (ComplexTourRequestStatus)Enum.Parse(typeof(ComplexTourRequestStatus), values[3]);
            StartingDate = DateOnly.Parse(values[4]);
        }

        public string[] ToCSV()
        {
            string[] values =
            {
                Id.ToString(),
                Capacity.ToString(),
                TouristId.ToString(),
                Status.ToString(),
                StartingDate.ToString()
            };
            return values;
        }
    }
}
