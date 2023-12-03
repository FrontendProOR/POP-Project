using HotelReservations.Model;
using HotelReservations.Model.HotelReservations.Model;
using HotelReservations.Repository;
using HotelReservations.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelReservations.Windows
{
    /// <summary>
    /// Interaction logic for AddEditUser.xaml
    /// </summary>
    public partial class AddEditUser : Window
    {

        private UserViewModel _viewModel;
        private UserRepository userRepository;
        
        private UserService userService;
        public AddEditUser(User user = null)
        {
            InitializeComponent();
            _viewModel = new UserViewModel();

            userRepository = new UserRepository();
            userService = new UserService(); 

            DataContext = _viewModel;
            AdjustWindow(user);
        }

        private void AdjustWindow(User user = null)
        {
            // TODO: Inicijalizovati combobox za selekciju tipa korisnika
            UserTypeCB.Items.Add(typeof(Receptionist).Name);
            UserTypeCB.Items.Add(typeof(Administrator).Name);

            if (user != null)
            {
                Title = "Edit user";

                // Set properties of the ViewModel with existing user data
                _viewModel.UserId = user.Id;
                _viewModel.Name = user.Name;
                _viewModel.Surname = user.Surname;
                _viewModel.Username = user.Username;
                _viewModel.JMBG = user.JMBG;
                _viewModel.Password = user.Password;
                //_viewModel.UserType = UserTypeCB.SelectedItem.ToString();//user.UserType
                if (UserTypeCB != null && UserTypeCB.Items.Count > 0)
                {
                    // Set selected item based on user.UserType
                    //UserTypeCB.SelectedItem = user.UserType?.Name;
                    UserTypeCB.SelectedItem = UserTypeCB.Items.OfType<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == user.UserType?.Name);

                    // Check if selected item is not null before accessing it
                    if (UserTypeCB.SelectedItem != null)
                    {
                        _viewModel.UserType = UserTypeCB.SelectedItem.ToString();
                    }

                    UserTypeCB.IsEnabled = true;//ako postavljas edit da ne menja role 
                }
            }
            else
            {
                Title = "Add user";
            }
        }


        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            User user = new User
            {
                Id = _viewModel.UserId, // Assuming you have a UserId property in your ViewModel
                Name = _viewModel.Name,
                Surname = _viewModel.Surname,
                Username = _viewModel.Username,
                JMBG = _viewModel.JMBG,
                Password = UserPassword.Password,
                UserType = new UserType { Name = UserTypeCB.Text }
            };
            // Initialize an empty message string
            //string validationMessage = "";

            //// Check for null or empty values
            //if (string.IsNullOrEmpty(user.Name) ||
            //    string.IsNullOrEmpty(user.Surname) ||
            //    string.IsNullOrEmpty(user.Username) ||
            //    string.IsNullOrEmpty(user.JMBG) ||
            //    string.IsNullOrEmpty(user.Password) ||
            //    user.UserType == null || string.IsNullOrEmpty(user.UserType.Name))
            //{
            //    validationMessage += "Please fill in all required fields.\n";
            //}

            //// Check for other validations only if there were no null or empty values
            //if (string.IsNullOrEmpty(validationMessage))
            //{
            //    // Validate Name
            //    if (!Regex.IsMatch(user.Name, @"^[a-zA-Z]{2,}$"))
            //    {
            //        validationMessage += "Please enter a valid Name (minimum 3 letters).\n";
            //    }

            //    // Validate Surname
            //    if (!Regex.IsMatch(user.Surname, @"^[a-zA-Z]{2,}$"))
            //    {
            //        validationMessage += "Please enter a valid Surname (minimum 3 letters).\n";
            //    }

            //    // Validate Username
            //    if (!Regex.IsMatch(user.Username, @"^[a-zA-Z]{2,}$"))
            //    {
            //        validationMessage += "Please enter a valid Username (minimum 3 letters).\n";
            //    }

            //    // Validate JMBG
            //    if (!Validation.Validation.IsValidJMBG(user.JMBG))
            //    {
            //        validationMessage += "Please enter a valid JMBG (13 digits, valid format).\n";
            //    }

            //    // Validate Password
            //    if (!Regex.IsMatch(user.Password, @"^.{8,}$"))
            //    {
            //        validationMessage += "Please enter a valid Password (minimum 8 characters).\n";
            //    }

            //    // Validate UserType
            //    if (string.IsNullOrEmpty(user.UserType.Name))
            //    {
            //        validationMessage += "Fill required fields.\n";
            //    }
            //}

            //// Check if there were any validation issues
            //if (!string.IsNullOrEmpty(validationMessage))
            //{
            //    MessageBox.Show(validationMessage, "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrWhiteSpace(user.Name))
            {
                MessageBox.Show("Please enter a valid Name.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(user.Surname) || string.IsNullOrWhiteSpace(user.Surname))
            {
                MessageBox.Show("Please enter a valid Surname.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrWhiteSpace(user.Username))
            {
                MessageBox.Show("Please enter a valid Username.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(user.JMBG) || string.IsNullOrWhiteSpace(user.JMBG))
            {
                MessageBox.Show("Please enter a valid JMBG.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(user.Password) || string.IsNullOrWhiteSpace(user.Password))
            {
                MessageBox.Show("Please enter a valid Password.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (user.UserType == null || string.IsNullOrEmpty(user.UserType.Name) || string.IsNullOrWhiteSpace(user.UserType.Name))
            {
                MessageBox.Show("Please select a valid UserType.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            userService.SaveUser(user);

            DialogResult = true;
            Close();

        }
        private void CancelBtn_Click(object sender,RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
