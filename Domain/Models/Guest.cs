using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class Guest : User, ISerializable
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool SuperGuestStatus { get; set; }
        public int Points { get; set; }
        public DateTime SuperGuestExpirationDate { get; set; }

        public Guest() { }
        public Guest(int userId, string name, string surname, bool superGuestStatus, int points, DateTime superGuestExpirationDate)
        {
            UserId = userId;
            Name = name;
            Surname = surname;
            SuperGuestStatus = superGuestStatus;
            Points = points;
            SuperGuestExpirationDate = superGuestExpirationDate;
        }
        public Guest(int id, int userId, string name, string surname, bool superGuestStatus, int points, DateTime superGuestExpirationDate)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Surname = surname;
            SuperGuestStatus = superGuestStatus;
            Points = points;
            SuperGuestExpirationDate = superGuestExpirationDate;
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            UserId = int.Parse(values[1]);
            Name = values[2];
            Surname = values[3];
            SuperGuestStatus = bool.Parse(values[4]);
            Points = int.Parse(values[5]);
            SuperGuestExpirationDate = DateTime.Parse(values[6]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                UserId.ToString(),
                Name,
                Surname,
                SuperGuestStatus.ToString(),
                Points.ToString(),
                SuperGuestExpirationDate.ToString()
            };
            return csvValues;
        }
    }
}
