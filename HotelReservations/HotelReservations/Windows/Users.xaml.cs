using HotelReservations.Model;
using HotelReservations.Model.HotelReservations.Model;
using HotelReservations.Service;
using System;
using System.Windows;

namespace HotelReservations.Windows
{
    public partial class Users : Window
    {
        private UserService userService;

        public Users()
        {
            userService = new UserService();
            InitializeComponent();
            FillData();
        }

        private void FillData()
        {
            try
            {
                List<User> users = userService.GetAllUsers();
                UsersDG.ItemsSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var addUsersWindow = new AddEditUser();
            addUsersWindow.Closed += AddUsersWindow_Closed;
            addUsersWindow.ShowDialog();
        }

        private void AddUsersWindow_Closed(object sender, EventArgs e)
        {
            // Refresh the data after the AddEditUser window is closed
            FillData();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = UsersDG.SelectedItem as User;

            if (selectedUser != null)
            {
                var editUsersWindow = new AddEditUser(selectedUser);
                editUsersWindow.Closed += EditUsersWindow_Closed;
                editUsersWindow.ShowDialog();
            }
        }

        private void EditUsersWindow_Closed(object sender, EventArgs e)
        {
            // Refresh the data after the AddEditUser window is closed
            FillData();
        }
    }
}
