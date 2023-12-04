using HotelReservations.Model;
using HotelReservations.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelReservations.Service
{
    internal class UserTypeService
    {
        UserTypeRepository userTypeRepository;
        public UserTypeService() {
            userTypeRepository = new UserTypeRepository();
        }

        public List<UserType> GetAllUserTypes()
        {
            return userTypeRepository.GetAll();
        }

        public List<UserType> GetSortedUserTypes()
        {
            var userTypes = userTypeRepository.GetAll();
            userTypes.Sort((r1, r2) => r1.Name.CompareTo(r2.Name));
            return userTypes;
        }

        public void SaveUserType(UserType userType)
        {
            // Check if the user type name is null or empty
            if (string.IsNullOrWhiteSpace(userType.Name))
            {
                MessageBox.Show("User type name cannot be empty.");
                return;
            }

            // Check if the user type ID is 0 or null
            if (userType.Id == 0)
            {
                // Before inserting, ensure that userType.Name is not null
                if (userType.Name != null)
                {
                    userType.Id = userTypeRepository.Insert(userType);
                    Hotel.GetInstance().UserTypes.Add(userType);
                }
                else
                {
                    MessageBox.Show("User type name cannot be null.");
                }
            }
            else
            {
                userTypeRepository.Update(userType);
                var index = Hotel.GetInstance().UserTypes.FindIndex(r => r.Id == userType.Id);
                //Hotel.GetInstance().UserTypes[index] = userType;
            }
        }



    }

}
