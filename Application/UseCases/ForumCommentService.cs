using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class ForumCommentService
    {
        public IForumCommentRepository forumCommentRepository;
        public LocationService locationService;

        public ForumCommentService(IForumCommentRepository forumCommentRepository)
        {
            this.forumCommentRepository = forumCommentRepository;
            locationService = new LocationService(Injector.Injector.CreateInstance<ILocationRepository>());
        }
        public ForumComment GetById(int id)
        {
            return forumCommentRepository.GetById(id);
        }
        public void Save(ForumComment forumComment)
        {
            forumCommentRepository.Save(forumComment);
        }
        public List<ForumComment> GetAll()
        {
            return forumCommentRepository.GetAll();
        }

        public void UpdateReportByCommentId(int commentId,string report) 
        {
            ForumComment forumComment = forumCommentRepository.GetById(commentId);
            forumComment.Reported=report;
            forumCommentRepository.Update(forumComment);
        }
    }
}
