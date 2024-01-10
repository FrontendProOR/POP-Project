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

    }
}
