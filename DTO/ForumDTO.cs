using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace BookingApp.DTO
{
    public class ForumDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if(name !=value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        private int guestId;
        public int GuestId
        {
            get { return guestId; }
            set
            {
                if(guestId != value)
                {
                    guestId = value;
                    OnPropertyChanged("GuestId");
                }
            }
        }
        private Guest guest;
        public Guest Guest
        {
            get { return guest; }
            set
            {
                if( guest != value)
                {
                    guest = value;
                    OnPropertyChanged("Guest");
                }
            }
        }
        private int locationId;
        public int LocationId
        {
            get { return locationId; }
            set
            {
                if(locationId != value)
                {
                    locationId = value;
                    OnPropertyChanged("LocationId");
                }
            }
        }
        private Location location;
        public Location Location
        {
            get { return location; }
            set
            {
                if(location !=value)
                {
                    location = value;
                    OnPropertyChanged("Location");
                }
            }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                if (comment != value)
                {
                    comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }
        private string report;
        public string Report
        {
            get { return report; }
            set
            {
                if(report != value)
                {
                    report = value;
                    OnPropertyChanged("Report");
                }
            }
        }
        private bool open;
        public bool Open
        {
            get { return open; }
            set
            {
                if(open != value)
                {
                    open = value;
                    OnPropertyChanged("Open");
                }
            }
        }
        private bool openAndCreator;
        public bool OpenAndCreator
        {
            get { return openAndCreator; }
            set
            {
                if (openAndCreator != value)
                {
                    openAndCreator = value;
                    OnPropertyChanged("OpenAndCreator");
                }
            }
        }

        public ForumDTO() { }
        public ForumDTO(int guestId, string name, int locationId,string comment, string report, bool open)
        {
            this.name = name;
            this.guestId = guestId;
            this.locationId = locationId;
            this.comment = comment;
            this.report = report;
            location = new Location();
            guest = new Guest();
            this.open = open;
        }

        public ForumDTO(int id, int guestId, string name, int locationId,string comment,string report, bool open)
        {
            Id = id;
            this.name = name;
            this.guestId = guestId;
            this.locationId = locationId;
            this.comment = comment;
            this.report = report;
            this.open = open;
        }

        public ForumDTO(Forum forum)
        {
            Id = forum.Id;
            this.guestId = forum.GuestId;
            this.locationId = forum.LocationId;
            this.comment = forum.Comment;
            this.report = forum.Report;
            this.open = forum.Open;
        }
        public Forum ToForum()
        {
            return new Forum(Id, guestId, locationId, comment, report, open);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
