using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum EntityType { a, t, ra, rt }; /* a - Accommodation, t - Tour, ra - Rate Accommodation, rt - Rate Tour */

namespace BookingApp.Domain.Models
{

    public class Image : ISerializable
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int EntityId { get; set; }
        public EntityType EntityType { get; set; }
        public Image() { }
        public Image(string path, int entityId, EntityType entityType)
        {
            Path = path;
            EntityId = entityId;
            EntityType = entityType;
        }

        public Image(int id, string path, int entityId, EntityType entityType)
        {
            Id = id;
            Path = path;
            EntityId = entityId;
            EntityType = entityType;
        }


        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Path = values[1];
            EntityId = Convert.ToInt32(values[2]);
            try
            {
                EntityType = (EntityType)Enum.Parse(typeof(EntityType), values[3]);

            }
            catch (ArgumentException)
            {
                /* Parsiranje nije uspjesno. Doslo je do izuzetka. */
            }
        }

        public string[] ToCSV()
        {
            string[] values =
            {
                Id.ToString(),
                Path,
                EntityId.ToString(),
                EntityType.ToString()
            };
            return values;
        }
    }
}
