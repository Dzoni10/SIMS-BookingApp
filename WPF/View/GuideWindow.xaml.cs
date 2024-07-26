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
using System.Windows.Shapes;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using iText.IO.Image;
using BookingApp.WPF.ViewModel;
using BookingApp.Domain.Models;

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for GuideWindow.xaml
    /// </summary>
    public partial class GuideWindow : Window
    {
        GuideViewModel guideViewModel;
        public GuideWindow(User user)
        {
            InitializeComponent();
            guideViewModel = new GuideViewModel(user);
            this.DataContext = guideViewModel;

        }
        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            guideViewModel.ComboBoxSelectionChanged((int)YearsComboBox.SelectedItem);
        }
    }
}
