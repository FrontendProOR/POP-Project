using HotelReservations.Model;

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
