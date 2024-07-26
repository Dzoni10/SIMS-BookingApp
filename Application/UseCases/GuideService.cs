using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using Org.BouncyCastle.Asn1.BC;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace BookingApp.Application.UseCases
{
    public class GuideService
    {
        private IGuideRepository guideRepository;
        private StartTourDateService startTourDateService;
        private TourRatingService tourRatingService;
        private TourService tourService;
        public GuideService(IGuideRepository guideRepository, StartTourDateService startTourDateService, TourRatingService tourRatingService, TourService tourService)
        {
            this.guideRepository = guideRepository;
            this.startTourDateService = startTourDateService;
            this.tourRatingService = tourRatingService;
            this.tourService = tourService;
        }
        public List<Guide> GetAll()
        {
            return guideRepository.GetAll();
        }
        public Guide GetById(int id)
        {
            return guideRepository.GetById(id);
        }
        public Guide Save(Guide guide)
        {
            return guideRepository.Save(guide);
        }
        public void Update(Guide guide)
        {
            guideRepository.Update(guide);
        }
        public Guide GetByGuideId(int guideId)
        {
            foreach(Guide guide in  guideRepository.GetAll())
            {
                if(guide.GuideId == guideId)
                {
                    return guide;
                }
            }
            return null;
        }

        public List<string> GetAllTourLanquages(int guideId)
        {
            List<string> languages = new List<string>();
            foreach (var startTourDate in startTourDateService.GetForGuide(tourService.GetToursByGuideId(guideId)))
            {
                if (startTourDate.Status == TourStatus.Finished)
                {
                    languages.Add(tourService.GetById(startTourDate.TourId).Language.Name);
                }
            }
            return languages;
        }
        public string FindLanguage(int guideId)
        {
            List<string> languages = GetAllTourLanquages(guideId);
            var mostFrequentLanguage = languages.GroupBy(x => x).OrderByDescending(g => g.Count()).First();
            if(mostFrequentLanguage.Count() >= 20)
            {
                return mostFrequentLanguage.Key;
            }
            return null;
        }

        public bool CalculateAverageGrades(string language)
        {
            double sum = 0;
            int count = 0;
            foreach(var rating in tourRatingService.GetAllInLastYear())
            {
                if(language.ToLower() == tourService.GetById(startTourDateService.GetById(rating.StartTourDateId).TourId).Language.Name.ToLower())
                {
                    sum += (rating.TourExcitement + rating.GuidesKnowledge + rating.GuidesLanguageAbility) / 3;
                    count++;
                }
            }
            if(sum / count >= 4.0)
            {
                return true;
            }
            return false;
        }

        public void SetSuperGuide(int guideId)
        {
            if(GetByGuideId(guideId) == null)
            {
                guideRepository.Save(new Guide(guideId, "nothing"));
            }
            if(FindLanguage(guideId) == null)
            {
                GuideIsNotSuper(guideId);
                return;
            }
            if (!CalculateAverageGrades(FindLanguage(guideId)))
            {
                GuideIsNotSuper(guideId);
                return;
            }
            GuideIsSuper(guideId);
        }
        public void GuideIsNotSuper(int guideId)
        {
            Guide guide = GetByGuideId(guideId);
            guide.IsSuperGuide = false;
            guideRepository.Update(guide);
        }

        public void GuideIsSuper(int guideId)
        {
            Guide guide = GetByGuideId(guideId);
            guide.IsSuperGuide = true;
            guide.Language = FindLanguage(guideId);
            guideRepository.Update(guide);
        }

    }
}
