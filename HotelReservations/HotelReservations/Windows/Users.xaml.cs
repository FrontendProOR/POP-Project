using HotelReservations.Model;
using HotelReservations.Model.HotelReservations.Model;
using HotelReservations.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace HotelReservations.Windows
{
    public partial class Users : Window
    {
        private ICollectionView view;
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
                view = CollectionViewSource.GetDefaultView(users);
                view.Filter = DoFilter;
                UsersDG.ItemsSource = view;
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
            FillData();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDG.SelectedItem != null)
            {
                var selectedUser = UsersDG.SelectedItem as User;

                if (selectedUser != null)
                {
                    var editUserWindow = new AddEditUser(selectedUser);

                    Hide();

                    if (editUserWindow.ShowDialog() == true)
                    {
                        FillData();
                    }

                    Show();
                }
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = UsersDG.SelectedItem as User;

            if (selectedUser == null)
            {
                MessageBox.Show("Please select a user to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show($"Are you sure that you want to delete user {selectedUser.Username}?",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                userService.DeleteUser(selectedUser.Id);

                FillData();
            }
        }

        private void UsernameSearchTB_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                var usernameSearchParam = UsernameSearchTB.Text;

                // Check if the search parameter is empty
                if (string.IsNullOrEmpty(usernameSearchParam))
                {
                    // If empty, reload the full list of users
                    FillData();
                }
                else
                {
                    // Call the GetUsersByUsername method to get filtered users
                    var filteredUsers = userService.GetUsersByUsername(usernameSearchParam);

                    // Update the DataGrid with the filtered users
                    view = CollectionViewSource.GetDefaultView(filteredUsers);
                    view.Filter = DoFilter;
                    UsersDG.ItemsSource = view;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching users: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool DoFilter(object userObject)
        {
            var user = userObject as User;
            // Implement your filtering logic if needed
            return true;
        }
    }
}
