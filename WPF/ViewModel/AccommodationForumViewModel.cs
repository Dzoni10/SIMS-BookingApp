using BookingApp.WPF.View;
using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Utilities;
using BookingApp.Application.UseCases;
using BookingApp.Application.Injector;
using BookingApp.Domain.RepositoryInterfaces;
using System.Collections.ObjectModel;
using BookingApp.DTO;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.ComponentModel;

namespace BookingApp.WPF.ViewModel
{
    public class AccommodationForumViewModel : INotifyPropertyChanged
    {
        public AccommodationForumPage page;
        private readonly GuestWindow parentWindow;
        private readonly User user;
        private string forumViewMode;
        private int locationId;

        private ForumCommentService forumCommentService { get; set; }
        private ForumService forumService { get; set; }
        private UserService userService { get; set; }

        //public ObservableCollection<ForumCommentDTO> ForumComments { get; set; }
        public ObservableCollection<ForumCard> ForumCards { get; set; }
        public ForumDTO Forum { get; set; }

        public RelayCommand CreateCommentCommand { get;  }
        public RelayCommand CloseForumCommand { get; }

        private string reservationFeedbackMessage;
        public string ReservationFeedbackMessage
        {
            get { return reservationFeedbackMessage; }
            set
            {
                if (value != reservationFeedbackMessage)
                {
                    reservationFeedbackMessage = value;
                    OnPropertyChanged("ReservationFeedbackMessage");
                }
            }
        }

        public AccommodationForumViewModel(GuestWindow parentWindow, User user, AccommodationForumPage page, int locationId, string forumViewMode)
        {
            this.parentWindow = parentWindow;
            this.user = user;
            this.page = page;
            this.locationId = locationId;
            this.forumViewMode = forumViewMode;

            forumCommentService = new ForumCommentService(Injector.CreateInstance<IForumCommentRepository>());
            forumService = new ForumService(Injector.CreateInstance<IForumRepository>());
            userService = new UserService(Injector.CreateInstance<IUserRepository>());

            //ForumComments = new ObservableCollection<ForumCommentDTO>();
            ForumCards = new ObservableCollection<ForumCard>();
            LoadForum(forumViewMode);
            SetForumCards();

            CreateCommentCommand = new RelayCommand(CreateCommentExecute, CanExecute);
            CloseForumCommand = new RelayCommand(CloseForumExecute, CanExecute);
        }
        public bool CanExecute()
        {
            return true;
        }
        public void CreateCommentExecute()
        {
            if (forumViewMode.Equals("create"))
            {
                forumViewMode = " ";
                Forum newForum = new Forum(userService.GetGuestIdFromUser(user), locationId, " ", " ", true);
                forumService.Save(newForum);
                ReservationFeedbackMessage = "Forum successfuly created";
                Forum.Id = newForum.Id;
                Forum.GuestId = newForum.GuestId;
                Forum.LocationId = locationId;
            }
            ForumComment comment = new ForumComment(Forum.Id, Forum.GuestId, 0, Forum.LocationId, page.CommentTextBox.Text.Replace("\n", " ").Replace("\r", ""), CreatorType.Guest, "");
            forumCommentService.Save(comment);
            ForumCards.Add(new ForumCard(parentWindow, user, comment.Id));

            page.CommentTextBox.Text = "";
            //SetForumCards();
            page.CloseForumButton.IsEnabled = true;
        }
        public void CloseForumExecute()
        {
            Forum.OpenAndCreator = false;
            Forum.Open = false;
            Forum forum = Forum.ToForum();
            forum.Open = false;
            forumService.Update(forum);
        }
        public void LoadForum(string forumViewMode)
        {
            if (forumViewMode.Equals("open"))
            {
                OpenForum();
            }
            else
            {
                CreateForum();
            }
        }
        public void OpenForum()
        {
            int forumId = forumService.GetIdFromLocationId(locationId);
            if (forumId > 0)
            {
                Forum forum = forumService.GetById(forumId);
                Forum = new ForumDTO(forum);
                LoadCloseButton();
            }
        }
        public void CreateForum()
        {
            Forum = new ForumDTO();
            Forum.Open = true;
            Forum.OpenAndCreator = true;
            page.CloseForumButton.IsEnabled = false;
        }
        public void LoadCloseButton()
        {
            if(Forum.GuestId == userService.GetGuestIdFromUser(user))
            {
                Forum.OpenAndCreator = true;
            }
        }
        private void SetForumCards()
        {
            foreach(ForumComment comment in forumCommentService.GetAll()) {
                if(Forum.Id == comment.ForumId)
                {
                    ForumCards.Add(new ForumCard(parentWindow, user, comment.Id));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
