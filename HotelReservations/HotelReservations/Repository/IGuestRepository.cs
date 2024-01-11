using HotelReservations.Model;

namespace HotelReservations.Repository
{
    public interface IGuestRepository
    {
        List<Guest> GetAll();
        int Insert(Guest guest);
        void Update(Guest guest);
        void Save(List<Guest> guestList);
        Guest GetGuestById(int guestId);
    }
}
