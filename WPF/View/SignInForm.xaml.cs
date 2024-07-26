using BookingApp.Repository;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using BookingApp.Domain.Models;
using System;

namespace BookingApp.WPF.View
{
    public partial class SignInForm : Window
    {

        //private readonly UserRepository _repository;

        private UserRepository userRepository;

        public string Username { get; set; }

        public string Password { get; set; }

        public SignInForm()
        {
            InitializeComponent();
            this.DataContext = this;
            //_repository = new UserRepository();
            userRepository = new UserRepository();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = null;
            //User user = _repository.GetByUsername(Username);
            if (!Username.Equals("") || !TextBoxPassword.Password.Equals(""))
            {
                user = userRepository.Login(TextBoxPassword.Password, Username);
                if (user == null)
                {
                    MessageBox.Show("User with this ceredentials doesn't exist");
                    return; 
                }

            }

            user = userRepository.Login(TextBoxPassword.Password, Username);

                if (user.Role == USERROLE.Owner)
            {
                Owner.OwnerWindow ownerWindow = new Owner.OwnerWindow(Username);
                ownerWindow.Show();
                this.Close();
            }
            else if (user.Role == USERROLE.Guide)
            {
                GuideWindow guideWindow = new GuideWindow(user);
                //FollowTourWindow guideWindow = new FollowTourWindow(user.Id);
                guideWindow.Show();
                this.Close();
            }
            else if (user.Role == USERROLE.Guest)
            {
                GuestWindow guestWindow = new GuestWindow(user);
                guestWindow.Show();
                this.Close();
            }
            else if(user.Role == USERROLE.Tourist)
            {
                TouristWindow touristWindow = new TouristWindow(user);
                touristWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("User with this ceredentials doesn't exist");
            }

        }

    }
}
