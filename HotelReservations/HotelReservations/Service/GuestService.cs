using HotelReservations.Model;
using HotelReservations.Repository;

namespace HotelReservations.Service
{
    class GuestService
    {
        IGuestRepository guestRepository;
        IReservationGuestRepository reservationGuestRepository;
        public GuestService()
        {
            guestRepository = new GuestRepository();
            reservationGuestRepository = new ReservationGuestRepository();
        }

        public List<Guest> GetAllGuests()
        {
            return guestRepository.GetAll();
        }

        public List<Guest> GetSortedGuests()
        {
            var guests = Hotel.GetInstance().Guests;
            guests.Sort((r1, r2) => r1.IDNumber.CompareTo(r2.IDNumber));
            return guests;
        }

        public List<Guest> GetGuestByReservation(Reservation reservation)
        {
            List<Guest> guests = reservationGuestRepository.GetGuestsByReservationId(reservation.Id);
            return guests;
            //return guestRepository.GetGuestById(reservation.GuestId);
        }

        public List<Guest> GetAllGuestsByIDNumber(string startingWith)
        {
            var guests = Hotel.GetInstance().Guests;
            var filteredGuests = guests.FindAll((r) => r.IDNumber.StartsWith(startingWith));
            return filteredGuests;
        }

        public void SaveGuest(Guest guest)
        {
            if (guest.Id == 0)
            {
                guestRepository.Insert(guest);
                //guest.Id = guestRepository.Insert(guest);
                //Hotel.GetInstance().Guests.Add(guest);
            }
            else
            {
                guestRepository.Update(guest);
                //var index = Hotel.GetInstance().Guests.FindIndex(r => r.Id == guest.Id);
                //Hotel.GetInstance().Guests[index] = guest;
            }
        }
    }
}
