using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for ComplexTourRequestCard.xaml
    /// </summary>
    public partial class ComplexTourRequestCard : UserControl
    {
        public int ComplexTourRequestId { get; set; }
        public ComplexTourRequestCard(User user, int complexTourRequestId)
        {
            InitializeComponent();
            ComplexTourRequestId = complexTourRequestId;
            DataContext = new ComplexTourRequestCardViewModel(this, user);
        }

        
    }
}
