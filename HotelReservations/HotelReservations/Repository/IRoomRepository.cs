using HotelReservations.Model;

namespace HotelReservations.Repository
{
    public interface IRoomRepository
    {
        Room GetRoomByRoomNumber(string roomNumber);
        List<Room> GetAll();
        List<Room> GetAllCurrentlyActive();
        List<RoomType> GetAllRoomTypes();
        int Insert(Room room);
        void Update(Room room);
        void Save(List<Room> roomList);
        void PermanentDeleteRoom(int id); 
        void Delete(int id);
    }
}
