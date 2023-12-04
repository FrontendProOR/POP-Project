using HotelReservations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Repository
{
    public interface IRoomTypeRepository
    {
        List<RoomType> GetAll();
        int Insert(RoomType roomType);
        void Update(RoomType roomType);
        void Save(List<RoomType> roomTypeList);
    }
}

