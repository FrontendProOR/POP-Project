using HotelReservations.Model;
using HotelReservations.Repository;
using System.Windows;

namespace HotelReservations.Service
{
    public class ReservationService
    {
        IReservationRepository reservationRepository;
        ReservationGuestRepository reservationGuestRepository;
        RoomRepository roomRepository;
        PriceRepository priceRepository;
        RoomService roomService;
        public ReservationService()
        {
            reservationRepository = new ReservationRepository();
            reservationGuestRepository = new ReservationGuestRepository();
            roomRepository = new RoomRepository();
            priceRepository = new PriceRepository();
            roomService = new RoomService();
        }

        public List<Reservation> GetAllReservations()
        {
            return reservationRepository.GetAll();
        }

        public List<ReservationGuest> GetAllRGuests()
        {
            return reservationGuestRepository.GetAll();
        }

        public List<Reservation> GetSortedReservations()
        {
            var reservations = Hotel.GetInstance().Reservations;
            reservations.Sort((r1, r2) => r1.TotalPrice.CompareTo(r2.TotalPrice));
            return reservations;
        }

        public void SaveReservation(Reservation reservation)
        {
            if (reservation.Id == 0)
            {
                if (IsRoomAvailable(reservation.RoomNumber, reservation.StartDateTime, reservation.EndDateTime))
                {
                    string roomTypeName = roomService.GetRoomTypeNameByRoomNumber(reservation.RoomNumber);
                    reservation.TotalPrice = CalculateTotalPrice(roomTypeName, reservation.StartDateTime, reservation.EndDateTime);
                    reservationRepository.Insert(reservation);
                    var room = GetRoomByReservation(reservation);
                    if (room != null)
                    {
                        room.IsActive = true;
                        roomRepository.Update(room);
                    }
                }
                else
                {
                    MessageBox.Show("This room is already reserved for the specified period.", "Room Unavailable", MessageBoxButton.OK, MessageBoxImage.Warning);
                    // Handle the situation when the room is already reserved
                }
            }
            else
            {
                reservationRepository.Update(reservation);
                // Handle the situation when updating an existing reservation
            }
            UpdateRoomsBasedOnExpiredReservations();
        }

        public bool IsRoomAvailable(string roomNumber, DateTime startDateTime, DateTime endDateTime)
        {
            // Retrieve existing reservations for the specified room
            var existingReservations = reservationRepository.GetAll().Where(r =>
                r.RoomNumber == roomNumber &&
                r.IsActive &&
                !(endDateTime <= r.StartDateTime || startDateTime >= r.EndDateTime)
            );

            // Check if there are any overlapping reservations
            return !existingReservations.Any();
        }
        public void UpdateRoomsBasedOnExpiredReservations()
        {
            // Get all active reservations
            var activeReservations = reservationRepository.GetAll().Where(r => r.IsActive);

            foreach (var reservation in activeReservations)
            {
                // Check if the reservation has expired
                if (reservation.EndDateTime < DateTime.Now)
                {
                    // Update the corresponding room
                    UpdateRoomBasedOnExpiredReservation(reservation);
                }
            }
        }
        private void UpdateRoomBasedOnExpiredReservation(Reservation reservation)
        {
            // Get the room associated with the reservation
            var room = GetRoomByReservation(reservation);

            // Update room properties based on the expired reservation (example: set room as inactive)
            if (room != null)
            {
                room.IsActive = false;
                roomRepository.Update(room);
            }

            // Mark the reservation as inactive
            reservation.IsActive = false;

            // Update the reservation in the repository
            reservationRepository.Update(reservation);
        }
        //private Room GetRoomByReservation(Reservation reservation)
        //{
        //    // Retrieve the room associated with the reservation
        //    return reservationRepository.GetRoomByNumber(reservation.RoomNumber);
        //}
        private Room GetRoomByReservation(Reservation reservation)
        {
            // Retrieve the room associated with the reservation
            var room = reservationRepository.GetRoomByNumber(reservation.RoomNumber);

            // Ensure that the room and its properties are not null
            if (room != null && room.RoomType != null)
            {
                return room;
            }

            // Handle the situation when the room or its properties are null
            return null;
        }
        public double CalculateTotalPrice(string roomTypeName, DateTime startDateTime, DateTime endDateTime)
        {
            // Retrieve the PriceValue based on the RoomTypeName
            var priceService = new PriceService();
            var priceValue = priceService.GetPriceValueByRoomTypeName(roomTypeName);

            // Calculate total price based on the PriceValue and reservation duration
            double totalPrice = priceValue * (endDateTime - startDateTime).TotalDays;

            return totalPrice;
        }


    }
}
