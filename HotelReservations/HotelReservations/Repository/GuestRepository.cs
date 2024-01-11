using HotelReservations.Model;
using System.Data.SqlClient;
using System.Data;

namespace HotelReservations.Repository
{
    public class GuestRepository: IGuestRepository
    {
        public List<Guest> GetAll()
        {
            var guests = new List<Guest>();
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                var commandText = "SELECT * FROM dbo.guest";
                SqlDataAdapter adapter = new SqlDataAdapter(commandText, conn);

                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "guest");

                foreach (DataRow row in dataSet.Tables["guest"]!.Rows)
                {
                    var guest = new Guest()
                    {
                        Id = (int)row["guest_id"],
                        Name = (string)row["guest_name"],
                        Surname = (string)row["guest_surname"],
                        IDNumber = (string)row["guest_id_number"],
                        IsActive = (bool)row["guest_is_active"]
                    };

                    guests.Add(guest);
                }
            }

            return guests;
        }

        public int Insert(Guest guest)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
                    INSERT INTO dbo.guest (guest_name, guest_surname, guest_id_number, guest_is_active)
                    OUTPUT inserted.guest_id
                    VALUES (@guest_name, @guest_surname, @guest_id_number, @guest_is_active)
                "
                ;

                command.Parameters.Add(new SqlParameter("guest_name", guest.Name));
                command.Parameters.Add(new SqlParameter("guest_surname", guest.Surname));
                command.Parameters.Add(new SqlParameter("guest_id_number", guest.IDNumber));
                command.Parameters.Add(new SqlParameter("guest_is_active", guest.IsActive));


                return (int)command.ExecuteScalar();
            }
        }

        public void Update(Guest guest)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
                    UPDATE dbo.guest 
                    SET guest_name=@guest_name, guest_surname=@guest_surname, guest_id_number=@guest_id_number, guest_is_active=@guest_is_active
                    WHERE guest_id=@guest_id
                ";

                command.Parameters.Add(new SqlParameter("guest_id", guest.Id));
                command.Parameters.Add(new SqlParameter("guest_name", guest.Name));
                command.Parameters.Add(new SqlParameter("guest_surname", guest.Surname));
                command.Parameters.Add(new SqlParameter("guest_id_number", guest.IDNumber));
                command.Parameters.Add(new SqlParameter("guest_is_active", guest.IsActive));

                command.ExecuteNonQuery();
            }
        }
        public Guest GetGuestById(int guestId)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var commandText = "SELECT * FROM dbo.guest WHERE guest_id = @guestId";
                using (SqlCommand command = new SqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("@guestId", guestId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Guest()
                            {
                                Id = (int)reader["guest_id"],
                                Name = reader["guest_name"] as string,
                                Surname = reader["guest_surname"] as string,
                                IDNumber = reader["guest_id_number"] as string,
                                IsActive = (bool)reader["guest_is_active"]
                            };
                        }
                    }
                }

                return null; // Return null if guest with the given ID is not found
            }
        }
        public void Save(List<Guest> guestList)
        {
            throw new NotImplementedException();
        }
    }
}
