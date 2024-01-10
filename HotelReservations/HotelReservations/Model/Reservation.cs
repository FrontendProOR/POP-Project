namespace HotelReservations.Model
{
    public class Reservation
    {
        public int Id { get; set; }
        public ReservationType ReservationType { get; set; }

        //public List<Guest> Guests { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public double TotalPrice { get; set; }
        public string RoomNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public Reservation Clone()
        {
            Reservation reservation = new Reservation();
            reservation.Id = Id;
            reservation.ReservationType = ReservationType;
            reservation.StartDateTime = StartDateTime;
            reservation.EndDateTime = EndDateTime;
            reservation.TotalPrice = TotalPrice;
            reservation.RoomNumber = RoomNumber;
            reservation.IsActive = IsActive;
            return reservation;
        }
    }
}
