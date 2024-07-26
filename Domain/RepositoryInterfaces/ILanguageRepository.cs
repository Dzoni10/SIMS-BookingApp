using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ILanguageRepository
    {
        public Language Save(Language language);
        public void SaveAll(List<Language> languages);
        public void Update(Language language);
        public Language GetById(int id);
        public List<Language> GetAll();
    }
}
