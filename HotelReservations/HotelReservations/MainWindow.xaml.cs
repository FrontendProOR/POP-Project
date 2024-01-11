using HotelReservations.Windows;
using System.Windows;

namespace HotelReservations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
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
        private void PricesMI_Click(object sender,RoutedEventArgs e)
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
    }
}
