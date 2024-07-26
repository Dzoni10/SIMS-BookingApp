using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BookingApp.Model;
using System.Resources;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModel;
using BookingApp.Application.UseCases;

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for CreateTourForm.xaml
    /// </summary>
    public partial class CreateTourForm : Window
    {
        public CreateTourForm(int userId)
        {
            InitializeComponent();
            DataContext = new CreateTourViewModel(userId);
        }

    }
}
