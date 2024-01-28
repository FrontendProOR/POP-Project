using System.Windows;

namespace HotelReservations.Windows
{
    /// <summary>
    /// Interaction logic for adminWindow.xaml
    /// </summary>
    public partial class adminWindow : Window
    {
        public adminWindow()
        {
            InitializeComponent();
        }

        private void RoomsMI_Click(object sender, RoutedEventArgs e)
        {
            var roomsWindow = new Rooms();
            roomsWindow.Show();
        }

        private void UsersMI_Click(object sender, RoutedEventArgs e)
        {
            var usersWindow = new Users();
            usersWindow.Show();
        }

        private void RoomTypesMI_Click(object sender, RoutedEventArgs e)
        {
            var roomTypeWindow = new RoomTypes();
            roomTypeWindow.Show();
        }

        private void PricesMI_Click(object sender, RoutedEventArgs e)
        {
            var pricesWindow = new Prices();
            pricesWindow.Show();
        }

        private void UserTypesMI_Click(object sender, RoutedEventArgs e)
        {
            var userTypesWindow = new UserTypes();
            userTypesWindow.Show();
        }

        private void ReservationsMI_Click(object sender, RoutedEventArgs e)
        {
            var reservationsWindow = new Reservations();
            reservationsWindow.Show();
        }

        private void GuestsMI_Click(object sender, RoutedEventArgs e)
        {
            var guestsWindow = new Guests();
            guestsWindow.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Handle the click event for the menu item
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
