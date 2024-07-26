using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookingApp.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Application.UseCases;
using BookingApp.WPF.ViewModel;

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for ReviewControl.xaml
    /// </summary>
    public partial class ReviewControl : UserControl
    {
        private ReviewControlViewModel reviewControlViewModel;
        public ReviewControl(int id, string tourist, int guidesKnowledge, int guidesLanguageAbility, int tourExcitement, string comment, string hint, string tour, bool valid)
        {
            InitializeComponent();
            reviewControlViewModel = new ReviewControlViewModel(id, tourist, guidesKnowledge, guidesLanguageAbility, tourExcitement, comment, hint, tour, valid);   
            this.DataContext = reviewControlViewModel;
        }
        public void ReportClick(object sender, RoutedEventArgs e)
        {
            reviewControlViewModel.Report();
        }
    }
}
