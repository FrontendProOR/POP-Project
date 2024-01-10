using HotelReservations.Model;
using HotelReservations.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // Implement logic to fetch room types from the database
            // For example, using a repository method.
            return roomRepository.GetAllRoomTypes();
        }
        public void PermanentDeleteRoom(int roomId)
        {
            // Implement logic for permanently deleting a room
            // For example, remove it from the repository and the in-memory list
            //var roomToRemove = Hotel.GetInstance().Rooms.Find(r => r.Id == roomId);
            //if (roomToRemove != null)
            //{
                //Hotel.GetInstance().Rooms.Remove(roomToRemove);
                RoomRepository roomRepository = new RoomRepository();
                roomRepository.PermanentDeleteRoom(roomId);
                
            //}
        }

        public void DeleteRoom(int roomId)
        {
            // Implement logic for logically deleting a room
            // For example, update the room status to inactive
            //var roomToDelete = Hotel.GetInstance().Rooms.Find(r => r.Id == roomId);
            //if (roomToDelete != null)
            //{
                //roomToDelete.IsActive = false; // Assuming IsActive is a property in your Room class
                roomRepository.Delete(roomId);
            //}
        }
    }
}
