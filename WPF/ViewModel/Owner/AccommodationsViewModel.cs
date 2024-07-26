using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.DTO;
using BookingApp.WPF.View.Owner;
using BookingApp.Domain.Models;
using System.Windows.Navigation;
using BookingApp.Utilities;
using BookingApp.Application.UseCases;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using ceTe.DynamicPDF.PageElements.BarCoding;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using ceTe.DynamicPDF;
namespace BookingApp.WPF.ViewModel.Owner
{
    public class AccommodationsViewModel
    {
        public ObservableCollection<AccommodationDTO> Accommodations { get; set; }
        public AccommodationsPage page;
        private readonly OwnerWindow parentWindow;
        public AccommodationsService accommodationsService;
        public AccommodationReservationService accommodationReservationService;
        public StatsService statsService;
        public AccommodationDTO SelectedAccommodation { get; set; }
        public RelayCommand RegistrationAccommodationCommand { get; }
        public RelayCommand RateGuestCommand { get; }
        public RelayCommand OwnerProfileCommand { get; }
        public RelayCommand RequestCommand { get; }
        public RelayCommand RenovatingCommand { get; }
        public RelayCommand ReviewCommand { get; }
        public RelayCommand HomePageCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand ViewStatsCommand { get; }
        public RelayCommand AdvicesCommand { get; }
        public RelayCommand ForumCommand { get; }
        public RelayCommand ReportCommand { get; }
        public AccommodationsViewModel(AccommodationsPage page, OwnerWindow parentWindow)
        {
            this.page = page;
            this.parentWindow = parentWindow;
            Accommodations = new ObservableCollection<AccommodationDTO>();
            accommodationsService = new AccommodationsService();
            accommodationReservationService = new AccommodationReservationService();
            statsService = new StatsService();
            ShowAccommodations();
            RegistrationAccommodationCommand = new RelayCommand(RegistrationAccommodationExecute);
            RateGuestCommand = new RelayCommand(RateGuestExecute);
            OwnerProfileCommand = new RelayCommand(OwnerProfileExecute);
            RequestCommand = new RelayCommand(RequestExecute);
            RenovatingCommand = new RelayCommand(RenovatingExecute);
            ReviewCommand = new RelayCommand(ReviewExecute);
            HomePageCommand = new RelayCommand(HomePageExecute);
            DeleteCommand = new RelayCommand(DeleteExecute, CanDelete);
            ViewStatsCommand = new RelayCommand(ViewStatsExecute, CanExecute);
            AdvicesCommand = new RelayCommand(AdvicesExecute);
            ForumCommand = new RelayCommand(ForumExecute);
            ReportCommand = new RelayCommand(ReportExecute,CanExecute);
        }
        public void ShowAccommodations()
        {
            Accommodations.Clear();
            Accommodations = new ObservableCollection<AccommodationDTO>(accommodationsService.GetAll());
        }
        private bool CanExecute()
        {
            return SelectedAccommodation != null && accommodationReservationService.isExist(SelectedAccommodation.Id);
        }
        private bool CanDelete()
        {
            return SelectedAccommodation != null;
        }
        public void RegistrationAccommodationExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RegistrationAccommodationForm(parentWindow));
        }
        public void RateGuestExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RateGuestPage(parentWindow));
        }
        public void OwnerProfileExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new OwnerProfilePage(parentWindow));
        }
        public void RequestExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RequestPage(parentWindow));
        }
        public void RenovatingExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new RenovatingPage(parentWindow));
        }
        public void ReviewExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new ReviewPage(parentWindow));
        }
        public void HomePageExecute()
        {
            parentWindow.Show();
            parentWindow.Activate();
            page.NavigationService.Navigate(null);
        }
        public void DeleteExecute()
        {
            Accommodation accomodation = accommodationsService.GetById(SelectedAccommodation.Id);
            accomodation.IsClosed = true;
            accommodationsService.Update(accomodation);
            Accommodations.Remove(SelectedAccommodation);
        }
        public void ViewStatsExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new ViewStatsPage(parentWindow, SelectedAccommodation));
        }
        public void AdvicesExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new AdvicePage(parentWindow));
        }
        public void ForumExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.Navigate(new ForumPage(parentWindow));
        }
        private void ReportExecute()
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF file (*.pdf)|*.pdf";
            saveFileDialog.DefaultExt = "pdf";
            saveFileDialog.AddExtension = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                Document document = new Document();
                ceTe.DynamicPDF.Page page = new ceTe.DynamicPDF.Page(PageSize.Letter, PageOrientation.Portrait, 33.0f);
                document.Pages.Add(page);
                GenerateBackgroundLogo(page);
                GeneratePDFText(page);
                GenerateReportTable(page);

                Dictionary<int, double> busyness = statsService.GetBusyYears(SelectedAccommodation);
                double maxBusyness = busyness.Values.Max();
                int busyYear = busyness.First(kvp => kvp.Value == maxBusyness).Key;

                string busyLabel = ((Properties.Settings.Default.currentLanguage == "en-US") ? "Most busy year: " : "Najzauzetija godina: ") + busyYear.ToString();
                Label busy = new Label(busyLabel, -50, 230, 200, 100, Font.HelveticaBold, 12, TextAlign.Right);
                page.Elements.Add(busy);

                string signatureLabel = (Properties.Settings.Default.currentLanguage == "en-US") ? "Owner" : "Vlasnik";
                Label signature = new Label(signatureLabel, 280, 600, 220, 100, Font.Helvetica, 12, TextAlign.Right);
                page.Elements.Add(signature);

                string signatureOwner = (Properties.Settings.Default.currentLanguage == "en-US") ? "Nikola Milivojevic" : "Nikola Milivojevic";
                Label signatureOwnerManufacture = new Label(signatureOwner, 320, 630, 220, 100, Font.HeiseiKakuGothicW5, 12, TextAlign.Right);
                page.Elements.Add(signatureOwnerManufacture);
                
                string pdfPath = saveFileDialog.FileName;
                document.Draw(pdfPath);

                Process.Start(new ProcessStartInfo()
                {
                    FileName = pdfPath,
                    UseShellExecute = true
                });
            }
        }
        private void GenerateBackgroundLogo(ceTe.DynamicPDF.Page page)
        {
            BackgroundImage backgroundImage = new BackgroundImage("../../../Resources/Images/pdfback.jpg");
            backgroundImage.UseMargins = true;
            page.Elements.Add(backgroundImage);

            ceTe.DynamicPDF.PageElements.Image image = new ceTe.DynamicPDF.PageElements.Image("../../../Resources/Images/booking.png", 10, 20);
            image.Width = 100;
            image.Height = 100;
            page.Elements.Add(image);
        }
        private void GeneratePDFText(ceTe.DynamicPDF.Page page)
        {
            string titleLabel = "Booking Company";
            Label label = new Label(titleLabel, 130, 20, 304, 100, Font.Helvetica, 22, TextAlign.Center);
            page.Elements.Add(label);

            string reportLabel = ((Properties.Settings.Default.currentLanguage == "en-US") ? "Report for accommodation : " : "Izveštaj za smeštaj : ") + (SelectedAccommodation.Name);
            Label report = new Label(reportLabel, 100, 130, 364, 100, Font.HelveticaBold, 20, TextAlign.Center);
            page.Elements.Add(report);

            string dateLabel = ((Properties.Settings.Default.currentLanguage == "en-US") ? "report created " + DateTime.Now.ToString() : "izveštaj kreiran : " + DateTime.Now.ToString("dd.MM.yyyy u HH:mm "));
            Label date = new Label(dateLabel, 120, 180, 304, 100, Font.Helvetica, 16, TextAlign.Center);
            page.Elements.Add(date);
        }
        private void GenerateReportTable(ceTe.DynamicPDF.Page page)
        {
            Table2 table = new Table2(20, 260, 500, 300);
            Column2 firstColumn = table.Columns.Add(100);
            firstColumn.CellDefault.Align = TextAlign.Center;
            table.Columns.Add(100);
            table.Columns.Add(100);
            table.Columns.Add(100);
            table.Columns.Add(100);
            Row2 row = table.Rows.Add(20, Font.Helvetica, 12, Grayscale.Black, Grayscale.Gray);
            row.CellDefault.Align = TextAlign.Center;
            row.CellDefault.VAlign = VAlign.Center;

            if ((Properties.Settings.Default.currentLanguage == "en-US"))
            {
                row.Cells.Add("Year");
                row.Cells.Add("Reserved");
                row.Cells.Add("Canceled");
                row.Cells.Add("Rescheduled");
                row.Cells.Add("Renovate Advices");
            }
            else
            {
                row.Cells.Add("Godina");
                row.Cells.Add("Rezervirsano");
                row.Cells.Add("Otkazano");
                row.Cells.Add("Pomereno");
                row.Cells.Add("Preporuke renoviranja");
            }
            var stats = statsService.GetStatistics(SelectedAccommodation);
            foreach (var stat in stats)
            {
                Row2 statRow = table.Rows.Add(20, Font.Helvetica, 10, Grayscale.Black, Grayscale.White);
                statRow.CellDefault.Align = TextAlign.Center;
                statRow.CellDefault.VAlign = VAlign.Center;
                statRow.Cells.Add(stat.Year.ToString());
                statRow.Cells.Add(stat.Reserved.ToString());
                statRow.Cells.Add(stat.Canceled.ToString());
                statRow.Cells.Add(stat.Rescheduled.ToString());
                statRow.Cells.Add(stat.Advices.ToString());
            }
            page.Elements.Add(table);
        }
    }
}

