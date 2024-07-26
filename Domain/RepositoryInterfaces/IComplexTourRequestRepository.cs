using BookingApp.Domain.Models;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IComplexTourRequestRepository
    {
        public ComplexTourRequest Save(ComplexTourRequest complexTourRequest);
        public void SaveAll(List<ComplexTourRequest> complexTourRequests);
        public void Update(ComplexTourRequest complexTourRequest);
        public ComplexTourRequest GetById(int id);
        public List<ComplexTourRequest> GetAll();
    }
}
