using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
public enum AccommodationType { House=1, Cottage, Apartment };

namespace BookingApp.Domain.Models
{
    public class Accommodation : ISerializable
    {

        public int Id { get; set; }

        public string Name
        {
            get;
            set;
        }

        public Location Location
        {
            get;
            set;
        }

        public AccommodationType Type
        {

            get;
            set;
        }

        public int Capacity
        {
            get;
            set;
        }

        public int MinStayDays
        {
            get;
            set;
        }

        public int CancelationPeriod
        {
            get;
            set;
        }

        public int LocationId
        {
            get;
            set;
        }

        public Image Image
        {
            get;
            set;
        }

        public bool IsClosed
        {
            get;
            set;
        }

        public Accommodation() { }

        public Accommodation(string name, AccommodationType type, int capacity, int minStayDays, int cancelationPeriod, int locationId,bool isClosed)
        {
            Name = name;
            Type = type;
            Capacity = capacity;
            MinStayDays = minStayDays;
            CancelationPeriod = cancelationPeriod;
            LocationId = locationId;
            IsClosed = isClosed;
            Image = new Image();
            Location = new Location();
        }
        public Accommodation(int id, string name, AccommodationType type, int capacity, int minStayDays, int cancelationPeriod, int locationId,bool isCLosed)
        {
            Id = id;
            Name = name;
            Type = type;
            Capacity = capacity;
            MinStayDays = minStayDays;
            CancelationPeriod = cancelationPeriod;
            LocationId = locationId;
            IsClosed = isCLosed;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                Type.ToString(),
                Capacity.ToString(),
                CancelationPeriod.ToString(),
                MinStayDays.ToString(),
                LocationId.ToString(),
                IsClosed.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            Type = (AccommodationType)Enum.Parse(typeof(AccommodationType), values[2]);
            Capacity = int.Parse(values[3]);
            CancelationPeriod = int.Parse(values[4]);
            MinStayDays = int.Parse(values[5]);
            LocationId = int.Parse(values[6]);
            IsClosed = bool.Parse(values[7]);
        }
    }
}
