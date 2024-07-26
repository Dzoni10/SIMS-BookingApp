using BookingApp.Domain.Models;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IRenovationRecommendationRepository
    {
        public void Save(RenovationRecommendation recommendation);
        public void SaveAll(List<RenovationRecommendation> recommendations);
        public void Update(RenovationRecommendation recommendation);
        public RenovationRecommendation GetById(int id);
        public List<RenovationRecommendation> GetAll();
        public void Delete(RenovationRecommendation recommendation);
    }
}
