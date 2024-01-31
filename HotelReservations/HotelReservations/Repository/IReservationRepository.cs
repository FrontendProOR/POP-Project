using HotelReservations.Model;

namespace HotelReservations.Repository
{
    public interface IReservationRepository
    {
        Room GetRoomByNumber(string roomNumber);
        List<Reservation> GetAll();
        int Insert(Reservation reservation);
        void Update(Reservation reservation);
        void Save(List<Reservation> reservationList);
        List<Reservation> GetCurrentAndFutureReservations();
        //List<Reservation> GetCurrentAndFutureReservations(ReservationGuest reservationGuest);
        Reservation GetById(int reservationId);
    }
}
