using HotelReservations.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                command.Parameters.Add(new SqlParameter("reservation_id", reservationGuest.ReservationId));
                command.Parameters.Add(new SqlParameter("guest_id", reservationGuest.GuestId));

                command.ExecuteNonQuery();
            }
        }

        public void Save(List<ReservationGuest> reservationGuests)
        {
            throw new NotImplementedException();
        }
    }
}
