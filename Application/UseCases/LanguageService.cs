using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class LanguageService
    {
        private ILanguageRepository languageRepository;
        public LanguageService(ILanguageRepository languageRepository) 
        {
            this.languageRepository = languageRepository;
        }

        public Language Save(Language language)
        {
           return languageRepository.Save(language);
        }

        public bool IsLanguageAlreadyWritten(string name)
        {
            Language language = languageRepository.GetAll().FirstOrDefault(l => l.Name.Equals(name));
            if (language == null)
                return false;
            return true;
        }

        public Language GetLanguageByName(string name)
        {
            Language language = languageRepository.GetAll().FirstOrDefault(l => l.Name.Equals(name));
            return language;
        }
    }
}
