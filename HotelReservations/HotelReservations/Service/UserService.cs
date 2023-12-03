
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

        //public List<User> GetUsersByUsername(string username) //celi username
        //{
        //    var users = userRepository.GetAll();
        //    var filteredUsers = users.Where(u => u.Username.Equals(username)).ToList();
        //    return filteredUsers;
        //}
        public List<User> GetUsersByUsername(string username)
        {
            var users = userRepository.GetAll();
            var filteredUsers = users.Where(u => u.Username.StartsWith(username)).ToList();
            return filteredUsers;
        }
        public void DeleteUser(int userId)
        {
            userRepository.DeleteById(userId);
        }
    }
}
