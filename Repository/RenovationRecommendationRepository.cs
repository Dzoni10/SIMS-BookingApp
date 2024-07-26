using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class RenovationRecommendationRepository : IRenovationRecommendationRepository
    {
        Repository<RenovationRecommendation> renovationRecommendationRepository;

        public RenovationRecommendationRepository()
        {
            renovationRecommendationRepository = new Repository<RenovationRecommendation>();
        }
        public void Save(RenovationRecommendation recommendation)
        {
            renovationRecommendationRepository.Save(recommendation);
        }
        public void SaveAll(List<RenovationRecommendation> recommendations)
        {
            renovationRecommendationRepository.SaveAll(recommendations);
        }
        public void Update(RenovationRecommendation recommendation)
        {
            renovationRecommendationRepository.Update(recommendation);
        }
        public RenovationRecommendation GetById(int id)
        {
            return renovationRecommendationRepository.FindId(id);
        }
        public List<RenovationRecommendation> GetAll()
        {
            return renovationRecommendationRepository.GetAll();
        }
        public void Delete(RenovationRecommendation recommendation)
        {
            renovationRecommendationRepository.Delete(recommendation);
        }
    }
}
