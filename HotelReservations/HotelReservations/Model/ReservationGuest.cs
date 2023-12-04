using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HotelReservations.Model
{
    public class ReservationGuest
    {
        public Reservation ReservationId { get; set; }
        public Guest GuestId { get; set; }
    }
}