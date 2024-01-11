using HotelReservations.Model;

namespace HotelReservations.Repository
{
    public interface IReservationGuestRepository
    {
        List<ReservationGuest> GetAll();
        //List<(int ReservationId, int GuestId)> GetAll();
        int Insert(ReservationGuest reservationGuest);
        void Update(ReservationGuest reservationGuest);
        void Save(List<ReservationGuest> reservationGuests);
        List<Guest> GetGuestsByReservationId(int reservationId);
    }
}
