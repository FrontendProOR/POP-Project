using HotelReservations.Model;
using HotelReservations.Repository;

namespace HotelReservations.Service
{
    public class RoomTypeService
    {

        IRoomTypeRepository roomTypeRepository;

        public RoomTypeService()
        {
            roomTypeRepository = new RoomTypeRepository();
        }

        public List<RoomType> GetAllRoomTypes()
        {
            return roomTypeRepository.GetAll();
        }

        public List<RoomType> GetSortedRoomTypes()
        {
            var roomTypes = Hotel.GetInstance().RoomTypes;
            roomTypes.Sort((r1, r2) => r1.Name.CompareTo(r2.Name));
            return roomTypes;
        }

        public List<RoomType> GetAllRoomTypesByName(string startingWith)
        {
            var roomTypes = Hotel.GetInstance().RoomTypes;
            var filteredRoomTypes = roomTypes.FindAll((r) => r.Name.StartsWith(startingWith));
            return filteredRoomTypes;
        }

        public void SaveRoomType(RoomType roomType)
        {
            if (roomType.Id == 0)
            {
                roomType.Id = roomTypeRepository.Insert(roomType);
                Hotel.GetInstance().RoomTypes.Add(roomType);
            }
            else
            {
                roomTypeRepository.Update(roomType);
                //var index = Hotel.GetInstance().RoomTypes.FindIndex(r => r.Id == roomType.Id);
                //Hotel.GetInstance().RoomTypes[index] = roomType;
            }
        }


    }
}
