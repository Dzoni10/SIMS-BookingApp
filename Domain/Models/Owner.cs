using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;

namespace BookingApp.Domain.Models
{
    public class Owner : User, ISerializable
    {
        public int Id { get; set; }

        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }


        public string Surname
        {
            get;
            set;
        }
        public bool IsSuperOwner
        {
            get;
            set;
        }

        public double AverageGrade
        {
            get;
            set;
        }

        public Owner()
        {

        }
        public Owner(string name, string surname)
        {
            Name = name;
            Surname = surname;
            AverageGrade = 0;
        }

        public Owner(string username, string password, string name, string surname, double averageGrade, bool isSuperOwner)
        {
            Username = username;
            Password = password;
            Name = name;
            Surname = surname;
            AverageGrade = averageGrade;
            IsSuperOwner = isSuperOwner;
        }

        public Owner(int id, string username, string password, string name, string surname, double averageGrade, bool isSuperOwner)
        {
            Id = id;
            Username = username;
            Password = password;
            Name = name;
            Surname = surname;
            AverageGrade = averageGrade;
            IsSuperOwner = isSuperOwner;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Username = values[1];
            Password = values[2];
            Name = values[3];
            Surname = values[4];
            AverageGrade = double.Parse(values[5]);
            IsSuperOwner = bool.Parse(values[6]);
        }


        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Username,
                Password,
                Name,
                Surname,
                AverageGrade.ToString(),
                IsSuperOwner.ToString()
            };
            return csvValues;
        }

    }
}
