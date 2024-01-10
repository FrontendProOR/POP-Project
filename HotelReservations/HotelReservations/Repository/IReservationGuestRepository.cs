using HotelReservations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Repository
{
    public interface IReservationGuestRepository
    {
        List<ReservationGuest> GetAll();
        //List<(int ReservationId, int GuestId)> GetAll();
        int Insert(ReservationGuest reservationGuest);
        void Update(ReservationGuest reservationGuest);
        void Save(List<ReservationGuest> reservationGuests);
    }
}
