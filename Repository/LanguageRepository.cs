using System.Collections.Generic;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;

namespace BookingApp.Repository
{
    public class LanguageRepository : ILanguageRepository
    {
        Repository<Language> languageRepository;

        public LanguageRepository()
        {
            languageRepository = new Repository<Language>();
        }

        public Language Save(Language language)
        {
            return languageRepository.Save(language);
        }

        public void SaveAll(List<Language> languages)
        {
            languageRepository.SaveAll(languages);
        }

        public void Update(Language language)
        {
            languageRepository.Update(language);
        }

        public Language GetById(int id)
        {
            return languageRepository.FindId(id);
        }

        public List<Language> GetAll()
        {
            return languageRepository.GetAll();
        }
    }
}
