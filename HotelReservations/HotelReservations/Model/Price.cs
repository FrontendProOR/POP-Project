namespace HotelReservations.Model
{
    public class Price
    {
        public int Id { get; set; }
        public RoomType RoomType { get; set; }
        public ReservationType ReservationType { get; set; }
        public double PriceValue { get; set; }
        public bool IsActive { get; set; } = true;
        public Price Clone()
        {
            var clone = new Price();
            clone.Id = Id;
            clone.RoomType = RoomType;
            clone.ReservationType = ReservationType;
            clone.PriceValue = PriceValue;
            clone.IsActive = IsActive;

            return clone;
        }
    }
}
