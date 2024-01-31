using HotelReservations.Model;
using System.Data;
using System.Data.SqlClient;

namespace HotelReservations.Repository
{
    public class RoomRepository : IRoomRepository
    {
        public List<Room> GetAll()
        {
            var rooms = new List<Room>();
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                var commandText = "SELECT r.*, rt.* FROM dbo.room r\r\nINNER JOIN dbo.room_type rt ON r.room_type_id = rt.room_type_id";
                SqlDataAdapter adapter = new SqlDataAdapter(commandText, conn);

                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "room");

                foreach (DataRow row in dataSet.Tables["room"]!.Rows)
                {
                    var room = new Room()
                    {
                        Id = (int)row["room_id"],
                        RoomNumber = row["room_number"] as string,
                        
                        HasTV = (bool)row["has_TV"],
                        HasMiniBar = (bool)row["has_mini_bar"],
                        IsActive = (bool)row["room_is_active"],
                        IsDeleted = (bool)row["is_deleted"],
                        RoomType = new RoomType()
                        {
                            Id = (int)row["room_type_id"],
                            Name = (string)row["room_type_name"],
                            NumberOfBeds = (int)row["number_of_beds"],
                            IsActive = (bool)row["room_type_is_active"]
                        }
                    };

                    rooms.Add(room);
                }
            }

            return rooms;
        }

        public List<Room> GetAllCurrentlyActive()
        {
            var rooms = new List<Room>();
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var commandText = @"
            SELECT r.*, rt.*, 
            CASE 
                WHEN EXISTS (
                    SELECT 1 
                    FROM dbo.reservation res 
                    WHERE res.room_number = r.room_number 
                    AND res.start_date_time <= GETDATE() 
                    AND res.end_date_time >= GETDATE() 
                    AND res.reservation_is_active = 1
                ) THEN CAST(1 AS bit) 
                ELSE CAST(0 AS bit) 
            END AS is_room_active 
            FROM dbo.room r
            INNER JOIN dbo.room_type rt ON r.room_type_id = rt.room_type_id";

                SqlDataAdapter adapter = new SqlDataAdapter(commandText, conn);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "room");

                foreach (DataRow row in dataSet.Tables["room"].Rows)
                {
                    var room = new Room()
                    {
                        Id = (int)row["room_id"],
                        RoomNumber = row["room_number"] as string,
                        HasTV = (bool)row["has_TV"],
                        HasMiniBar = (bool)row["has_mini_bar"],
                        IsActive = (bool)row["is_room_active"], // Set IsActive based on reservation status
                        IsDeleted = (bool)row["is_deleted"],
                        RoomType = new RoomType()
                        {
                            Id = (int)row["room_type_id"],
                            Name = (string)row["room_type_name"],
                            NumberOfBeds = (int)row["number_of_beds"],
                            IsActive = (bool)row["room_type_is_active"]
                        }
                    };

                    rooms.Add(room);
                }

                // Update room_is_active column in the room table based on current reservations
                foreach (var room in rooms)
                {
                    // Assuming there is a method to update the database with the new isActive value
                    UpdateRoomIsActive(conn, room.RoomNumber, room.IsActive);
                }
            }

            return rooms;
        }

        private void UpdateRoomIsActive(SqlConnection conn, string roomNumber, bool isActive)
        {
            // Prepare the SQL update command
            string updateCommandText = @"
        UPDATE dbo.room
        SET room_is_active = @IsActive
        WHERE room_number = @RoomNumber";

            using (SqlCommand command = new SqlCommand(updateCommandText, conn))
            {
                // Add parameters
                command.Parameters.AddWithValue("@IsActive", isActive);
                command.Parameters.AddWithValue("@RoomNumber", roomNumber);

                // Execute the update command
                command.ExecuteNonQuery();
            }
        }


        public int Insert(Room room)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
                    INSERT INTO dbo.[room] (room_number, has_TV, has_mini_bar, room_is_active, room_type_id)
                    OUTPUT inserted.room_id
                    VALUES (@room_number, @has_TV, @has_mini_bar, @room_is_active, @room_type_id)
                ";

                command.Parameters.Add(new SqlParameter("room_number", room.RoomNumber));
                command.Parameters.Add(new SqlParameter("has_TV", room.HasTV));
                command.Parameters.Add(new SqlParameter("has_mini_bar", room.HasMiniBar));
                command.Parameters.Add(new SqlParameter("room_is_active", room.IsActive));
                command.Parameters.Add(new SqlParameter("room_type_id", room.RoomType.Id));
                command.Parameters.Add(new SqlParameter("is_deleted",false));

                return (int)command.ExecuteScalar();
            }
        }

        public void Update(Room room)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
                    UPDATE dbo.[room] 
                    SET room_number=@room_number, has_TV=@has_TV, has_mini_bar=@has_mini_bar, room_is_active=@room_is_active, room_type_id=@room_type_id
                    WHERE room_id=@room_id
                ";

                command.Parameters.Add(new SqlParameter("room_id", room.Id));
                command.Parameters.Add(new SqlParameter("room_number", room.RoomNumber));
                command.Parameters.Add(new SqlParameter("has_TV", room.HasTV));
                command.Parameters.Add(new SqlParameter("has_mini_bar", room.HasMiniBar));
                command.Parameters.Add(new SqlParameter("room_is_active", room.IsActive));
                command.Parameters.Add(new SqlParameter("room_type_id", room.RoomType.Id));
                command.Parameters.Add(new SqlParameter("is_deleted", false));

                command.ExecuteNonQuery();
            }
        }

        public void Save(List<Room> roomList)
        {
            throw new NotImplementedException();
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
            UPDATE dbo.[room] 
            SET is_deleted = 1
            WHERE room_id = @room_id
        ";

                command.Parameters.Add(new SqlParameter("room_id", id));

                command.ExecuteNonQuery();
            }
        }

        public void PermanentDeleteRoom(int id)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
            DELETE FROM dbo.[room] 
            WHERE room_id = @room_id
        ";

                command.Parameters.Add(new SqlParameter("room_id", id));

                command.ExecuteNonQuery();
            }
        }
        public List<RoomType> GetAllRoomTypes()
        {
            var roomTypes = new List<RoomType>();

            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var commandText = "SELECT * FROM dbo.room_type";
                SqlCommand command = new SqlCommand(commandText, conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if ((bool)reader["room_type_is_active"])
                        {

                        var roomType = new RoomType()
                        {
                            Id = (int)reader["room_type_id"],
                            Name = reader["room_type_name"] as string,
                            IsActive = (bool)reader["room_type_is_active"] 
                        };

                        roomTypes.Add(roomType);
                        }
                    }
                }
            }

            return roomTypes;
        }

        public Room GetRoomByRoomNumber(string roomNumber)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var commandText = "SELECT r.*, rt.* FROM dbo.room r\r\nINNER JOIN dbo.room_type rt ON r.room_type_id = rt.room_type_id " +
                                  "WHERE r.room_number = @roomNumber";

                using (SqlCommand command = new SqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("@roomNumber", roomNumber);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Room()
                            {
                                Id = (int)reader["room_id"],
                                RoomNumber = reader["room_number"] as string,
                                HasTV = (bool)reader["has_TV"],
                                HasMiniBar = (bool)reader["has_mini_bar"],
                                IsActive = (bool)reader["room_is_active"],
                                IsDeleted = (bool)reader["is_deleted"],
                                RoomType = new RoomType()
                                {
                                    Id = (int)reader["room_type_id"],
                                    Name = reader["room_type_name"] as string,
                                    NumberOfBeds = (int)reader["number_of_beds"],
                                    IsActive = (bool)reader["room_type_is_active"]
                                }
                            };
                        }
                    }
                }

                return null; // Return null if room with the given room number is not found
            }
        }
    }
}
