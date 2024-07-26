using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public enum TourRequestStatus {Pending_request, Invalid, Accepted};
    public class TourRequest : ISerializable
    {
        public int Id { get; set; }
        public User RequestingUser { get; set; }
        public TourRequestStatus Status { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public Language Language { get; set; }
        public DateOnly DateFrom { get; set; }
        public DateOnly DateTo { get; set; }
        public int Capacity {  get; set; }
        public int GuideId {  get; set; }

        public int ComplexTourRequestId { get; set; }

        public DateTime DateIfAccepted { get; set; }

        public TourRequest() { }

        public TourRequest(User requestingUser, Location location, string description, Language language, DateOnly dateFrom, DateOnly dateTo, int capacity, int guideId, int complexTourRequestId)
        {
            RequestingUser = requestingUser;
            Status = TourRequestStatus.Pending_request;
            Location = location;
            Description = description;
            Language = language;
            DateFrom = dateFrom;
            DateTo = dateTo;
            Capacity = capacity;
            GuideId = guideId;
            DateIfAccepted = DateTime.MinValue;
            ComplexTourRequestId = complexTourRequestId;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserRepository userRepository = new UserRepository();
            RequestingUser = userRepository.GetById(Convert.ToInt32(values[1]));
            Status = (TourRequestStatus)Enum.Parse(typeof(TourRequestStatus), values[2]);
            LocationRepository locationRepository = new LocationRepository();
            Location = locationRepository.GetById(Convert.ToInt32(values[3]));
            Description = values[4];
            LanguageRepository languageRepository = new LanguageRepository();
            Language = languageRepository.GetById(Convert.ToInt32(values[5]));
            DateFrom = DateOnly.Parse(values[6]);
            DateTo = DateOnly.Parse(values[7]);
            Capacity = Convert.ToInt32(values[8]);
            GuideId = Convert.ToInt32(values[9]);
            DateIfAccepted = DateTime.Parse(values[10]);
            ComplexTourRequestId = Convert.ToInt32(values[11]);
        }

        public string[] ToCSV()
        {
            string[] values =
            {
                Id.ToString(),
                RequestingUser.Id.ToString(),
                Status.ToString(),
                Location.Id.ToString(),
                Description,
                Language.Id.ToString(),
                DateFrom.ToString(),
                DateTo.ToString(),
                Capacity.ToString(),
                GuideId.ToString(),
                DateIfAccepted.ToString(),
                ComplexTourRequestId.ToString()
            };
            return values;
        }
    }
}
