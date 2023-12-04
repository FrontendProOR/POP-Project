using HotelReservations.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelReservations.Windows;

namespace HotelReservations.Repository
{
    internal class UserTypeRepository : IUserTypeRepository
    {
        public List<UserType> GetAll()
        {
            var userTypes = new List<UserType>();
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                var commandText = "SELECT * FROM dbo.[user_type]";
                SqlDataAdapter adapter = new SqlDataAdapter(commandText, conn);

                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "user_type");

                foreach (DataRow row in dataSet.Tables["user_type"]!.Rows)
                {
                    var userType = new UserType()
                    {
                        Id = (int)row["user_type_id"],
                        Name = row["user_type_name"] as string,
                        IsActive = (bool)row["user_type_is_active"]
                    };

                    userTypes.Add(userType);
                }
            }

            return userTypes;
        }

        public int Insert(UserType userType)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
            INSERT INTO dbo.[user_type] (user_type_name, user_type_is_active)
            OUTPUT inserted.user_type_id
            VALUES (@user_type_name, @user_type_is_active)
        ";
                
                command.Parameters.Add(new SqlParameter("user_type_name", userType.Name));
                command.Parameters.Add(new SqlParameter("user_type_is_active", userType.IsActive));

                // Do not include 'user_type_id' parameter as it's an identity column

                return (int)command.ExecuteScalar();
            }
        }

        public void Update(UserType userType)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
                UPDATE dbo.[user_type] 
                SET user_type_name=@user_type_name, user_type_is_active=@user_type_is_active
                WHERE user_type_id=@user_type_id
            ";

                command.Parameters.Add(new SqlParameter("user_type_id", userType.Id));
                command.Parameters.Add(new SqlParameter("user_type_name", userType.Name));
                command.Parameters.Add(new SqlParameter("user_type_is_active", userType.IsActive));

                command.ExecuteNonQuery();
            }
        }
        public void Delete(int userTypeId)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
            DELETE FROM dbo.[user_type]
            WHERE user_type_id = @user_type_id
        ";

                command.Parameters.Add(new SqlParameter("user_type_id", userTypeId));

                command.ExecuteNonQuery();
            }
        }

        public void Save(List<UserType> userTypeList)
        {
            throw new NotImplementedException();
        }
    }
}
