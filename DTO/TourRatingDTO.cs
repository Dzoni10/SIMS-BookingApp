using BookingApp.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApp.DTO
{
    public class TourRatingDTO : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        private int guidesKnowledge;
        public int GuidesKnowledge
        {
            get { return guidesKnowledge; }
            set
            {
                if (value != guidesKnowledge)
                {
                    guidesKnowledge = value;
                    OnPropertyChanged("GuidesKnowledge");
                }
            }
        }

        private int guidesLanguageAbility;
        public int GuidesLanguageAbility
        {
            get { return guidesLanguageAbility; }
            set
            {
                if (value != guidesLanguageAbility)
                {
                    guidesLanguageAbility = value;
                    OnPropertyChanged("GuidesLanguageAbility");
                }
            }
        }

        private int tourExcitement;
        public int TourExcitement
        {
            get { return tourExcitement; }
            set
            {
                if (value != tourExcitement)
                {
                    tourExcitement = value;
                    OnPropertyChanged("TourExcitement");
                }
            }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                if (value != comment)
                {
                    comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }

        private int userId;
        public int UserId
        {
            get { return userId; }
            set
            {
                if (value != userId)
                {
                    userId = value;
                    OnPropertyChanged("UserId");
                }
            }
        }

        private int startTourDateId;
        public int StartTourDateId
        {
            get { return startTourDateId; }
            set
            {
                if (value != startTourDateId)
                {
                    startTourDateId = value;
                    OnPropertyChanged("StartTourDateId");
                }
            }
        }

        public TourRatingDTO()
        {
        }

        public TourRatingDTO(TourRating rating)
        {
            Id = rating.Id;
            GuidesKnowledge = rating.GuidesKnowledge;
            GuidesLanguageAbility = rating.GuidesLanguageAbility;
            TourExcitement = rating.TourExcitement;
            Comment = rating.Comment;
            UserId = rating.UserId;
            StartTourDateId = rating.StartTourDateId;
        }

        public TourRating ToTourRating()
        {
            return new TourRating
            {
                Id = Id,
                GuidesKnowledge = GuidesKnowledge,
                GuidesLanguageAbility = GuidesLanguageAbility,
                TourExcitement = TourExcitement,
                Comment = Comment,
                UserId = UserId,
                StartTourDateId = StartTourDateId
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
