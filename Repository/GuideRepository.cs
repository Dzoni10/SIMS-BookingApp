using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class GuideRepository : IGuideRepository
    {
        Repository<Guide> guideRepository;
        public GuideRepository()
        {
            guideRepository = new Repository<Guide>();
        }
        public List<Guide> GetAll()
        {
            return guideRepository.GetAll();
        }

        public Guide GetById(int id)
        {
            return guideRepository.FindId(id);
        }

        public Guide Save(Guide guide)
        {
            return guideRepository.Save(guide);
        }

        public void SaveAll(List<Guide> guides)
        {
            guideRepository.SaveAll(guides);
        }

        public void Update(Guide guide)
        {
            guideRepository.Update(guide);
        }
    }
}
