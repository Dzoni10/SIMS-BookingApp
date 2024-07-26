using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class TourRating : ISerializable
    {

        public int Id {  get; set; }

        public int GuidesKnowledge {  get; set; }

        public int GuidesLanguageAbility {  get; set; }

        public int TourExcitement {  get; set; }
        
        public string Comment { get; set; }

        public int UserId { get; set; }

        public int StartTourDateId { get; set; }
        public bool Valid { get; set; }
        public DateTime AssessmentDate { get; set; }

        public TourRating()
        {
            Valid= true;
            AssessmentDate = DateTime.Now;
        }
        public TourRating(int guidesKnowledge, int guidesLanguageAbility, int tourExcitement, string comment, int userId, int startTourDateId)
        {
            GuidesKnowledge = guidesKnowledge;
            GuidesLanguageAbility = guidesLanguageAbility;
            TourExcitement = tourExcitement;
            Comment = comment;
            UserId = userId;
            StartTourDateId = startTourDateId;
            Valid = true;
            AssessmentDate = DateTime.Now;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                    Id.ToString(),
                    UserId.ToString(),
                    StartTourDateId.ToString(),
                    GuidesKnowledge.ToString(),
                    GuidesLanguageAbility.ToString(),
                    TourExcitement.ToString(),
                    Comment,
                    Valid.ToString(),
                    AssessmentDate.ToString()
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            UserId = int.Parse(values[1]);
            StartTourDateId = int.Parse(values[2]);
            GuidesKnowledge = int.Parse(values[3]);
            GuidesLanguageAbility = int.Parse(values[4]);
            TourExcitement = int.Parse(values[5]);
            Comment = values[6];
            Valid = bool.Parse(values[7]);
            AssessmentDate = DateTime.Parse(values[8]);
        }
    }
}
