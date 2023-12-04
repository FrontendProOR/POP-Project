using HotelReservations.Model;
using HotelReservations.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Service
{
    public class ReservationService
    {
        IReservationRepository reservationRepository;
        ReservationGuestRepository reservationGuestRepository;
        public ReservationService()
        {
            reservationRepository = new ReservationRepository();
            reservationGuestRepository = new ReservationGuestRepository();

        }

        public List<Reservation> GetAllReservations()
        {
            return reservationRepository.GetAll();
        }

        public List<ReservationGuest> GetAllRGuests()
        {
            return reservationGuestRepository.GetAll();
        }

        public List<Reservation> GetSortedReservations()
        {
            var reservations = Hotel.GetInstance().Reservations;
            reservations.Sort((r1, r2) => r1.TotalPrice.CompareTo(r2.TotalPrice));
            return reservations;
        }

        public void SaveReservation(Reservation reservation)
        {
            if (reservation.Id == 0)
            {
                reservationRepository.Insert(reservation);
                //reservation.Id = reservationRepository.Insert(reservation);
                //Hotel.GetInstance().Reservations.Add(reservation);
            }
            else
            {
                reservationRepository.Update(reservation);
                //var index = Hotel.GetInstance().Reservations.FindIndex(r => r.Id == reservation.Id);
                //Hotel.GetInstance().Reservations[index] = reservation;
            }
        }

    }
}
