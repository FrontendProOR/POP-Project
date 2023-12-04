
using HotelReservations.Model;
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

        public void PermanentDeleteUser(int userId)
        {
            // Create an instance of UserRepository
            UserRepository userRepositoryInstance = new UserRepository();

            // Call the instance method on the instance
            userRepositoryInstance.PermanentDeleteUser(userId);
        }

        public void SaveUser(User user)
        {
            if (user.Id == 0)
            {
                user.Id = userRepository.Insert(user);
                Hotel.GetInstance().Users.Add(user);
            }
            else
            {
                userRepository.Update(user);
                var index = Hotel.GetInstance().Users.FindIndex(u => u.Id == user.Id);

                if (index != -1)
                {
                    Hotel.GetInstance().Users[index] = user;
                }
                else
                {
                    Console.WriteLine($"User with ID {user.Id} not found in the collection.");
                }
            }
        }
    }
}
