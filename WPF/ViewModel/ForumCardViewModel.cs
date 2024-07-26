using BookingApp.Domain.Models;
using BookingApp.WPF.View;
using BookingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Application.UseCases;
using BookingApp.Application.Injector;
using BookingApp.Domain.RepositoryInterfaces;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace BookingApp.WPF.ViewModel
{
    class ForumCardViewModel : INotifyPropertyChanged
    {
        private readonly GuestWindow parentWindow;
        private readonly User user;
        private readonly ForumCard page;
        private int forumCommentId;

        private ForumCommentService forumCommentService { get; set; }
        private GuestService guestService { get; set; }
        private ForumService forumService { get; set; }
        private OwnerService ownerService { get; set; }
        public ForumCommentDTO ForumComment { get; set; }


        private bool guestBeenOnLocation;
        public bool GuestBeenOnLocation
        {
            get { return guestBeenOnLocation; }
            set
            {
                if (guestBeenOnLocation != value)
                {
                    guestBeenOnLocation = value;
                    OnPropertyChanged("GuestBeenOnLocation");
                }
            }
        }

        public ForumCardViewModel(GuestWindow parentWindow, User user, ForumCard page, int forumCommentId)
        {
            this.parentWindow = parentWindow;
            this.user = user;
            this.page = page;
            this.forumCommentId = forumCommentId;

            forumCommentService = new ForumCommentService(Injector.CreateInstance<IForumCommentRepository>());
            guestService = new GuestService();
            forumService = new ForumService(Injector.CreateInstance<IForumRepository>());
            ownerService = new OwnerService();

            ForumComment = new ForumCommentDTO();
            LoadCommentData();

        }
        public void LoadCommentData()
        {
            ForumComment forumComment = forumCommentService.GetById(forumCommentId);
            ForumComment.Comment = forumComment.Comment;
            if(forumComment.CreatorType == CreatorType.Guest)
            {
                ForumComment.Author = guestService.GetById(forumComment.GuestId).Name;
            }
            else
            {
                ForumComment.Author = ownerService.GetById(forumComment.OwnerId).Name + " - Owner";
            }
            guestBeenOnLocation = guestService.HasBeenOnLocation(forumComment.GuestId, forumComment.LocationId);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
