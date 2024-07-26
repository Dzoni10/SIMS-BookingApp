using BookingApp.Domain.Models;
using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class TourReservation : ISerializable
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int StartTourDateId { get; set; }

        public TourReservation()
        {

        }
        public TourReservation(User user, int startTourDateId)
        {
            User = user;
            StartTourDateId = startTourDateId;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            /* User */
            UserRepository userRepository = new UserRepository();
            User = userRepository.GetById(Convert.ToInt32(values[1]));
            StartTourDateId = Convert.ToInt32(values[2]);
        }

        public string[] ToCSV()
        {
            String[] values = 
            { 
                Id.ToString(),
                User.Id.ToString(),
                StartTourDateId.ToString() 
            };
            return values;
        }
    }
}
