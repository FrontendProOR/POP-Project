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
                _viewModel.UserType = UserTypeCB.SelectedItem.ToString();//user.UserType

                UserTypeCB.SelectedItem = user.UserType;
                UserTypeCB.IsEnabled = false;
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
                UserType = new UserType { Name = (string)((ComboBoxItem)UserTypeCB.SelectedItem).Content }

            };

            
            userRepository.Insert(user);
            this.Close();
        }


    }
}
