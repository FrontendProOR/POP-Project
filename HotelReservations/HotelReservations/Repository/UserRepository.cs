using HotelReservations.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Repository
{
    internal class UserRepository : IUserRepository
    {
        public List<User> GetAll()
        {
            var users = new List<User>();
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();
                var commandText = @"SELECT u.*, ut.* FROM dbo.[user] u INNER JOIN dbo.user_type ut ON u.user_type = ut.user_type_name";
                SqlDataAdapter adapter = new SqlDataAdapter(commandText, conn);

                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "user");

                foreach (DataRow row in dataSet.Tables["user"].Rows)
                {
                    var user = new User()
                    {
                        Id = (int)row["user_id"],
                        Name = (string)row["first_name"],
                        Surname = (string)row["last_name"],
                        JMBG = (string)row["JMBG"],
                        Username = (string)row["username"],
                        Password = (string)row["password"],
                        UserType = new UserType()
                        {
                            Id = (int)row["user_type_id"],
                            Name = (string)row["user_type_name"],
                            IsActive = (bool)row["user_type_is_active"]
                        },
                        IsDeleted = (bool)row["is_deleted"]

                    };

                    users.Add(user);
                }
            }

            return users;
        }

        public int Insert(User user)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
                    INSERT INTO dbo.[user] (first_name, last_name, JMBG, username, password, user_type)
                    OUTPUT inserted.user_id
                    VALUES (@first_name, @last_name, @JMBG, @username, @password, @user_type)
                ";

                command.Parameters.Add(new SqlParameter("@first_name", user.Name));
                command.Parameters.Add(new SqlParameter("@last_name", user.Surname));
                command.Parameters.Add(new SqlParameter("@JMBG", user.JMBG));
                command.Parameters.Add(new SqlParameter("@username", user.Username));
                command.Parameters.Add(new SqlParameter("@password", user.Password));
                command.Parameters.Add(new SqlParameter("@user_type", user.UserType.Name));

                return (int)command.ExecuteScalar();
            }
        }

        public void Update(User user)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();
                //if (IsUsernameTaken(conn, user.Username, user.Id))
                //{
                //    // Handle the case where the new username is already taken
                //    // You can throw an exception, log an error, or take other appropriate actions.
                //    throw new InvalidOperationException("New username is already taken.");
                //}
                var command = conn.CreateCommand();
                command.CommandText = @"
                    UPDATE dbo.[user] 
                    SET first_name = @first_name, last_name = @last_name, JMBG = @JMBG, username = @username, password = @password, user_type = @user_type
                    WHERE user_id = @user_id
                ";

                command.Parameters.Add(new SqlParameter("@user_id", user.Id));
                command.Parameters.Add(new SqlParameter("@first_name", user.Name));
                command.Parameters.Add(new SqlParameter("@last_name", user.Surname));
                command.Parameters.Add(new SqlParameter("@JMBG", user.JMBG));
                command.Parameters.Add(new SqlParameter("@username", user.Username));
                command.Parameters.Add(new SqlParameter("@password", user.Password));
                command.Parameters.Add(new SqlParameter("@user_type", user.UserType.Name));

                command.ExecuteNonQuery();
            }
        }



        //private bool IsUsernameTaken(SqlConnection conn, string username, int userIdToExclude)
        //{
        //    var command = conn.CreateCommand();
        //    command.CommandText = "SELECT COUNT(*) FROM dbo.[user] WHERE username = @username AND user_id <> @user_id";
        //    command.Parameters.Add(new SqlParameter("@username", username));
        //    command.Parameters.Add(new SqlParameter("@user_id", userIdToExclude));

        //    int count = (int)command.ExecuteScalar();
        //    return count > 0;
        //}

        public void Save(List<User> userList)
        {
            foreach (var user in userList)
            {
                if (user.Id == 0)
                {
                    // If the user ID is 0, it means it's a new user, so insert it
                    Insert(user);
                }
                else
                {
                    // If the user ID is not 0, it means it's an existing user, so update it
                    Update(user);
                }
            }
        }

        //normal delete
        public void PermanentDeleteUser(int userId)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
                DELETE FROM dbo.[user] WHERE user_id = @user_id
            ";

                command.Parameters.Add(new SqlParameter("@user_id", userId));

                command.ExecuteNonQuery();
            }
        }

        //Logical delete
        public void DeleteById(int userId)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"UPDATE dbo.[user] SET is_deleted = 1 WHERE user_id = @user_id";

                command.Parameters.Add(new SqlParameter("@user_id", userId));

                command.ExecuteNonQuery();
            }
        }


        public User GetUserById(int userId)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var commandText = @"
            SELECT u.*, ut.* FROM dbo.[user] u
            INNER JOIN dbo.user_type ut ON u.user_type = ut.user_type_name
            WHERE u.user_id = @user_id
        ";

                var command = new SqlCommand(commandText, conn);
                command.Parameters.Add(new SqlParameter("@user_id", userId));

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = (int)reader["user_id"],
                            Name = (string)reader["first_name"],
                            Surname = (string)reader["last_name"],
                            JMBG = (string)reader["JMBG"],
                            Username = (string)reader["username"],
                            Password = (string)reader["password"],
                            UserType = new UserType
                            {
                                Id = (int)reader["user_type_id"],
                                Name = (string)reader["user_type_name"],
                                IsActive = (bool)reader["user_type_is_active"]
                            }
                        };
                    }
                }

                // Return null if user with the specified ID is not found
                return null;
            }
        }
        public bool UserExists(int userId)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var commandText = @"
            SELECT 1
            FROM dbo.[user]
            WHERE user_id = @user_id
        ";

                var command = new SqlCommand(commandText, conn);
                command.Parameters.Add(new SqlParameter("@user_id", userId));

                var result = command.ExecuteScalar();

                return result != null && (int)result == 1;
            }
        }

    }
}
