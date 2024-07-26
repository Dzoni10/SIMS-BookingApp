using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class RenovationRecommendationService
    {
        public IRenovationRecommendationRepository renovationRecommendationRepository;

        public RenovationRecommendationService()
        {
            renovationRecommendationRepository = Injector.Injector.CreateInstance<IRenovationRecommendationRepository>();
        }
        
        public RenovationRecommendation GetById(int id)
        {
            return renovationRecommendationRepository.GetById(id);
        }
        public List<RenovationRecommendation> GetAll()
        {
            return renovationRecommendationRepository.GetAll();
        }
        public void Save(RenovationRecommendation recommendation)
        {
            renovationRecommendationRepository.Save(recommendation);
        }
        public RenovationRecommendation GetByAccommodationReservationId(int reservationId)
        {
            foreach(RenovationRecommendation renovation in renovationRecommendationRepository.GetAll())
            {
                if(renovation.AccommodationReservationId == reservationId)
                {
                    return renovation;
                }
            }
            return null;
        }
    }
}
