using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel
{
    public class ReviewControlViewModel : INotifyPropertyChanged
    {
        public string Tourist { get; set; }
        public string Tour { get; set; }
        public string GuideKnowlage { get; set; }
        public string GuideLanguage { get; set; }
        public string TourFun { get; set; }
        public string Checkpoint { get; set; }
        public string Comment { get; set; }
        public bool Valid { get; set; }

        public int Id { get; set; }
        private string imagePath;
        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                if (imagePath != value)
                {
                    imagePath = value;
                    OnPropertyChanged(nameof(ImagePath));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private TourRatingService tourRatingService;
        public ReviewControlViewModel(int id, string tourist, int guidesKnowledge, int guidesLanguageAbility, int tourExcitement, string comment, string hint, string tour, bool valid)
        {
            this.tourRatingService = new TourRatingService(Injector.CreateInstance<ITourRatingRepository>());
            Id = id;
            Tourist = tourist;
            GuideKnowlage = $"{guidesKnowledge}/5";
            GuideLanguage = $"{guidesLanguageAbility}/5";
            TourFun = $"{tourExcitement}/5";
            Comment = comment;
            Checkpoint = hint;
            Tour = tour;
            Valid = valid;
            SetValidation();
        }
        public void SetValidation()
        {
            if (Valid == true)
            {
                ImagePath = "../../Resources/Images/accept.png";
            }
            else
            {
                ImagePath = "../../Resources/Images/multiply.png";
            }
        }

        public void Report()
        {
            TourRating rating = tourRatingService.GetById(Id);
            rating.Valid = false;
            tourRatingService.Update(rating);
            Valid = false;
            SetValidation();
        }
    }
}
