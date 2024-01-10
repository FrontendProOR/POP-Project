using HotelReservations.Model;
using System.Data.SqlClient;
using System.Data;

namespace HotelReservations.Repository
{
    public class PriceRepository : IPriceRepository
    {

        public List<Price> GetAll()
        {
            var priceList = new List<Price>();
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                var commandText = "SELECT p.*, rt.* FROM dbo.[price] p \r\nINNER JOIN dbo.[room_type] rt ON p.room_type_id = rt.room_type_id";
                SqlDataAdapter adapter = new SqlDataAdapter(commandText, conn);

                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "price");

                foreach (DataRow row in dataSet.Tables["price"]!.Rows)
                {
                    var price = new Price()
                    {
                        Id = (int)row["price_id"],
                        RoomType = new RoomType()
                        {
                            Id = (int)row["room_type_id"],
                            Name = (string)row["room_type_name"],
                            IsActive = (bool)row["room_type_is_active"]
                        },
                        ReservationType = (ReservationType)row["reservation_type"],
                        PriceValue = (double)row["price_value"],
                        IsActive = (bool)row["price_is_active"],

                    };

                    priceList.Add(price);
                }
            }

            return priceList;
        }

        public int Insert(Price price)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
                    INSERT INTO dbo.[price] (room_type_id, reservation_type, price_value, price_is_active)
                    OUTPUT inserted.price_id
                    VALUES (@room_type_id, @reservation_type, @price_value, @price_is_active)
                "
                ;

                command.Parameters.Add(new SqlParameter("room_type_id", price.RoomType.Id));
                command.Parameters.Add(new SqlParameter("reservation_type", Convert.ToInt32(price.ReservationType)));
                command.Parameters.Add(new SqlParameter("price_value", price.PriceValue));
                command.Parameters.Add(new SqlParameter("price_is_active", price.IsActive));


                return (int)command.ExecuteScalar();
            }
        }

        public void Update(Price price)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"
                    UPDATE dbo.[price] 
                    SET room_type_id=@room_type_id, reservation_type=@reservation_type, price_value=@price_value, price_is_active=@price_is_active
                    WHERE price_id=@price_id
                ";

                command.Parameters.Add(new SqlParameter("price_id", price.Id));
                command.Parameters.Add(new SqlParameter("room_type_id", price.RoomType.Id));
                command.Parameters.Add(new SqlParameter("reservation_type", price.ReservationType));
                command.Parameters.Add(new SqlParameter("price_value", price.PriceValue));
                command.Parameters.Add(new SqlParameter("price_is_active", price.IsActive));

                command.ExecuteNonQuery();
            }
        }

        public void Save(List<Price> priceList)
        {
            throw new NotImplementedException();
        }
    }
}
