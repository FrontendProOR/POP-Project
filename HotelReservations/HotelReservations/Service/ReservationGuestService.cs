using HotelReservations.Model;
using HotelReservations.Repository;
using System.Data.SqlClient;

namespace HotelReservations.Service
{
    public class ReservationGuestService
    {
        ReservationGuestRepository reservationGuestRepository;
        public ReservationGuestService()
        {
            reservationGuestRepository = new ReservationGuestRepository();
        }

        public List<ReservationGuest> GetAllReservationGuests()
        {
            //return Hotel.GetInstance().ReservationGuests;
            return reservationGuestRepository.GetAll();
        }

        public void SaveReservationGuest(ReservationGuest reservationGuest)
        {

            //reservationGuestRepository.Insert(reservationGuest);
            //Hotel.GetInstance().ReservationGuests.Add(reservationGuest);
            try
            {
                reservationGuestRepository.Insert(reservationGuest);
            }
            catch (SqlException ex)
            {
                // Log the exception or handle it as appropriate for your application
                //Console.WriteLine($"SQL Exception: {ex.Message}");
                //Log.Error($"SQL Exception: {ex.Message}");
                throw ex;
                // Optionally, you can choose not to rethrow the exception or take other actions as needed
            }
        }
        public List<Guest> GetGuestsByReservationId(int reservationId)
        {
            return reservationGuestRepository.GetGuestsByReservationId(reservationId);
        }
    }
}
