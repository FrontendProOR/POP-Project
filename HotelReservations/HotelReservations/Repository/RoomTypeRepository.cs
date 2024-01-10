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
    public class RoomTypeRepository : IRoomTypeRepository
    {
        List<RoomType> IRoomTypeRepository.GetAll()
        {
            var roomTypes = new List<RoomType>();
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                var commandText = "SELECT * FROM dbo.[room_type]";
                SqlDataAdapter adapter = new SqlDataAdapter(commandText, conn);

                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "room_type");

                foreach (DataRow row in dataSet.Tables["room_type"]!.Rows)
                {
                    var roomType = new RoomType()
                    {
                        Id = (int)row["room_type_id"],
                        Name = (string)row["room_type_name"],
                        NumberOfBeds = (int)row["number_of_beds"],
                        IsActive = (bool)row["room_type_is_active"]
                    };

                    roomTypes.Add(roomType);
                }
            }

            return roomTypes;
        }

        int IRoomTypeRepository.Insert(RoomType roomType)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
                    INSERT INTO dbo.[room_type] (room_type_name, number_of_beds, room_type_is_active)
                    OUTPUT inserted.room_type_id
                    VALUES (@room_type_name,@number_of_beds, @room_type_is_active)
                "
                ;

                command.Parameters.Add(new SqlParameter("room_type_name", roomType.Name));
                command.Parameters.Add(new SqlParameter("number_of_beds", roomType.NumberOfBeds));
                command.Parameters.Add(new SqlParameter("room_type_is_active", roomType.IsActive));


                return (int)command.ExecuteScalar();
            }
        }

        void IRoomTypeRepository.Update(RoomType roomType)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
                    UPDATE dbo.[room_type] 
                    SET room_type_name=@room_type_name,number_of_beds = @number_of_beds, room_type_is_active=@room_type_is_active
                    WHERE room_type_id=@room_type_id
                ";

                command.Parameters.Add(new SqlParameter("room_type_id", roomType.Id));
                command.Parameters.Add(new SqlParameter("room_type_name", roomType.Name));
                command.Parameters.Add(new SqlParameter("number_of_beds", roomType.NumberOfBeds));
                command.Parameters.Add(new SqlParameter("room_type_is_active", roomType.IsActive));

                command.ExecuteNonQuery();
            }
        }

        void IRoomTypeRepository.Save(List<RoomType> roomTypeList)
        {
            throw new NotImplementedException();
        }
    }
}
