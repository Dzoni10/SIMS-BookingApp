using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Repository;

namespace BookingApp.Application.Injector
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
    {

            {typeof(IAccommodationRepository), new AccommodationRepository()},
            {typeof(IAccommodationReservationRepository), new AccommodationReservationRepository()},
            {typeof(ICheckpointRepository),new CheckpointRepository() },
            {typeof(ICommentRepository), new CommentRepository()},
            {typeof(IEditedReservationRepository), new EditedReservationRepository() },
            {typeof(IGuestAccommodationRateRepository), new GuestAccommodationRateRepository() },
            {typeof(IGuestRepository), new GuestRepository()},
            {typeof(IIgnoreRepository), new IgnoreRepository()},
            {typeof(IImageRepository), new ImageRepository()},
            {typeof(ILanguageRepository), new LanguageRepository() },
            {typeof(ILocationRepository), new LocationRepository()},
            {typeof(IOwnerAccommodationRateRepository), new OwnerAccommodationRateRepository()},
            {typeof(IOwnerRepository), new OwnerRepository()},
            {typeof(IOwnerNotificationRepository), new OwnerNotificationRepository()},
            {typeof(IStartTourDateRepository), new StartTourDateRepository()},
            {typeof(ITourGuestRepository), new TourGuestRepository()},
            {typeof(ITourRatingRepository), new TourRatingRepository()},
            {typeof(ITourRepository), new TourRepository()},
            {typeof(ITourReservationRepository), new TourReservationRepository()},
            {typeof(IUserRepository), new UserRepository()},
            {typeof(IVoucherRepository), new VoucherRepository()},
            {typeof(IOwnerGuestNotificationRepository),new OwnerGuestNotificationRepository()},
            {typeof(IRenovationRepository),new RenovationRepository()},
            {typeof(INotificationRepository),new NotificationRepository()},
            {typeof(ITourRequestRepository),new TourRequestRepository()},
            {typeof(ICanceledReservationRepository), new CanceledReservationRepository() },
            {typeof(IRenovationRecommendationRepository), new RenovationRecommendationRepository()},
            {typeof(ITourRequestGuestRepository), new TourRequestGuestRepository()},
            {typeof(IGuideRepository), new GuideRepository()},
            {typeof(IForumRepository), new ForumRepository()},
            {typeof(IForumCommentRepository), new ForumCommentRepository()},
            {typeof(IComplexTourRequestRepository), new ComplexTourRequestRepository()}
        // Add more implementations here
    };
        

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }

    }
}
