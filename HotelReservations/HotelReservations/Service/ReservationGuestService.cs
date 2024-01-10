using HotelReservations.Model;
using HotelReservations.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Service
{
    public class ReservationGuestService
    {
        ReservationGuestRepository reservationGuestRepository;
        public ReservationGuestService()
        {
            reservationGuestRepository = new ReservationGuestRepository();
        }

        public List<ReservationGuest> GetAllReservationGuests()
        {
            //return Hotel.GetInstance().ReservationGuests;
            return reservationGuestRepository.GetAll();
        }

        public void SaveReservationGuest(ReservationGuest reservationGuest)
        {

            reservationGuestRepository.Insert(reservationGuest);
            //Hotel.GetInstance().ReservationGuests.Add(reservationGuest);

        }
    }
}
