using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public enum CreatorType { Guest, Owner };

    public class ForumComment : ISerializable
    {
        public int Id { get; set; }
        public int ForumId { get; set; }
        public int GuestId { get; set; }
        public int OwnerId { get; set; }
        public int LocationId { get; set; }
        public string Comment { get; set; }
        public CreatorType CreatorType { get; set; }
        public string Reported { get; set; }

        public ForumComment() { }
        public ForumComment(int forumId, int guestId, int ownerId, int locationId, string comment, CreatorType creatorType, string reported)
        {
            ForumId = forumId;
            GuestId = guestId;
            OwnerId = ownerId;
            LocationId = locationId;
            Comment = comment;
            CreatorType = creatorType;
            Reported = reported;
        }
        public ForumComment(int id, int forumId, int guestId, int ownerId, int locationId, string comment, CreatorType creatorType, string reported)
        {
            Id = id;
            ForumId = forumId;
            GuestId = guestId;
            OwnerId = ownerId;
            LocationId = locationId;
            Comment = comment;
            CreatorType = creatorType;
            Reported = reported;
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                ForumId.ToString(),
                GuestId.ToString(),
                OwnerId.ToString(),
                LocationId.ToString(),
                Comment,
                CreatorType.ToString(),
                Reported.ToString()
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            ForumId = int.Parse(values[1]);
            GuestId = int.Parse(values[2]);
            OwnerId = int.Parse(values[3]);
            LocationId = int.Parse(values[4]);
            Comment = values[5];
            CreatorType = (CreatorType)Enum.Parse(typeof(CreatorType), values[6]);
            Reported = values[7];
        }
    }
}
