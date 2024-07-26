using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;
using BookingApp.Repository;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class TourReservationService
    {
        private ITourReservationRepository tourReservationRepository;
        private TourService tourService;
        public TourReservationService(ITourReservationRepository tourReservationRepository, TourService tourService) 
        {
            this.tourReservationRepository = tourReservationRepository;
            this.tourService = tourService;
        }
        public List<TourReservation> GetAll()
        {
            return tourReservationRepository.GetAll();
        }

        public List<TourReservation> GetByStartTourDateId(int id)
        {
            return tourReservationRepository.GetAll()
            .Where(tourReservation => tourReservation.StartTourDateId == id)
            .ToList();

        }

        public TourReservation Save(TourReservation tourReservation)
        {
            return tourReservationRepository.Save(tourReservation);
        }

        public void GeneratePDF(string filePath, StartTourDate startTourDate, List<TourGuest> guests, Voucher voucher)
        {
            Tour tour = tourService.GetById(startTourDate.TourId);
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
                            x.Item().Text("Information about your reservation:");
                            x.Item().Text($"Tour Name: {tour.Name}");
                            x.Item().Text($"Location: {tour.Location.City}, {tour.Location.State}");
                            x.Item().Text($"Language: {tour.Language.Name}");
                            x.Item().Text($"Description: {tour.Description}");
                            x.Item().Text($"Duration: {tour.Duration}h");
                            x.Item().Text($"Starting time: {startTourDate.Date.ToString("dd.MM.yyyy HH:mm")}");
                            x.Item().Text($"Your guide: Milos");
                            if (voucher.Id == 0)
                                x.Item().Text($"Voucher used: No");
                            else
                                x.Item().Text($"Voucher used: Voucher #{voucher.Id}");
                            x.Item().Text("People attending:");
                            x.Item().Padding(10).Table(table =>
                            {
                                // Definisanje kolona
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text("Full name").SemiBold();
                                    header.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text("Age").SemiBold();
                                });

                                foreach (var guest in guests)
                                {
                                    table.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text(guest.FullName);
                                    table.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text(guest.Age.ToString());                                   
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
                                            .Image("../../../Resources/Images/smiling-face-with-sunglasses.png");
                                    });
                                    column.Item().Text("See you at the tour!").AlignCenter().FontSize(14);
                                });
                        });
                });

            })
            .GeneratePdf(filePath);
        }
    }
}
