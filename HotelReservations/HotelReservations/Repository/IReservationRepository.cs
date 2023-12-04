using HotelReservations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Repository
{
    public interface IReservationRepository
    {
        List<Reservation> GetAll();
        int Insert(Reservation reservation);
        void Update(Reservation reservation);
        void Save(List<Reservation> reservationList);
    }
}
