using HotelReservations.Model;

namespace HotelReservations.Repository
{
    internal interface IUserRepository
    {
        List<User> GetAll() => throw new System.NotImplementedException();
        int Insert(User user) => throw new System.NotImplementedException();
        void Update(User user) => throw new System.NotImplementedException();
        void Save(List<User> userList) => throw new System.NotImplementedException();
        void DeleteById(int userId);
        void PermanentDeleteUser(int userId);
        User GetUserById(int userId);
        bool UserExists(int userId);
    }
}
