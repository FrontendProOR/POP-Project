using HotelReservations.Model;
using HotelReservations.Model.HotelReservations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Repository
{
    internal interface IUserRepository
    {
        List<User> GetAll() => throw new System.NotImplementedException();
        int Insert(User user) => throw new System.NotImplementedException();
        void Update(User user) => throw new System.NotImplementedException();
        void Save(List<User> userList) => throw new System.NotImplementedException();
        void DeleteById(int userId);
        User GetUserById(int userId);
    }
}
