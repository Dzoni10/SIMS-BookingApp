using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using BookingApp.Repository;
using BookingApp.Domain.Models;

namespace BookingApp.Model
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public Language Language { get; set; }
        public int Capacity {  get; set; }
        public double Duration { get; set; }

        public int UserId { get; set; }
        public Tour() { }

        public Tour(string name, Location location, string description, Language language, int capacity, double duration)
        {
            Name = name;
            Location = location;
            Description = description;
            Language = language;
            Capacity = capacity;
            Duration = duration;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            /* Location */
            LocationRepository locationRepository = new LocationRepository();
            Location = locationRepository.GetById(Convert.ToInt32(values[2]));

            Description = values[3];
            /* Language */
            LanguageRepository languageRepository = new LanguageRepository();
            Language = languageRepository.GetById(Convert.ToInt32(values[4]));

            Capacity = Convert.ToInt32(values[5]);
            
            Duration = Convert.ToDouble(values[6]);

            UserId = Convert.ToInt32(values[7]);
        }

        public string[] ToCSV()
        {
            string[] values =
            {
                Id.ToString(),
                Name,
                Location.Id.ToString(),
                Description,
                Language.Id.ToString(),
                Capacity.ToString(),
                Duration.ToString(),
                UserId.ToString()
            };
            return values;
        }
    }
}
