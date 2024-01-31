using HotelReservations.Model;
using System.Data.SqlClient;
using System.Data;

namespace HotelReservations.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        public List<Reservation> GetAll()
        {
            var reservations = new List<Reservation>();
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                var commandText = "SELECT * FROM dbo.[reservation]";
                SqlDataAdapter adapter = new SqlDataAdapter(commandText, conn);

                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "reservation");

                foreach (DataRow row in dataSet.Tables["reservation"]!.Rows)
                {
                    var reservation = new Reservation()
                    {
                        Id = (int)row["reservation_id"],
                        ReservationType = (ReservationType)row["reservation_type"],
                        StartDateTime = (DateTime)row["start_date_time"],
                        EndDateTime = (DateTime)row["end_date_time"],
                        TotalPrice = (double)row["total_price"],
                        RoomNumber = row["room_number"] as string,
                        IsActive = (bool)row["reservation_is_active"],
                    };

                    reservations.Add(reservation);
                }
            }

            return reservations;
        }

        public int Insert(Reservation reservation)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
            INSERT INTO dbo.[reservation] (reservation_type, start_date_time, end_date_time, total_price, room_number, reservation_is_active)
            OUTPUT inserted.reservation_id
            VALUES (@reservation_type, @start_date_time, @end_date_time, @total_price, @room_number, @reservation_is_active)
        ";

                command.Parameters.Add(new SqlParameter("reservation_type", Convert.ToInt32(reservation.ReservationType)));
                command.Parameters.Add(new SqlParameter("start_date_time", reservation.StartDateTime));
                command.Parameters.Add(new SqlParameter("end_date_time", reservation.EndDateTime));
                command.Parameters.Add(new SqlParameter("total_price", reservation.TotalPrice));
                command.Parameters.Add(new SqlParameter("room_number", reservation.RoomNumber)); // Include room_number parameter
                command.Parameters.Add(new SqlParameter("reservation_is_active", reservation.IsActive));

                return (int)command.ExecuteScalar();
            }
        }

        public List<Reservation> GetCurrentAndFutureReservations()
        {
            var reservations = new List<Reservation>();
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                // Retrieve reservations that are current or in the future
                string commandText = @"
                    SELECT *
                    FROM dbo.[reservation]
                    WHERE end_date_time >= GETDATE()";

                SqlDataAdapter adapter = new SqlDataAdapter(commandText, conn);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "reservation");

                foreach (DataRow row in dataSet.Tables["reservation"].Rows)
                {
                    var reservation = new Reservation()
                    {
                        Id = (int)row["reservation_id"],
                        ReservationType = (ReservationType)row["reservation_type"],
                        StartDateTime = (DateTime)row["start_date_time"],
                        EndDateTime = (DateTime)row["end_date_time"],
                        TotalPrice = (double)row["total_price"],
                        RoomNumber = row["room_number"] as string,
                        IsActive = (bool)row["reservation_is_active"],
                    };

                    // Update reservation status if end date has passed
                    if (reservation.EndDateTime < DateTime.Now)
                    {
                        UpdateReservationStatus(conn, reservation.Id, false);
                        reservation.IsActive = false;
                    }

                    reservations.Add(reservation);
                }
            }

            return reservations;
        }

        private void UpdateReservationStatus(SqlConnection conn, int reservationId, bool isActive)
        {
            // Update reservation_is_active column in the reservation table
            string updateCommandText = @"
                UPDATE dbo.reservation
                SET reservation_is_active = @IsActive
                WHERE reservation_id = @ReservationId";

            using (SqlCommand command = new SqlCommand(updateCommandText, conn))
            {
                command.Parameters.AddWithValue("@IsActive", isActive);
                command.Parameters.AddWithValue("@ReservationId", reservationId);
                command.ExecuteNonQuery();
            }
        }

        public void Update(Reservation reservation)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
                    UPDATE dbo.[reservation] 
                    SET reservation_type=@reservation_type, start_date_time=@start_date_time, end_date_time=@end_date_time, total_price=@total_price,room_number=@room_number, reservation_is_active=@reservation_is_active 
                    WHERE reservation_id=@reservation_id
                "
                ;

                command.Parameters.Add(new SqlParameter("reservation_id", reservation.Id));
                command.Parameters.Add(new SqlParameter("reservation_type", Convert.ToInt32(reservation.ReservationType)));
                command.Parameters.Add(new SqlParameter("start_date_time", reservation.StartDateTime));
                command.Parameters.Add(new SqlParameter("end_date_time", reservation.EndDateTime));
                command.Parameters.Add(new SqlParameter("total_price", reservation.TotalPrice));
                command.Parameters.Add(new SqlParameter("room_number", reservation.RoomNumber));
                command.Parameters.Add(new SqlParameter("reservation_is_active", reservation.IsActive));

                command.ExecuteNonQuery();
            }
        }
        public Room GetRoomByNumber(string roomNumber)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = "SELECT * FROM dbo.[room] WHERE room_number = @room_number";
                command.Parameters.Add(new SqlParameter("room_number", roomNumber));

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var room = new Room()
                    {
                        Id = (int)reader["room_id"],
                        RoomNumber = (string)reader["room_number"],
                        // Populate other room properties as needed
                        // ...
                    };

                    return room;
                }

                return null;
            }
        }
        public Reservation GetById(int reservationId)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = "SELECT * FROM dbo.[reservation] WHERE reservation_id = @reservation_id";
                command.Parameters.Add(new SqlParameter("@reservation_id", reservationId));

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var reservation = new Reservation()
                        {
                            Id = (int)reader["reservation_id"],
                            ReservationType = (ReservationType)reader["reservation_type"],
                            StartDateTime = (DateTime)reader["start_date_time"],
                            EndDateTime = (DateTime)reader["end_date_time"],
                            TotalPrice = (double)reader["total_price"],
                            RoomNumber = reader["room_number"] as string,
                            IsActive = (bool)reader["reservation_is_active"]
                        };

                        return reservation;
                    }
                }
            }

            return null; // If no matching reservation is found
        }

        public void Save(List<Reservation> reservationList)
        {
            throw new NotImplementedException();
        }
    }
}
