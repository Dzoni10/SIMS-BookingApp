using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class ForumCommentDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private int forumId;
        public int ForumId
        {
            get { return forumId; }
            set
            {
                if (forumId != value)
                {
                    forumId = value;
                    OnPropertyChanged("ForumId");
                }
            }
        }

        private int guestId;
        public int GuestId
        {
            get { return guestId; }
            set
            {
                if (guestId != value)
                {
                    guestId = value;
                    OnPropertyChanged("GuestId");
                }
            }
        }
        private string author;
        public string Author
        {
            get { return author; }
            set
            {
                if (author != value)
                {
                    author = value;
                    OnPropertyChanged("Author");
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
            get { return report;}
            set
            {
                if (report != value)
                {
                    report = value;
                    OnPropertyChanged("Report");
                }
            }
        }

        private string city;
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                if(value != city)
                {
                    city = value;
                    OnPropertyChanged("City");
                }
            }
        }

        private CreatorType creatorType;
        public CreatorType CreatorType
        {
            get { return creatorType; }
            set
            {
                if(creatorType!=value)
                {
                    creatorType = value;
                    OnPropertyChanged("CreatorType");
                }
            }
        }

        public ForumCommentDTO() { }
        public ForumCommentDTO(int id, int forumId ,int guestId, string comment,string report,string author,string city,CreatorType creatorType)
        {
            Id = id;
            this.forumId = forumId;
            this.guestId = guestId;
            this.comment = comment;
            this.report = report;
            this.author = author;
            this.city = city;
            this.creatorType = creatorType;
        }
        public ForumCommentDTO(int forumId, int guestId, string comment,string report,string author,string city,CreatorType creatorType)
        {
            this.forumId = forumId;
            this.guestId = guestId;
            this.comment = comment;
            this.report = report;
            this.author = author;
            this.city = city;
            this.creatorType = creatorType;
        }
        public ForumCommentDTO(ForumComment forumComment)
        {
            Id = forumComment.Id;
            this.forumId = forumComment.ForumId;
            this.guestId = forumComment.GuestId;
            this.comment = forumComment.Comment;
            this.report = forumComment.Reported;
            this.creatorType = forumComment.CreatorType;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
