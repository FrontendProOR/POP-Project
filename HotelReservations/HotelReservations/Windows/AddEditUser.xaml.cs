using HotelReservations.Model;
using HotelReservations.Model.HotelReservations.Model;
using HotelReservations.Repository;
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
        public AddEditUser(User user = null)
        {
            InitializeComponent();
            _viewModel = new UserViewModel();

            userRepository = new UserRepository();

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
            // Create a User object with the data from the ViewModel
            User user = new User
            {
                Name = _viewModel.Name,
                Surname = _viewModel.Surname,
                Username = _viewModel.Username,
                JMBG = _viewModel.JMBG,
                Password = UserPassword.Password,
                //UserType = new UserType { Name = (string)((ComboBoxItem)UserTypeCB.SelectedItem).Content }
                UserType = new UserType { Name = UserTypeCB.SelectedItem.ToString() }

            };

            User existingUser = userRepository.GetUserById(user.Id);

            if (existingUser == null)
            {
                userRepository.Insert(user);
                this.Close();
            }
            else
            {
                userRepository.Update(user);
                this.Close();
            }
        }
        private void CancelBtn_Click(object sender,RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
