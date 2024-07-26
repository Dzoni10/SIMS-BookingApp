using BookingApp.WPF.View.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using BookingApp.Utilities;
using System.Windows.Data;
using BookingApp.DTO;
using System.Collections.ObjectModel;
using BookingApp.Application.UseCases;
using BookingApp.Application.Injector;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Models;
namespace BookingApp.WPF.ViewModel.Owner
{
    public class ForumViewModel
    {
        private readonly OwnerWindow parentWindow;
        public ForumPage page;
        public ObservableCollection<ForumCommentDTO> Comments { get; set; }
        public ForumCommentService forumCommentService;
        public GuestService guestService;
        public OwnerService ownerService;
        public LocationService locationService;
        public ForumCommentDTO SelectedComment { get; set; }
        public RelayCommand RegistrationCommand { get; }
        public RelayCommand RateGuestCommand { get; }
        public RelayCommand RequestCommand { get; }
        public RelayCommand AccommodationsCommand { get; }
        public RelayCommand OwnerProfileCommand { get; }
        public RelayCommand RenovatingCommand { get; }
        public RelayCommand ReviewCommand { get; }
        public RelayCommand AdvicesCommand { get; }
        public RelayCommand HomePageCommand { get; }
        public RelayCommand BackCommand { get; }
        public RelayCommand ReportCommand { get; }
        public RelayCommand CommentCommand { get; }
        public ForumViewModel(ForumPage page, OwnerWindow parentWindow)
        {
            this.parentWindow = parentWindow;
            this.page = page;
            Comments = new ObservableCollection<ForumCommentDTO>();
            forumCommentService = new ForumCommentService(Injector.CreateInstance<IForumCommentRepository>());
            guestService = new GuestService();
            ownerService = new OwnerService();
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            RegistrationCommand = new RelayCommand(RegistrationExecute);
            RateGuestCommand = new RelayCommand(RateGuestExecute);
            RequestCommand = new RelayCommand(RequestExecute);
            AccommodationsCommand = new RelayCommand(AccommodationsExecute);
            OwnerProfileCommand = new RelayCommand(OwnerProfileExecute);
            RenovatingCommand = new RelayCommand(RenovatingExecute);
            ReviewCommand = new RelayCommand(ReviewExecute);
            AdvicesCommand = new RelayCommand(AdvicesExecute);
            BackCommand = new RelayCommand(BackExecute);
            ReportCommand = new RelayCommand(ReportExecute, CanReport);
            CommentCommand = new RelayCommand(CommentExecute, CanComment);
            HomePageCommand = new RelayCommand(HomePageExecute);
            FillForum();
        }

        public void FillForum()
        {
            Comments.Clear();
            string author = "";
            foreach (ForumComment forumComment in forumCommentService.GetAll())
            {
                if (forumComment.CreatorType == CreatorType.Guest)
                {
                    author = guestService.GetGuestNameById(forumComment.GuestId);
                }
                else
                {
                    author = ownerService.GetById(forumComment.OwnerId).Name + "(Owner)";
                }
                string city = locationService.GetById(forumComment.LocationId).City;
                ForumCommentDTO forumCommentView = new ForumCommentDTO(forumComment.Id, forumComment.ForumId, forumComment.GuestId, forumComment.Comment, forumComment.Reported, author,city,forumComment.CreatorType);
                Comments.Add(forumCommentView);
            }
        }

        private bool CanReport()
        {
            return SelectedComment != null && string.IsNullOrEmpty(page.CommentTextBox.Text) && !SelectedComment.CreatorType.Equals(CreatorType.Owner);
        }
        private bool CanComment()
        {
            return !(string.IsNullOrEmpty(page.CommentTextBox.Text)) && SelectedComment!=null;
        }
        public void RegistrationExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RegistrationAccommodationForm(parentWindow));
        }
        public void RateGuestExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RateGuestPage(parentWindow));
        }
        public void RequestExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RequestPage(parentWindow));
        }
        public void AccommodationsExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new AccommodationsPage(parentWindow));
        }
        public void OwnerProfileExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new OwnerProfilePage(parentWindow));
        }
        public void RenovatingExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RenovatingPage(parentWindow));
        }
        public void ReviewExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new ReviewPage(parentWindow));
        }
        public void AdvicesExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new AdvicePage(parentWindow));
        }
        public void CommentExecute() 
        {
            string comment = page.CommentTextBox.Text;
            int locationId = forumCommentService.GetById(SelectedComment.Id).LocationId;
            ForumComment forumComment = new ForumComment(SelectedComment.ForumId, 0, 1, locationId, comment, CreatorType.Owner, "");
            forumCommentService.Save(forumComment);
            page.CommentTextBox.Text = "";
            FillForum();
        }
        
        public void ReportExecute() {
           int forumCommentId = SelectedComment.Id;
           forumCommentService.UpdateReportByCommentId(forumCommentId, "reported");
            Comments.Clear();
            FillForum();
        }
        public void HomePageExecute()
        {
            parentWindow.Show();
            parentWindow.Activate();
            page.NavigationService.Navigate(null);
        }
        public void BackExecute()
        {
            parentWindow.Show();
            parentWindow.Activate();
            page.NavigationService.Navigate(null);
        }
    }
}
