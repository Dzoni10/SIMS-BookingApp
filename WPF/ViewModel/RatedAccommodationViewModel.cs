using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.DTO;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel
{
    public class RatedAccommodationViewModel : INotifyPropertyChanged
    {
        public RatedAccommodationPage page;
        private readonly GuestWindow parentWindow;
        private readonly User user;
        private int reservationId;

        private ImageService imageService;
        private OwnerAccommodationRateService ownerAccommodationRateService;
        private AccommodationsService accommodationService;
        private AccommodationReservationService accommodationReservationService;
        private RenovationRecommendationService renovationRecommendationService;

        private int currentImageId;
        public List<Image> Images { get; set; }
        public OwnerAccommodationRateDTO OwnerAccommodationRate { get; set; }

        public RelayCommand LeftImageCommand { get; }
        public RelayCommand RightImageCommand { get; }


        private Image currentImage;
        public Image CurrentImage
        {
            get { return currentImage; }
            set
            {
                if (value != currentImage)
                {
                    currentImage = value;
                    OnPropertyChanged("CurrentImage");
                }
            }
        }


        private string ratedAccommodation;
        public string RatedAccommodation
        {
            get { return ratedAccommodation; }
            set
            {
                if (value != ratedAccommodation)
                {
                    ratedAccommodation = value;
                    OnPropertyChanged("RatedAccommodation");
                }
            }
        }
        public RatedAccommodationViewModel(GuestWindow parentWindow, User user, RatedAccommodationPage page, int reservationId)
        {
            this.page = page;
            this.parentWindow = parentWindow;
            this.user = user;
            this.reservationId = reservationId;

            imageService = new ImageService();
            ownerAccommodationRateService = new OwnerAccommodationRateService();
            accommodationService = new AccommodationsService();
            accommodationReservationService = new AccommodationReservationService();
            renovationRecommendationService = new RenovationRecommendationService();

            AccommodationReservation reservation = accommodationReservationService.GetById(reservationId);
            RatedAccommodation = "Rated accommodation - " + accommodationService.GetById(reservation.AccommodationId).Name;
            currentImageId = 0;
            CurrentImage = new Image();
            Images = new List<Image>();
            OwnerAccommodationRate = new OwnerAccommodationRateDTO(ownerAccommodationRateService.GetRateByReservationId(reservationId));
            RenovationRecommendation renovation = renovationRecommendationService.GetByAccommodationReservationId(reservationId);
            OwnerAccommodationRate.RenovationComment = renovation.RenovationComment;
            OwnerAccommodationRate.RenovationEmergencyGrade = renovation.RenovationEmergencyGrade;
            LoadImage();

            LeftImageCommand = new RelayCommand(LeftImageExecute, CanExecute);
            RightImageCommand = new RelayCommand(RightImageExecute, CanExecute);

        }
        public bool CanExecute()
        {
            return true;
        }

        public void LeftImageExecute()
        {
            if(currentImageId > 0)
            {
                currentImageId--;
                CurrentImage = Images[currentImageId];
            }
        }
        public void RightImageExecute()
        {
            if(currentImageId < Images.Count - 1)
            {
                currentImageId++;
                CurrentImage = Images[currentImageId];
            }
        }
        public void LoadImage()
        {
            foreach(Image image in imageService.GetImagesForRateAccommodation(reservationId))
            {
                Images.Add(image);
            }
            if(Images.Count > 0)
            {
                CurrentImage = Images[currentImageId];
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
