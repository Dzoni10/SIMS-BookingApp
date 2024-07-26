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
    public class ComplexTourRequestService
    {
        private IComplexTourRequestRepository complexTourRequestRepository;

        public ComplexTourRequestService(IComplexTourRequestRepository complexTourRequestRepository)
        {
            this.complexTourRequestRepository = complexTourRequestRepository;
        }

        public ComplexTourRequest Save(ComplexTourRequest complexTourRequest)
        {
            return complexTourRequestRepository.Save(complexTourRequest);
        }

        public void SaveAll(List<ComplexTourRequest> complexTourRequests)
        {
            complexTourRequestRepository.SaveAll(complexTourRequests);
        }

        public void Update(ComplexTourRequest complexTourRequest)
        {
            complexTourRequestRepository.Update(complexTourRequest);
        }

        public ComplexTourRequest GetById(int id)
        {
            return complexTourRequestRepository.GetById(id);
        }

        public List<ComplexTourRequest> GetAll()
        {
            return complexTourRequestRepository.GetAll();
        }

        public List<ComplexTourRequest> GetByTouristId(int touristId)
        {
            return GetAll().Where(r => r.TouristId == touristId).ToList();
        }

    }
}
