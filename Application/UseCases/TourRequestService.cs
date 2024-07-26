using BookingApp.Application.Injector;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;
using BookingApp.Repository;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Org.BouncyCastle.Bcpg;

namespace BookingApp.Application.UseCases
{
    public class TourRequestService
    {
        public ITourRequestRepository tourRequestRepository;
        public TourRequestGuestService tourRequestGuestService;

        public TourRequestService(ITourRequestRepository tourRequestRepository, TourRequestGuestService tourRequestGuestService) 
        {
            this.tourRequestRepository = tourRequestRepository;
            this.tourRequestGuestService = tourRequestGuestService;
        }

        public void Save(TourRequest tourRequest)
        {
            tourRequestRepository.Save(tourRequest);
        }
        public void SaveAll(List<TourRequest> tourRequest)
        {
            tourRequestRepository.SaveAll(tourRequest);
        }
        public void Update(TourRequest tourRequest)
        {
            tourRequestRepository.Update(tourRequest);
        }
        public TourRequest GetById(int id)
        {
            return tourRequestRepository.GetById(id);
        }
        public List<TourRequest> GetAll()
        {
            return tourRequestRepository.GetAll();
        }

        public List<TourRequest> GetRequestsByUser(int id)
        {
            List<TourRequest> list = new List<TourRequest>();
            foreach(TourRequest request in GetAll())
                if(request.RequestingUser.Id == id && request.ComplexTourRequestId == 0)
                {
                    if (request.Status == TourRequestStatus.Accepted && request.DateIfAccepted < DateTime.Now)
                        continue;
                    list.Add(request);
                }
            return list;
        }


        public List<TourRequest> GetPendingRequests()
        {
            List<TourRequest> tourRequests = new List<TourRequest>();
            foreach(TourRequest request in tourRequestRepository.GetAll())
            {
                if(request.Status == TourRequestStatus.Pending_request && request.DateTo >= DateOnly.FromDateTime(DateTime.Now) && request.ComplexTourRequestId==0)
                {
                    tourRequests.Add(request);
                }          
            }
            return tourRequests;
        }

        public List<TourRequest> GetInvalidRequests()
        {
            List<TourRequest> tourRequests = new List<TourRequest>();
            List<TourRequest> allRequests = tourRequestRepository.GetAll();
            foreach (TourRequest request in allRequests)
            {
                if (request.Status == TourRequestStatus.Invalid)
                {
                    tourRequests.Add(request);
                }
            }
            return tourRequests;
        }

        public List<TourRequest> GetAllRequestInLastYear()
        {
            List<TourRequest> requests = new List<TourRequest>();
            foreach (var request in tourRequestRepository.GetAll())
            {
                if (request.DateTo >= DateOnly.FromDateTime(DateTime.Now.AddYears(-1)))
                {
                    requests.Add(request);
                }
            }
            return requests;
        }

        public string FindMostWantedLanguage()
        {
            List<string> languages = new List<string>();
            foreach(var request in GetAllRequestInLastYear())
            {           
                languages.Add(request.Language.Name);               
            }
            return languages.GroupBy(x => x).OrderByDescending(g => g.Count()).First().Key;           
        }

        public string FindMostWantedState()
        {
            List<string> states = new List<string>();
            foreach (var request in GetAllRequestInLastYear())
            {
                states.Add(request.Location.State);                   
            }
            return states.GroupBy(x => x).OrderByDescending(g => g.Count()).First().Key;
        }
        public string FindMostWantedCity(string selectedState)
        {
            List<string> cities = new List<string>();
            foreach (var request in GetAllRequestInLastYear())
            {
                if(request.Location.State == selectedState)
                    cities.Add(request.Location.City);               
            }
            return cities.GroupBy(x => x).OrderByDescending(g => g.Count()).First().Key;
        }

        public List<int> GetAllRequestYears()
        {
            List<int> years = new List<int>();
            years = tourRequestRepository.GetAll().Select(reguest => reguest.DateFrom.Year).Distinct().ToList();
            return years;
        }

        public List<Tuple<string, int>> GetRequestByYears(List<TourRequest> requests)
        {
            List <Tuple<string,int> > statistics = new List<Tuple<string, int>>();
            var countByYear = requests
            .GroupBy(m => m.DateFrom.Year)
            .Select(g => new { Year = g.Key, Count = g.Count() });
            foreach (var item in countByYear)
            {
                statistics.Add(Tuple.Create(item.Year.ToString(), item.Count));
            }
            return statistics;
        }

        public List<Tuple<string, int>> GetRequestByMonth(List<TourRequest> requests, int year)
        {
            List<Tuple<string, int>> statistics = new List<Tuple<string, int>>();
            var countByMonth = requests.Where(m => m.DateFrom.Year == year)
            .GroupBy(m => m.DateFrom.Month)
            .Select(g => new { Month = g.Key, Count = g.Count() });
            foreach (var item in countByMonth)
            {
                DateTime date = new DateTime(1, item.Month, 1);
                statistics.Add(Tuple.Create(date.ToString("MMMM"), item.Count));
            }
            return statistics;
        }

        public List<TourRequest> GetAllRequestByLanguage(string language)
        {
            return tourRequestRepository.GetAll().Where(x => x.Language.Name.ToLower() == language.ToLower()).ToList();
        }
        public List<TourRequest> GetAllRequestByLocation(string state, string city)
        {
            return tourRequestRepository.GetAll().Where(x => x.Location.State == state && x.Location.City == city).ToList();
        }

        public double GetPercentageOfAcceptedRequestsAllTime()
        {
            double sum = 0;
            double acceptedRequests = 0;
            foreach(TourRequest request in tourRequestRepository.GetAll())
            {
                sum++;
                if (request.Status == TourRequestStatus.Accepted)
                    acceptedRequests++;
            }
            double value = acceptedRequests / sum * 100;
            double roundedValue = Math.Round(value, 2);
            return roundedValue;
        }

        public double GetPercentageOfAcceptedRequestsByYear(int year)
        {
            double sum = 0;
            double acceptedRequests = 0;
            foreach (TourRequest request in tourRequestRepository.GetByYear(year))
            {
                sum++;
                if (request.Status == TourRequestStatus.Accepted)
                    acceptedRequests++;
            }
            double value = acceptedRequests / sum * 100;
            double roundedValue = Math.Round(value, 2);
            return roundedValue;
        }

        public double GetPercentageOfDeclinedOrPendingRequestsByYear(int year)
        {
            double sum = 0;
            double otherRequests = 0;
            foreach (TourRequest request in tourRequestRepository.GetByYear(year))
            {
                sum++;
                if (request.Status != TourRequestStatus.Accepted)
                    otherRequests++;
            }
            double value = otherRequests / sum * 100;
            double roundedValue = Math.Round(value, 2);
            return roundedValue;
        }

        public double GetPercentageOfDeclinedOrPendingRequestsAllTime()
        {
            double sum = 0;
            double otherRequests = 0;
            foreach (TourRequest request in tourRequestRepository.GetAll())
            {
                sum++;
                if (request.Status != TourRequestStatus.Accepted)
                    otherRequests++;
            }
            double value = otherRequests / sum * 100;
            double roundedValue = Math.Round(value, 2);
            return roundedValue;
        }

        public double GetAverageAttendanceOnAcceptedRequestsByYear(int year)
        {
            double requests = 0;
            double numberOfAttendees = 0;
            foreach (TourRequest request in tourRequestRepository.GetByYear(year))
            {
                if (request.Status == TourRequestStatus.Accepted)
                {
                    requests++;
                    List<TourRequestGuest> guests = tourRequestGuestService.GetByRequestId(request.Id);
                    numberOfAttendees += guests.Count();
                }
            }
            double value = numberOfAttendees / requests;
            double roundedValue = Math.Round(value, 2);
            return roundedValue;
        }

        public double GetAverageAttendanceOnAcceptedRequestsAllTime()
        {
            double requests = 0;
            double numberOfAttendees = 0;
            foreach(TourRequest request in tourRequestRepository.GetAll())
            {
                if(request.Status == TourRequestStatus.Accepted)
                {
                    requests++;
                    List<TourRequestGuest> guests = tourRequestGuestService.GetByRequestId(request.Id);
                    numberOfAttendees += guests.Count();
                }
            }
            double value = numberOfAttendees / requests;
            double roundedValue = Math.Round(value, 2);
            return roundedValue;
        }

        public List<Tuple<Language, int>> GetStatsForLanguages()
        {
            List<Tuple<Language, int>> stats = new List<Tuple<Language, int>>();
            foreach(TourRequest request in tourRequestRepository.GetAll())
            {
                if (stats.Any(t => t.Item1.Id == request.Language.Id))
                    continue;

                int numberOfLanguages = 1;
                foreach(TourRequest sample in tourRequestRepository.GetAll())
                {
                    if (request.Id != sample.Id && request.Language.Id == sample.Language.Id)
                        numberOfLanguages++;
                }
                if(numberOfLanguages > 0)
                    stats.Add(new Tuple<Language, int>(request.Language, numberOfLanguages));
            }

            return stats;
        }

        public List<Tuple<Location, int>> GetStatsForLocations()
        {
            List<Tuple<Location, int>> stats = new List<Tuple<Location, int>>();
            foreach (TourRequest request in tourRequestRepository.GetAll())
            {
                if (stats.Any(t => t.Item1.Id == request.Location.Id))
                    continue;

                int numberOfLanguages = 1;
                foreach (TourRequest sample in tourRequestRepository.GetAll())
                {
                    if (request.Id != sample.Id && request.Location.Id == sample.Location.Id)
                        numberOfLanguages++;
                }
                stats.Add(new Tuple<Location, int>(request.Location, numberOfLanguages));
            }

            return stats;
        }

        public void GeneratePDF(string filePath)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.LightBlue.Lighten4);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header()
                         .Row(row =>
                         {
                             row.RelativeItem()
                                 .Column(column =>
                                 {
                                     column.Item().Text("BookingApp").FontSize(24).SemiBold();
                                     column.Spacing(5);
                                     column.Item().Text("Marka Kraljevica 18, Novi Sad").FontSize(14);
                                     column.Spacing(5);
                                     column.Item().Text("Tel: 064-21-11-111").FontSize(14);
                                     //column.Item().Text("Report creation date: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm")).FontSize(14);
                                 });

                             row.ConstantItem(100)
                                 .Image("../../../Resources/Images/booking.png");
                         });

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(10);
                            x.Item().Text("Report for all tour requests in 2024.");
                            x.Item().Padding(10).Table(table =>
                            {
                                // Definisanje kolona
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text("Month").SemiBold();
                                    header.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text("Number of Request").SemiBold();
                                    header.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text("Number of Accepted Request").SemiBold();
                                    header.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text("Number of Not Accepted Request").SemiBold();
                                });

                                foreach (var month in GetRequestsMonths())
                                {
                                    table.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text(month);
                                    foreach (var request in GetRequestByMonth(tourRequestRepository.GetAll(), 2024))
                                    {
                                        if (request.Item1 == month)
                                        {
                                            table.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text(request.Item2);
                                        }
                                    }
                                    int a = 0;
                                    foreach (var request in GetRequestByMonth(GetAcceptedRequest(), 2024))
                                    {
                                        if (request.Item1 == month)
                                        {
                                            table.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text(request.Item2);
                                            a = 1;
                                        }
                                    }
                                    if (a == 0)
                                    {
                                        table.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text(0);

                                    }
                                    a = 0;
                                    foreach (var request in GetRequestByMonth(GetNotAcceptedRequest(), 2024))
                                    {
                                        if (request.Item1 == month)
                                        {
                                            table.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text(request.Item2);
                                            a = 1;
                                        }
                                    }
                                    if (a == 0)
                                    {
                                        table.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text(0);

                                    }
                                }
                                
                            });

                        });

                    page.Footer()
                        .Row(row =>
                        {
                            row.RelativeItem().AlignLeft().AlignBottom().Text("Date: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm")).FontSize(14);

                            row.RelativeItem()
                                .AlignRight()
                                .Column(column =>
                                {
                                    column.Item().AlignRight().Element(container =>
                                    {
                                        container.AlignRight().AlignBottom()
                                            .Width(100)   
                                            .Height(50) 
                                            .Image("../../../Resources/Images/signature.png");
                                    });
                                    column.Item().Text("______________").AlignRight();
                                    column.Item().Text("Milos").AlignCenter().FontSize(14);
                                });
                        });
                });

            })
            .GeneratePdf(filePath);
        }
        
        public List<TourRequest> GetAcceptedRequest()
        {
            List<TourRequest> tourRequests = new List<TourRequest>();
            List<TourRequest> allRequests = tourRequestRepository.GetAll();
            foreach (TourRequest request in allRequests)
            {
                if (request.Status == TourRequestStatus.Accepted)
                {
                    tourRequests.Add(request);
                }
            }
            return tourRequests;
        }

        public List<TourRequest> GetNotAcceptedRequest()
        {
            List<TourRequest> tourRequests = new List<TourRequest>();
            List<TourRequest> allRequests = tourRequestRepository.GetAll();
            foreach (TourRequest request in allRequests)
            {
                if (request.Status != TourRequestStatus.Accepted)
                {
                    tourRequests.Add(request);
                }
            }
            return tourRequests;
        }

        public List<string> GetRequestsMonths()
        {
            List<string> months = new List<string>();
            foreach(var request in GetRequestByMonth(tourRequestRepository.GetAll(), 2024))
            {
                months.Add(request.Item1);
            }
            return months;
        }

        public List<TourRequest> GetAllRequestByComplexRequestId(int id)
        {
            List<TourRequest> requests = new List<TourRequest>();
            foreach(var request in tourRequestRepository.GetAll())
            {
                if(request.ComplexTourRequestId == id)
                {
                    requests.Add(request);
                }
            }
            return requests;
        }

        public List<DateTime> GetFreeAppointments(int id, int guideId)
        {
            List<DateTime> dates = new List<DateTime>();
            TourRequest request = tourRequestRepository.GetById(id);
            DateTime dateFrom = request.DateFrom.ToDateTime(TimeOnly.MinValue);
            DateTime dateTo = request.DateTo.ToDateTime(TimeOnly.MinValue);
            dateFrom = dateFrom.AddHours(8);
            dateTo = dateTo.AddHours(20);
            while (dateFrom <= dateTo)
            {
                if(IsOtherAccepted(dateFrom) && AmIFree(dateFrom, guideId))
                {
                    dates.Add(dateFrom);
                }
                dateFrom = dateFrom.AddHours(1);
                if(dateFrom.Hour == 21)
                    dateFrom = dateFrom.AddHours(11);
            }
            return dates;
        }

        private bool IsOtherAccepted(DateTime date)
        {
            foreach(var request in tourRequestRepository.GetAll())
            {
                if(request.DateIfAccepted == date)
                {
                    return false;
                }
            }
            return true;
        }
        private bool AmIFree(DateTime date, int guideId)
        {
                SearchTourRequestService searchTourRequestService = new SearchTourRequestService(new TourRequestService(Injector.Injector.CreateInstance<ITourRequestRepository>(),
                new TourRequestGuestService(Injector.Injector.CreateInstance<ITourRequestGuestRepository>())),
                new LocationService(Injector.Injector.CreateInstance<ILocationRepository>()),
                new StartTourDateService(Injector.Injector.CreateInstance<IStartTourDateRepository>()),
                new TourService(Injector.Injector.CreateInstance<ITourRepository>(),
                new StartTourDateService(Injector.Injector.CreateInstance<IStartTourDateRepository>()),
                new TourRequestService(Injector.Injector.CreateInstance<ITourRequestRepository>(),
                new TourRequestGuestService(Injector.Injector.CreateInstance<ITourRequestGuestRepository>()))),
                new NotificationService(Injector.Injector.CreateInstance<INotificationRepository>()));
            return searchTourRequestService.IsDateValid(date, guideId);
        }
        public void Accept(TourRequest tourRequest, int guideId, DateTime date)
        {
                SearchTourRequestService searchTourRequestService = new SearchTourRequestService(new TourRequestService(Injector.Injector.CreateInstance<ITourRequestRepository>(),
                new TourRequestGuestService(Injector.Injector.CreateInstance<ITourRequestGuestRepository>())),
                new LocationService(Injector.Injector.CreateInstance<ILocationRepository>()),
                new StartTourDateService(Injector.Injector.CreateInstance<IStartTourDateRepository>()),
                new TourService(Injector.Injector.CreateInstance<ITourRepository>(),
                new StartTourDateService(Injector.Injector.CreateInstance<IStartTourDateRepository>()),
                new TourRequestService(Injector.Injector.CreateInstance<ITourRequestRepository>(),
                new TourRequestGuestService(Injector.Injector.CreateInstance<ITourRequestGuestRepository>()))),
                new NotificationService(Injector.Injector.CreateInstance<INotificationRepository>()));
                searchTourRequestService.Accept(tourRequest, guideId, date);
        }

        public DateOnly GetFurthestDate(int complexRequestId)
        {
            List<TourRequest> list = GetAll().Where(r => r.ComplexTourRequestId == complexRequestId).ToList();
            DateOnly furthest = list[0].DateTo;
            foreach(var item in list)
            {
                if (item.DateTo > furthest)
                    furthest = item.DateTo;
            }
            return furthest;
        }
    }
}
