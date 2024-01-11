using HotelReservations.Model;
using System.Windows;

namespace HotelReservations.Windows
{
    public partial class ReservationDetailsWindow : Window
    {
        public ReservationDetailsWindow(Reservation selectedReservation, Room associatedRoom, List<Guest> associatedGuests)
        {
            InitializeComponent();

            // Set DataContext to an instance of a view model with the necessary properties
            DataContext = new ReservationDetailsViewModel(selectedReservation, associatedRoom, associatedGuests);
        }
    }
}
