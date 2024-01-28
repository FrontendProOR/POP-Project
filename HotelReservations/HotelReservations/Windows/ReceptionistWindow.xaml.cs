using System.Windows;

namespace HotelReservations.Windows
{
    /// <summary>
    /// Interaction logic for ReceptionistWindow.xaml
    /// </summary>
    public partial class ReceptionistWindow : Window
    {
        public ReceptionistWindow()
        {
            InitializeComponent();
        }
        private void OpenReservationsWindow(object sender, RoutedEventArgs e)
        {
            // Create and show the Reservations window
            var reservationsWindow = new Reservations();
            reservationsWindow.Show();
        }

        private void OpenGuestsWindow(object sender, RoutedEventArgs e)
        {
            // Create and show the Guests window
            var guestsWindow = new Guests(); // Replace with the actual name of your Guests window class
            guestsWindow.Show();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
