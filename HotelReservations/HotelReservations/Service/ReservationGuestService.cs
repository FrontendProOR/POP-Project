using HotelReservations.Model;
using HotelReservations.Repository;
using System.Data.SqlClient;

namespace HotelReservations.Service
{
    public class ReservationGuestService
    {
        ReservationGuestRepository reservationGuestRepository;
        ReservationRepository reservationRepository;
        public ReservationGuestService()
        {
            reservationGuestRepository = new ReservationGuestRepository();
            reservationRepository = new ReservationRepository();
        }

        public List<ReservationGuest> GetAllReservationGuests()
        {
            //return Hotel.GetInstance().ReservationGuests;
            return reservationGuestRepository.GetAll();
        }

        //public void SaveReservationGuest(ReservationGuest reservationGuest)
        //{

        //    //reservationGuestRepository.Insert(reservationGuest);
        //    //Hotel.GetInstance().ReservationGuests.Add(reservationGuest);
        //    try
        //    {
        //        reservationGuestRepository.Insert(reservationGuest);
        //    }
        //    catch (SqlException ex)
        //    {
        //        // Log the exception or handle it as appropriate for your application
        //        //Console.WriteLine($"SQL Exception: {ex.Message}");
        //        //Log.Error($"SQL Exception: {ex.Message}");
        //        throw ex;
        //        // Optionally, you can choose not to rethrow the exception or take other actions as needed
        //    }
        //}
        //public void SaveReservationGuest(ReservationGuest reservationGuest)
        //{
        //    try
        //    {
        //        var existingReservationGuest = reservationGuestRepository.GetByReservationIdAndGuestId(reservationGuest.ReservationId.Id,reservationGuest.GuestId.Id);

        //        if (existingReservationGuest != null)
        //        {
        //            if (existingReservationGuest.GuestId.Id != reservationGuest.GuestId.Id)
        //            {
        //                reservationGuestRepository.Update(reservationGuest);
        //            }
        //        }
        //        else
        //        {
        //            reservationGuestRepository.Insert(reservationGuest);
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new ApplicationException($"Message:{ex.Message}An error occurred while saving the reservation guest. Please try again later.");
        //    }
        //}
        //public void SaveReservationGuest(ReservationGuest reservationGuest)
        //{
        //    var existingReservationGuest = reservationGuestRepository.GetByReservationIdAndGuestId(reservationGuest.ReservationId.Id, reservationGuest.GuestId.Id);

        //    if (existingReservationGuest != null)
        //    {
        //        if (existingReservationGuest.GuestId.Id != reservationGuest.GuestId.Id)
        //        {
        //            reservationGuestRepository.Update(reservationGuest);
        //        }
        //    }
        //    else
        //    {
        //        reservationGuestRepository.Insert(reservationGuest);
        //    }
        //}
        //public void SaveReservationGuest(ReservationGuest reservationGuest)
        //{
        //    // Check if the reservation guest entry already exists
        //    var existingReservationGuest = reservationGuestRepository.GetByReservationIdAndGuestId(reservationGuest.ReservationId.Id, reservationGuest.GuestId.Id);

        //    if (existingReservationGuest != null)
        //    {
        //        reservationGuestRepository.Update(reservationGuest);
        //    }
        //    else
        //    {
        //        reservationGuestRepository.Insert(reservationGuest);
        //    }
        //}
        //public void SaveReservationGuest(ReservationGuest reservationGuest)
        //{
        //    try
        //    {
        //        // Check if the reservation guest entry already exists in the Hotel instance
        //        if (!Hotel.GetInstance().ReservationGuests.Any(rg => rg.ReservationId.Id == reservationGuest.ReservationId.Id && rg.GuestId.Id == reservationGuest.GuestId.Id))
        //        {
        //            reservationGuestRepository.Insert(reservationGuest);
        //            Hotel.GetInstance().ReservationGuests.Add(reservationGuest);
        //        }
        //        else
        //        {
        //            reservationGuestRepository.Update(reservationGuest);
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        // Handle SQL exception
        //        throw new ApplicationException($"Message:{ex.Message}An error occurred while saving the reservation guest. Please try again later.");
        //    }
        //}
        public void SaveReservationGuest(ReservationGuest reservationGuest)
        {
            //try
            //{
                if (reservationGuest.ReservationId == null)
                {
                    throw new ArgumentException("The specified reservation does not exist.");
                }

                // Check if the reservation guest entry already exists in the Hotel instance
                if (!Hotel.GetInstance().ReservationGuests.Any(rg => rg.ReservationId.Id == reservationGuest.ReservationId.Id && rg.GuestId.Id == reservationGuest.GuestId.Id))
                {
                    reservationGuestRepository.Insert(reservationGuest);
                    Hotel.GetInstance().ReservationGuests.Add(reservationGuest);
                }
                else
                {
                    reservationGuestRepository.Update(reservationGuest);
                }
            //}
            //catch (SqlException ex)
            //{
            //    // Handle SQL exception
            //    throw new ApplicationException($"Message:{ex.Message} An error occurred while saving the reservation guest. Please try again later.");
            //}
            //catch (ArgumentException ex)
            //{
            //    // Handle invalid reservation ID
            //    throw new ApplicationException($"Message: {ex.Message} Please provide a valid reservation ID.");
            //}
        }


        public List<Guest> GetGuestsByReservationId(int reservationId)
        {
            return reservationGuestRepository.GetGuestsByReservationId(reservationId);
        }
    }
}
