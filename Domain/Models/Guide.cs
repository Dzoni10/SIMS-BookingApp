using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class Guide : ISerializable
    {
        public int Id { get; set; }
        public int GuideId { get; set; }
        public bool IsSuperGuide { get; set; }
        public string Language { get; set; }


        public Guide()
        {
        }
        public Guide(int guideId, string language)
        {
            GuideId = guideId;
            IsSuperGuide = false;
            Language = language;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuideId = Convert.ToInt32(values[1]);
            IsSuperGuide = Convert.ToBoolean(values[2]);
            Language = values[3];
        }
        public string[] ToCSV()
        {
            string[] values =
            {
                Id.ToString(),
                GuideId.ToString(),
                IsSuperGuide.ToString(),
                Language
            };
            return values;
        }
    }
}
