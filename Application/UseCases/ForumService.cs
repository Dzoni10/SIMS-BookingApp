using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class ForumService
    {
        private IForumRepository forumRepository;
        //private LocationService locationService;
        private ForumCommentService forumCommentService;
        private GuestService guestService;

        public ForumService(IForumRepository forumRepository)
        {
            this.forumRepository = forumRepository;
            //locationService = new LocationService(Injector.Injector.CreateInstance<ILocationRepository>());
            forumCommentService = new ForumCommentService(Injector.Injector.CreateInstance<IForumCommentRepository>());
            guestService = new GuestService();
        }

        public List<Forum> GetAll()
        {
            return forumRepository.GetAll();
        }


        public Forum GetById(int id)
        {
            return forumRepository.GetById(id);
        }
        public void Save(Forum forum)
        {
            forumRepository.Save(forum);
        }
        public void Update(Forum forum)
        {
            forumRepository.Update(forum);
        }
        public bool ExistsForLocation(int locationId)
        {
            foreach(Forum forum in forumRepository.GetAll())
            {
                if (forum.LocationId == locationId) return true;
            }
            return false;
        }
        public int GetIdFromLocationId(int locationId)
        {
            foreach (Forum forum in forumRepository.GetAll())
            {
                if (forum.LocationId == locationId) return forum.Id;
            }
            return -1;
        }
        public bool IsVeryUseful(int forumId)
        {

            int usefulCommentOwner = 0;
            int usefulCommentGuest = 0;

            List<ForumComment> commentsForForum = forumCommentService.GetAll().Where(r => r.ForumId == forumId).ToList();

            foreach (ForumComment comment in commentsForForum)
            {
                if(comment.CreatorType == CreatorType.Owner)
                {
                    usefulCommentOwner++;
                }
                else if(guestService.HasBeenOnLocation(comment.GuestId, comment.LocationId))
                {
                    usefulCommentGuest++;
                }
            }
            
            if(usefulCommentOwner >= 10 && usefulCommentGuest >= 20)
            {
                return true;
            }

            return false;
        }
    }
}
