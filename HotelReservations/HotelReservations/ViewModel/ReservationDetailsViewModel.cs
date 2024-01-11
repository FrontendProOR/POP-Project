using HotelReservations.Model;

namespace HotelReservations.Windows
{
    public class ReservationDetailsViewModel
    {
        public string ReservationInfo { get; }
        public string RoomInfo { get; }
        public List<Guest> GuestList { get; }

        public ReservationDetailsViewModel(Reservation selectedReservation, Room associatedRoom, List<Guest> associatedGuests)
        {
            ReservationInfo = $"Reservation ID: {selectedReservation.Id}, Total Price: {selectedReservation.TotalPrice}";
            RoomInfo = $"Room Number: {associatedRoom.RoomNumber}, Room Type: {associatedRoom.RoomType?.Name}";
            GuestList = associatedGuests;
        }
    }
}
