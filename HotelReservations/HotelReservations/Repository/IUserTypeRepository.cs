using HotelReservations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Repository
{
    public interface IUserTypeRepository
    {
        public List<UserType> GetAll();
        int Insert(UserType userType);
        void Update(UserType userType);
        void Save(List<UserType> UserTypeList);
    }
}
