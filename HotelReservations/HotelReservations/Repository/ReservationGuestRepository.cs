using HotelReservations.Model;
using System.Data.SqlClient;
using System.Data;

namespace HotelReservations.Repository
{
    public class ReservationGuestRepository : IReservationGuestRepository
    {
        public List<ReservationGuest> GetAll()
        {
            var reservationGuests = new List<ReservationGuest>();
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                var commandText = "SELECT * FROM dbo.[reservation_guest] rg\r\nINNER JOIN dbo.reservation r ON rg.reservation_id = r.reservation_id\r\nINNER JOIN dbo.guest g ON rg.guest_id = g.guest_id";
                SqlDataAdapter adapter = new SqlDataAdapter(commandText, conn);

                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "reservation_guest");

                foreach (DataRow row in dataSet.Tables["reservation_guest"].Rows)
                {
                    var reservationGuest = new ReservationGuest()
                    {
                        GuestId = new Guest()
                        {
                            Id = (int)row["guest_id"],
                            Name = (string)row["guest_name"],
                            Surname = (string)row["guest_surname"],
                            IDNumber = (string)row["guest_id_number"],
                            IsActive = (bool)row["guest_is_active"]
                        },
                        ReservationId = new Reservation()
                        {
                            Id = (int)row["reservation_id"],
                            //ReservationType = (ReservationType)row["reservation_type"],
                            //StartDateTime = (DateTime)row["start_date_time"],
                            //EndDateTime = (DateTime)row["end_date_time"],
                            //TotalPrice = (double)row["total_price"],
                            //IsActive = (bool)row["reservation_is_active"],
                        }
                    };

                    reservationGuests.Add(reservationGuest);
                }
            }

            return reservationGuests;
        }

        //        public List<(int ReservationId, int GuestId)> GetAll()
        //{
        //    var reservationGuests = new List<(int ReservationId, int GuestId)>();
        //    using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
        //    {
        //        var commandText = "SELECT rg.reservation_id, g.guest_id FROM dbo.[reservation_guest] rg\r\nINNER JOIN dbo.reservation r ON rg.reservation_id = r.reservation_id\r\nINNER JOIN dbo.guest g ON rg.guest_id = g.guest_id";
        //        SqlDataAdapter adapter = new SqlDataAdapter(commandText, conn);

        //        DataSet dataSet = new DataSet();
        //        adapter.Fill(dataSet, "reservation_guest");

        //        foreach (DataRow row in dataSet.Tables["reservation_guest"].Rows)
        //        {
        //            var reservationId = (int)row["reservation_id"];
        //            var guestId = (int)row["guest_id"];

        //            reservationGuests.Add((reservationId, guestId));
        //        }
        //    }

        //    return reservationGuests;
        //}


        /*
        public int Insert(ReservationGuest reservationGuest)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
                    INSERT INTO dbo.[reservation_guest] (reservation_id, guest_id)
                    VALUES (@reservation_id, @guest_id)
                ";

                command.Parameters.Add(new SqlParameter("reservation_id", Convert.ToInt32(reservationGuest.ReservationId.Id)));
                command.Parameters.Add(new SqlParameter("guest_id", Convert.ToInt32(reservationGuest.GuestId.Id)));

                return (int)command.ExecuteNonQuery();
            }
        }
         */
        public int Insert(ReservationGuest reservationGuest)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
            INSERT INTO dbo.[reservation_guest] (reservation_id, guest_id)
            VALUES (@reservation_id, @guest_id)
        ";

                command.Parameters.Add(new SqlParameter("@reservation_id", reservationGuest.ReservationId.Id));
                command.Parameters.Add(new SqlParameter("@guest_id", reservationGuest.GuestId.Id));

                return (int)command.ExecuteNonQuery();
            }
        }


        public void Update(ReservationGuest reservationGuest)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
                    UPDATE dbo.[reservation_guest] 
                    SET guest_id=@guest_id
                    WHERE reservation_id=@reservation_id
                ";

                command.Parameters.Add(new SqlParameter("@reservation_id", reservationGuest.ReservationId.Id));
                command.Parameters.Add(new SqlParameter("@guest_id", reservationGuest.GuestId.Id));

                command.ExecuteNonQuery();
            }
        }

        public List<Guest> GetGuestsByReservationId(int reservationId)
        {
            var guests = new List<Guest>();
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var commandText = "SELECT g.* FROM dbo.[reservation_guest] rg\r\nINNER JOIN dbo.guest g ON rg.guest_id = g.guest_id\r\nWHERE rg.reservation_id = @reservationId";
                using (SqlCommand command = new SqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("@reservationId", reservationId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var guest = new Guest()
                            {
                                Id = (int)reader["guest_id"],
                                Name = reader["guest_name"] as string,
                                Surname = reader["guest_surname"] as string,
                                IDNumber = reader["guest_id_number"] as string,
                                IsActive = (bool)reader["guest_is_active"]
                            };

                            guests.Add(guest);
                        }
                    }
                }

                return guests;
            }
        }

        public ReservationGuest GetByReservationIdAndGuestId(int reservationId, int guestId)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var commandText = @"
            SELECT *
            FROM dbo.[reservation_guest]
            WHERE reservation_id = @reservationId AND guest_id = @guestId";

                using (SqlCommand command = new SqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("@reservationId", reservationId);
                    command.Parameters.AddWithValue("@guestId", guestId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Create a new ReservationGuest object with the retrieved data
                            var reservationGuest = new ReservationGuest()
                            {
                                ReservationId = (int)reader["reservation_id"],
                                GuestId = (int)reader["guest_id"]
                            };

                            return reservationGuest;
                        }
                    }
                }
            }

            return null; // If no matching entry is found
        }




        public void Save(List<ReservationGuest> reservationGuests)
        {
            throw new NotImplementedException();
        }
    }
}
