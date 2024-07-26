using BookingApp.Domain.Models;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Org.BouncyCastle.Asn1.Ocsp;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using Org.BouncyCastle.Bcpg;


namespace BookingApp.Application.UseCases
{
    public class ReportGeneratorService
    {
        private AccommodationsService accommodationService;
        private GuestService guestService;
        private AccommodationReservationService accommodationReservationService;
        private ImageService imageService;
        public ReportGeneratorService()
        {
            accommodationService = new AccommodationsService();
            guestService = new GuestService();
            accommodationReservationService = new AccommodationReservationService();
            imageService = new ImageService();
        }
        /*
        public void GenerateReport(AccommodationReservation reservation)
        {
            Accommodation accommodation = accommodationService.GetById(reservation.AccommodationId);
            Guest guest = guestService.GetById(reservation.GuestId);


            GenerateHTMLPage(accommodation, reservation, guest);
            ConvertHtmlToPdf("podaci.html", "report.pdf");
        }
        public void GenerateHTMLPage(Accommodation accommodation, AccommodationReservation reservation, Guest guest)
        {
            string htmlContent = "<html><head><title>Podaci iz C# koda</title></head><body>" +
                 "<h1 style='text-align: center; color: blue;'>Reservation report</h1>" +
                 "<h2 style='text-align: center; color: blue;'> " + accommodation.Name + "</h2>" +
                 "<ul style='list-style-type:none;'>" +
                 "<li><h3 style='border-bottom: 3px solid blue; padding-bottom: 20px;'>Guest name: " + guest.Name + " " + guest.Surname + "</h3></li>" +
                 "<li><h3 style='border-bottom: 3px solid blue; padding-bottom: 20px;'>Room type: " + accommodation.Type + "</h3></li> " +
                 "<li><h3 style='border-bottom: 3px solid blue; padding-bottom: 20px;'>Location: " + accommodation.Location.State + ", " + accommodation.Location.City + "</h3></li> " +
                 "<li><h3 style='border-bottom: 3px solid blue; padding-bottom: 20px;'>Number of guests: " + reservation.GuestCount + "</h3></li> " +
                 "<li><h3 style='border-bottom: 3px solid blue; padding-bottom: 20px;'>Start date: " + reservation.StartDate.ToShortDateString() + "</h3></li> " +
                 "<li><h3 style='border-bottom: 3px solid blue; padding-bottom: 20px;'>End date: " + reservation.EndDate.ToShortDateString() + "</h3></li>  " +
                 "<li><h3 style='border-bottom: 3px solid blue; padding-bottom: 20px;'>Total days: " + reservation.DaysReserved + "</h3></li>  " +
                 "<li><h3 style='border-bottom: 3px solid blue; padding-bottom: 20px;'>Can be canceled to: " + reservation.StartDate.AddDays(-accommodation.CancelationPeriod).ToShortDateString() + "</h3></li> " +

                 "</ul>" + 
                 "</body></html>";

            // Spremanje HTML sadržaja u datoteku
            string filePath = "podaci.html";
            File.WriteAllText(filePath, htmlContent);


            Console.WriteLine("HTML datoteka generirana.");
        }

        static void ConvertHtmlToPdf(string htmlFilePath, string pdfFilePath)
        {
            // Kreiranje PDF dokumenta
            using (FileStream outputStream = new FileStream(pdfFilePath, FileMode.Create))
            {
                using (Document document = new Document())
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, outputStream);
                    document.Open();

                    // Čitanje HTML sadržaja iz datoteke
                    using (TextReader htmlReader = new StreamReader(htmlFilePath))
                    {
                        // Konverzija HTML-a u PDF
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, htmlReader);
                    }
                }
            }
        }
        */


        public void GeneratePDF(string filePath, int accommodationReservationId)
        {
            AccommodationReservation reservation = accommodationReservationService.GetById(accommodationReservationId);
            reservation.Accommodation = accommodationService.GetById(reservation.AccommodationId);
            reservation.Guest = guestService.GetById(reservation.GuestId);

            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor("eff2f7");
                    page.DefaultTextStyle(x => x.FontSize(16));

                    page.Header()
                        .Column
                            (column =>
                            {
                                column.Item().Text("BookingApp").AlignLeft();
                                column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                                column.Item().Text("Accommodation reservation report");
                                column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                                column.Item().Text("Guest: " + reservation.Guest.Name + " " + reservation.Guest.Surname);
                                column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                                column.Item().Text(DateTime.Now.ToString("dd.MM.yyyy HH:mm")).AlignLeft();
                                column.Item().PaddingVertical(40);
                            }
                            );

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .DefaultTextStyle(style => style.FontSize(12))
                        .Column(x =>
                        {
                            x.Spacing(20);

                            x.Item().Padding(10).Table(table =>
                            {
                                // Defining columns
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text("Accommodation Name").SemiBold();
                                    header.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text("Location").SemiBold();
                                    header.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text("Number of guests").SemiBold();
                                    header.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text("Days reserved").SemiBold();
                                    header.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text("Start Date").SemiBold();
                                    header.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text("End Date").SemiBold();
                                });

                                table.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text(reservation.Accommodation.Name);
                                table.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text(reservation.Accommodation.Location.State + ", " + reservation.Accommodation.Location.City);
                                table.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text(reservation.GuestCount);
                                table.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text(reservation.DaysReserved);
                                table.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text(reservation.StartDate.ToString("dd.MM.yyyy"));
                                table.Cell().Border(2).Padding(5).AlignCenter().AlignMiddle().Text(reservation.EndDate.ToString("dd.MM.yyyy"));

                            });

                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });

            })
            .GeneratePdf(filePath);
        }


    }
}
