using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Utilities;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Domain;
using BookingApp.WPF.View.Owner;
using iText.Commons.Utils;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using iText.Layout.Properties;
using BookingApp.Localization;
using BookingApp.Application.UseCases;
using BookingApp.WPF.View;
using BookingApp.Application.Injector;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Models;
namespace BookingApp.WPF.ViewModel.Owner
{
    public class OwnerWindowViewModel
    {
        private readonly OwnerWindow parentWindow;
        public RateGuestService rateGuestService;
        public ForumService forumService;
        public RelayCommand RequestCommand { get; }
        public RelayCommand OwnerProfileCommand { get; }
        public RelayCommand RenovatingCommand { get; }
        public RelayCommand ReviewCommand { get; }
        public RelayCommand AccommodationsCommand { get; }
        public RelayCommand RateGuestCommand { get; }
        public RelayCommand RegistrationCommand { get; }
        public RelayCommand ForumCommand { get; }
        public RelayCommand AdvicesCommand { get; }
        public OwnerWindowViewModel(OwnerWindow parentWindow)
        {
            this.parentWindow = parentWindow;
            rateGuestService = new RateGuestService();
            forumService = new ForumService(Injector.CreateInstance<IForumRepository>());
            RequestCommand = new RelayCommand(RequestExecute);
            OwnerProfileCommand = new RelayCommand(OwnerProfileExecute);
            RenovatingCommand = new RelayCommand(RenovatingExecute);
            ReviewCommand = new RelayCommand(ReviewExecute);
            RateGuestCommand = new RelayCommand(RateGuestExecute);
            AccommodationsCommand = new RelayCommand(AccommodationsExecute);
            RegistrationCommand = new RelayCommand(RegistrationExecute);
            ForumCommand = new RelayCommand(ForumExecute);
            AdvicesCommand = new RelayCommand(AdvicesExecute);
            ShowAttention();
            ShowForumAttention();
        }

          public void ShowAttention()
            {
                if (rateGuestService.AllowAttentionShow())
                {
                    parentWindow.AttentionImage.Visibility = System.Windows.Visibility.Visible;
                    parentWindow.RateAttentionLabel.Visibility = System.Windows.Visibility.Visible;
                }
            }

        public void ShowForumAttention()
        {
            foreach(Forum forum in forumService.GetAll()){
                if(forum.Open)
                {
                    parentWindow.AttentionImage.Visibility = System.Windows.Visibility.Visible;
                    parentWindow.ForumAttentionLabel.Visibility = System.Windows.Visibility.Visible;
                    break;
                }
            }
        }
          
        public void RequestExecute()
        {
            parentWindow.MainFrame.NavigationService.Navigate(new RequestPage(parentWindow));
        }

        public void OwnerProfileExecute()
        {
            parentWindow.MainFrame.NavigationService.Navigate(new OwnerProfilePage(parentWindow));
        }

        public void RenovatingExecute()
        {
            parentWindow.MainFrame.NavigationService.Navigate(new RenovatingPage(parentWindow));
        }

        public void ReviewExecute()
        {
            parentWindow.MainFrame.NavigationService.Navigate(new ReviewPage(parentWindow));
        }

        public void AccommodationsExecute()
        {
            parentWindow.MainFrame.NavigationService.Navigate(new AccommodationsPage(parentWindow));
        }

        public void RateGuestExecute()
        {
            parentWindow.MainFrame.NavigationService.Navigate(new RateGuestPage(parentWindow));
        }

        public void RegistrationExecute()
        {
            parentWindow.MainFrame.NavigationService.Navigate(new RegistrationAccommodationForm(parentWindow));
        }
        public void ForumExecute()
        {
            parentWindow.MainFrame.NavigationService.Navigate(new ForumPage(parentWindow));
        }

        public void AdvicesExecute()
        {
            parentWindow.MainFrame.NavigationService.Navigate(new AdvicePage(parentWindow));
        }

    }
}

