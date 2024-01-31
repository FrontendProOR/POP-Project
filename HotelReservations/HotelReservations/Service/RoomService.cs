using HotelReservations.Model;
using HotelReservations.Repository;

namespace HotelReservations.Service
{
    public class RoomService
    {
        private IRoomRepository roomRepository;
        public RoomService()
        {
            roomRepository = new RoomRepository();
        }

        public List<Room> GetAllRooms()
        {
            return roomRepository.GetAll();
        }

        public List<Room> GetSortedRooms()
        {
            var rooms = Hotel.GetInstance().Rooms;
            rooms.Sort((r1, r2) => r1.RoomNumber.CompareTo(r2.RoomNumber));
            return rooms;
        }

        public List<Room> GetRoomsByRoomType(string roomTypeName)
        {
            var rooms = Hotel.GetInstance().Rooms;
            var filteredRooms = rooms.FindAll((r) => r.RoomType.Name.ToLower().Contains(roomTypeName.ToLower()));
            return filteredRooms;
        }

        public List<Room> GetRoomsByIsActive(bool isActive)
        {
            var rooms = Hotel.GetInstance().Rooms;
            var filteredRooms = rooms.FindAll((r) => r.IsActive == isActive);
            return filteredRooms;
        }

        public List<Room> GetAllRoomsByRoomNumber(string startingWith)
        {
            var rooms = Hotel.GetInstance().Rooms;
            var filteredRooms = rooms.FindAll((r) => r.RoomNumber.StartsWith(startingWith));
            return filteredRooms;
        }

        //public List<Room> GetFilteredRooms(string roomNumber, string roomTypeName, bool isActive)
        //{
        //    var rooms = Hotel.GetInstance().Rooms;

        //    var filteredRooms = rooms.Where(r =>
        //        (string.IsNullOrEmpty(roomNumber) || r.RoomNumber.StartsWith(roomNumber)) &&
        //        (string.IsNullOrEmpty(roomTypeName) || r.RoomType.Name.ToLower().Contains(roomTypeName.ToLower())) &&
        //        (!isActive || r.IsActive)&&(r.IsDeleted != true)
        //    ).ToList();

        //    return filteredRooms;
        //}
        public List<Room> GetFilteredRooms(string roomNumber, string roomTypeName, bool isActive)
        {
            var rooms = Hotel.GetInstance().Rooms;

            var filteredRooms = rooms.AsParallel().Where(r =>
                (string.IsNullOrEmpty(roomNumber) || r.RoomNumber.StartsWith(roomNumber)) &&
                (string.IsNullOrEmpty(roomTypeName) || r.RoomType.Name.ToLower().Contains(roomTypeName.ToLower())) &&
                (!isActive || r.IsActive) && (r.IsDeleted != true)
            ).ToList();

            return filteredRooms;
        }


        public void SaveRoom(Room room)
        {
            if (room.Id == 0)
            {
                room.Id = roomRepository.Insert(room);
                Hotel.GetInstance().Rooms.Add(room);
            }
            else
            {
                roomRepository.Update(room);
                var index = Hotel.GetInstance().Rooms.FindIndex(r => r.Id == room.Id);
                Hotel.GetInstance().Rooms[index] = room;
            }
        }
        public List<RoomType> GetAllRoomTypes()
        {
            
            return roomRepository.GetAllRoomTypes();
        }
        public void PermanentDeleteRoom(int roomId)
        {
            
            //var roomToRemove = Hotel.GetInstance().Rooms.Find(r => r.Id == roomId);
            //if (roomToRemove != null)
            //{
                //Hotel.GetInstance().Rooms.Remove(roomToRemove);
                RoomRepository roomRepository = new RoomRepository();
                roomRepository.PermanentDeleteRoom(roomId);
                
            //}
        }
        public string GetRoomTypeNameByRoomNumber(string roomNumber)
        {
            var rooms = Hotel.GetInstance().Rooms;
            var room = rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);

            return room?.RoomType?.Name ?? string.Empty;
        }

        public Room GetRoomByReservation(Reservation reservation)
        {
            // Implement logic to retrieve the room associated with the reservation
            return roomRepository.GetRoomByRoomNumber(reservation.RoomNumber);
        }
        public List<Room> GetAllRoomsWithCheckedReservations()
        {
            return roomRepository.GetAllCurrentlyActive();
        }
        public void DeleteRoom(int roomId)
        {
            
            //var roomToDelete = Hotel.GetInstance().Rooms.Find(r => r.Id == roomId);
            //if (roomToDelete != null)
            //{
                //roomToDelete.IsActive = false; // Assuming IsActive is a property in your Room class
                roomRepository.Delete(roomId);
            //}
        }
        public List<Room> GetFilteredRoomsByRoomNumberAndRoomType(string roomNumber, string roomTypeName, bool isActive)
        {
            var rooms = Hotel.GetInstance().Rooms;

            var filteredRooms = rooms.Where(r =>
                (string.IsNullOrEmpty(roomNumber) || r.RoomNumber.ToLower().Contains(roomNumber.ToLower())) &&
                (string.IsNullOrEmpty(roomTypeName) || r.RoomType.Name.ToLower().Contains(roomTypeName.ToLower())) &&
                (!r.IsDeleted)
            ).ToList();

            return filteredRooms;
        }

    }
}
