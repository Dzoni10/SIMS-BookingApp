using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shell;

namespace BookingApp.WPF.ViewModel
{
    public class RateAccommodationViewModel : INotifyPropertyChanged
    {
        public RateAccommodationPage page;
        private readonly GuestWindow parentWindow;
        private readonly User user;
        private readonly int reservationId;
        private OwnerAccommodationRateService ownerAccommodationRateService { get; set; }
        private AccommodationReservationService accommodationReservationService { get; set; }
        private RenovationRecommendationService renovationRecommendationService { get; set; }
        private ImageRepository imageRepository { get; set; }
        public ObservableCollection<string> ImagesPaths { get; set; }
        public ObservableCollection<OwnerAccommodationRateDTO> Rate { get; set; }
        public OwnerAccommodationRateDTO SelectedRate { get; set; }
        public string SelectedImagePath { get; set; }
        public RelayCommand AddImageCommand { get; }
        public RelayCommand DeleteImageCommand { get; }
        public RelayCommand RateAccommodationCommand { get; }
        public RelayCommand RenovationGetFocusCommand { get; }
        public RelayCommand RenovationLostFocusCommand { get; }
        public RelayCommand CommentGetFocusCommand { get; }
        public RelayCommand CommentLostFocusCommand { get; }
        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                if (value != errorMessage)
                {
                    errorMessage = value;
                    OnPropertyChanged("ErrorMessage");
                }
            }
        }
        public RateAccommodationViewModel(GuestWindow parentWindow, User user, RateAccommodationPage page, int reservationId)
        {
            this.page = page;
            this.parentWindow = parentWindow;
            this.user = user;
            this.reservationId = reservationId;

            ownerAccommodationRateService = new OwnerAccommodationRateService();
            accommodationReservationService = new AccommodationReservationService();
            renovationRecommendationService = new RenovationRecommendationService();

            imageRepository = new ImageRepository();
            ImagesPaths = new ObservableCollection<string>();
            Rate = new ObservableCollection<OwnerAccommodationRateDTO>();
            SelectedRate = new OwnerAccommodationRateDTO();
            SelectedRate.AccommodationId = accommodationReservationService.GetById(reservationId).AccommodationId;
            SelectedRate.GuestId = accommodationReservationService.GetById(reservationId).GuestId;

            ErrorMessage = "";

            AddImageCommand = new RelayCommand(AddImageExecute, CanExecute);
            DeleteImageCommand = new RelayCommand(DeleteImageExecute, CanExecute);
            RateAccommodationCommand = new RelayCommand(RateAccommodationExecute, CanExecute);
            RenovationGetFocusCommand = new RelayCommand(RenovationGetFocusExecute, CanExecute);
            RenovationLostFocusCommand = new RelayCommand(RenovationLostFocusExecute, CanExecute);
            CommentGetFocusCommand = new RelayCommand(CommentGetFocusExecute, CanExecute);
            CommentLostFocusCommand = new RelayCommand(CommentLostFocusExecute, CanExecute);
        }
        public bool CanExecute()
        {
            return true;
        }

        public void AddImageExecute()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Odaberi datoteku";
            openFileDialog.Filter = "Slike (*.jpg; *.png; *.bmp)|*.jpg;*.png;*.bmp";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string relativePath = "Resources\\Images";

                string selectedFilePath = openFileDialog.FileName;
                ImagesPaths.Add(selectedFilePath);
            }
        }
        public void DeleteImageExecute()
        {
            if (SelectedImagePath != null)
            {
                ErrorMessage = SelectedImagePath + " - Removed";
                ImagesPaths.Remove(SelectedImagePath);
                page.ErrorMessageTextBlock.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                ErrorMessage = "Morate prvo selektovati sliku";
                page.ErrorMessageTextBlock.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
        public void RateAccommodationExecute()
        {
            SelectedRate.CleanlinessGrade = GetRateForCleanliness();

            SelectedRate.OwnerHospitality = GetRateForHospitality();

            if (page.AdditionalCommentTextBox.Text == "Add additional comment...")
            {
                SelectedRate.AdditionalComment = " ";
            }
            else
            {
                SelectedRate.AdditionalComment = page.AdditionalCommentTextBox.Text;
            }


            SetImages();

            OwnerAccommodationRate ownerAccommodationRate = SelectedRate.ToOwnerAccommodationRate();
            ownerAccommodationRate.AccommodationReservationId = reservationId;
            ownerAccommodationRateService.Save(ownerAccommodationRate);

            //SelectedRate.AccommodationReservationId = reservationId;

            //ownerAccommodationRateService.Save(SelectedRate.ToOwnerAccommodationRate());

            string renovationComment;
            if (page.RenovationCommentTextBox.Text == "Add renovation reccommendation comment...")
            {
                renovationComment = " ";
            }
            else
            {
                renovationComment = page.RenovationCommentTextBox.Text;
            }
            
            RenovationRecommendation recommendation = new RenovationRecommendation(SelectedRate.AccommodationId, reservationId, renovationComment, GetEmergencyRenovationRate(), DateTime.Now);
            renovationRecommendationService.Save(recommendation);


            accommodationReservationService.UpdateReservationRateStatus(reservationId);

            parentWindow.MainFrame.NavigationService.Navigate(new RateAccommodations(parentWindow, user, "Accommodation successfuly rated"));
        }

        public void RenovationGetFocusExecute()
        {
            if (page.RenovationCommentTextBox.Text == "Add renovation reccommendation comment...")
            {
                page.RenovationCommentTextBox.Text = "";
                page.RenovationCommentTextBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }
        public void RenovationLostFocusExecute()
        {
            if (page.RenovationCommentTextBox.Text == "")
            {
                page.RenovationCommentTextBox.Text = "Add renovation reccommendation comment...";
                page.RenovationCommentTextBox.Foreground = System.Windows.Media.Brushes.LightGray;
            }
        }
        public void CommentGetFocusExecute()
        {
            if (page.AdditionalCommentTextBox.Text == "Add additional comment...")
            {
                page.AdditionalCommentTextBox.Text = "";
                page.AdditionalCommentTextBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }
        public void CommentLostFocusExecute()
        {
            if (page.AdditionalCommentTextBox.Text == "")
            {
                page.AdditionalCommentTextBox.Text = "Add additional comment...";
                page.AdditionalCommentTextBox.Foreground = System.Windows.Media.Brushes.LightGray;
            }
        }
        public double GetRateForHospitality()
        {
            foreach (var child in page.HospitalityStackPanel.Children)
            {
                if (child is RadioButton rb && rb.IsChecked == true)
                {
                    return Convert.ToDouble(rb.Content);
                }
            }
            return 0;
        }
        public double GetRateForCleanliness()
        {
            foreach (var child in page.CleanlinessStackPanel.Children)
            {
                if (child is RadioButton rb && rb.IsChecked == true)
                {
                    return Convert.ToDouble(rb.Content);
                }
            }
            return 0;
        }
        public double GetEmergencyRenovationRate()
        {
            return page.RenovationEmergencyComboBox.SelectedIndex + 1;
        }

        private void SetImages()
        {
            foreach (string image in ImagesPaths)
            {
                //imageRepository.Save(new Domain.Models.Image(image, SelectedRate.AccommodationId, EntityType.ra));
                imageRepository.Save(new Domain.Models.Image(image, reservationId, EntityType.ra));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
