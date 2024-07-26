using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Image = BookingApp.Domain.Models.Image;

namespace BookingApp.WPF.ViewModel
{
    public class RateTourControlViewModel : UserControl, IDataErrorInfo
    {

        public RateTourControl parentWindow { get; set; }

        public static readonly DependencyProperty TourNameProperty =
            DependencyProperty.Register("TourName", typeof(string), typeof(RateTourControl), new PropertyMetadata(null));

        public string TourName
        {
            get { return (string)GetValue(TourNameProperty); }
            set { SetValue(TourNameProperty, value); }
        }

        public static readonly DependencyProperty TourGuidesKnowledgeProperty =
            DependencyProperty.Register("TourGuidesKnowledge", typeof(int), typeof(RateTourControl), new PropertyMetadata(null));

        public int TourGuidesKnowledge
        {
            get { return (int)GetValue(TourGuidesKnowledgeProperty); }
            set { SetValue(TourGuidesKnowledgeProperty, value); }
        }

        public static readonly DependencyProperty TourGuidesLanguageSpeakingProperty =
            DependencyProperty.Register("TourGuidesLanguageSpeaking", typeof(int), typeof(RateTourControl), new PropertyMetadata(null));

        public int TourGuidesLanguageSpeaking
        {
            get { return (int)GetValue(TourGuidesLanguageSpeakingProperty); }
            set { SetValue(TourGuidesLanguageSpeakingProperty, value); }
        }

        public static readonly DependencyProperty TourExcitementProperty =
            DependencyProperty.Register("TourExcitement", typeof(int), typeof(RateTourControl), new PropertyMetadata(null));

        public int TourExcitement
        {
            get { return (int)GetValue(TourExcitementProperty); }
            set { SetValue(TourExcitementProperty, value); }
        }

        public static readonly DependencyProperty AdditionalCommentProperty =
            DependencyProperty.Register("AdditionalComment", typeof(string), typeof(RateTourControl), new PropertyMetadata(null));

        public string AdditionalComment
        {
            get { return (string)GetValue(AdditionalCommentProperty); }
            set { SetValue(AdditionalCommentProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(Image), typeof(RateTourControl), new PropertyMetadata(null));

        public event EventHandler DeleteRequested;

        public List<Image> Images;
        public int StartTourDateId;


        private TourService TourService;
        private TourRatingService TourRatingService;
        private ImageService ImageService;
        private int ImageCounter;
        public User LoggedInUser { get; set; }

        public RelayCommand AttachPicturesCommand { get; set; }
        public RelayCommand RateTourCommand { get; set; }

        public RateTourControlViewModel(RateTourControl rateTourControl, string name, int startTourDateId, User user)
        {
            parentWindow = rateTourControl;
            LoggedInUser = user;
            TourName = name;
            StartTourDateId = startTourDateId;
            TourService = new TourService(Injector.CreateInstance<ITourRepository>(), 
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), 
                new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>())));
            TourRatingService = new TourRatingService(Injector.CreateInstance<ITourRatingRepository>());
            ImageService = new ImageService(Injector.CreateInstance<IImageRepository>());
            Images = new List<Image>();
            ImageCounter = 0;
            TourGuidesKnowledge = 1;
            TourGuidesLanguageSpeaking = 1;
            TourExcitement = 1;
            AdditionalComment = "";

            AttachPicturesCommand = new RelayCommand(AttachPicturesClick, CanExecute);
            RateTourCommand = new RelayCommand(RateTourClick, CanExecute);
        }

        private bool CanExecute()
        {
            return true;
        }

        private void AttachPicturesClick()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.gif)|*.png;*.jpeg;*.jpg;*.gif|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string path in openFileDialog.FileNames)
                {
                    Image addedImage = new Image();
                    addedImage.Path = path;
                    addedImage.EntityType = EntityType.rt;
                    string[] name = TourName.Split(" ");
                    TourName = name[0] + " " + name[1];
                    addedImage.EntityId = TourService.GetByName(TourName).Id;
                    Images.Add(addedImage);
                    ImageCounter++;
                }
            }
            InitializeLabel();
        }

        private void InitializeLabel()
        {
            parentWindow.PicturesAttached.Content = $"{ImageCounter} pictures attached";
            parentWindow.PicturesAttached.Visibility = Visibility.Visible;
        }

        private async void RateTourClick()
        {
            if (!AreConditionsMet() || !string.IsNullOrEmpty(this["AdditionalComment"]))
            {
                parentWindow.ErrorLabel.Visibility = Visibility.Visible;
                return;
            }
            TourRating tourRating = new TourRating(TourGuidesKnowledge, TourGuidesLanguageSpeaking, TourExcitement, AdditionalComment, LoggedInUser.Id, StartTourDateId);
            TourRatingService.Save(tourRating);
            foreach (Image image in Images)
            {
                ImageService.Save(image);
            }
            parentWindow.ErrorLabel.Visibility = Visibility.Collapsed;
            parentWindow.PicturesAttached.Visibility = Visibility.Collapsed;
            parentWindow.ThankYouMessage.Visibility = Visibility.Visible;
            parentWindow.RateButton.Visibility = Visibility.Collapsed;
            await Task.Delay(3000);
            DeleteRequested?.Invoke(this, EventArgs.Empty);
        }

        private bool AreConditionsMet()
        {
            if (TourGuidesKnowledge != 0 && TourGuidesLanguageSpeaking != 0 && TourExcitement != 0 && !AdditionalComment.Equals("") && AdditionalComment != null)
                return true;
            return false;
        }

        public string this[string columnName]
        {
            get
            {
                string error = null;
                switch (columnName)
                {
                    case nameof(TourGuidesKnowledge):
                        if (TourGuidesKnowledge < 1 || TourGuidesKnowledge > 5)
                            error = "Tour guide's knowledge must be between 1 and 5.";
                        break;
                    case nameof(TourGuidesLanguageSpeaking):
                        if (TourGuidesLanguageSpeaking < 1 || TourGuidesLanguageSpeaking > 5)
                            error = "Tour guide's language speaking must be between 1 and 5.";
                        break;
                    case nameof(TourExcitement):
                        if (TourExcitement < 1 || TourExcitement > 5)
                            error = "Tour excitement must be between 1 and 5.";
                        break;
                    case nameof(AdditionalComment):
                        if (AdditionalComment.Equals(""))
                            error = "You have to leave a comment.";
                        break;
                }
                return error;
            }
        }

        public string Error => null; // No global error for the entire object
    }
}
