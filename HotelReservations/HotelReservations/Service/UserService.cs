
using HotelReservations.Model;
using HotelReservations.Model.HotelReservations.Model;
using HotelReservations.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Service
{
    public class UserService
    {
        private IUserRepository userRepository;

        public UserService()
        {
            userRepository = new UserRepository();
        }

        public List<User> GetAllUsers()
        {
            return userRepository.GetAll();
        }
    }
}
