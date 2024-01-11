using HotelReservations.Model;
using HotelReservations.Service;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace HotelReservations.Windows
{
    /// <summary>
    /// Interaction logic for Reservations.xaml
    /// </summary>
    public partial class Reservations : Window
    {
        private ICollectionView view;
        private ReservationService reservationService;
        private RoomService roomService;
        private GuestService guestService;
        public Reservations()
        {
            InitializeComponent();
            reservationService = new ReservationService();
            roomService = new RoomService();
            guestService = new GuestService();
            FillData();

        }

        public void FillData()
        {
            var reservationService = new ReservationService();
            var reservations = reservationService.GetAllReservations();

            view = CollectionViewSource.GetDefaultView(reservations);
            view.Filter = DoFilter;


            ReservationsDG.ItemsSource = null;
            ReservationsDG.ItemsSource = view;
            ReservationsDG.IsSynchronizedWithCurrentItem = true;
        }

        //private bool DoFilter(object reservationObject)
        //{
        //    var reservation = reservationObject as Reservation;

        //    if (reservation != null && reservation.IsActive)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            view.Filter = DoFilter;
            view.Refresh();
        }

        private bool DoFilter(object reservationObject)
        {
            var reservation = reservationObject as Reservation;

             //Filter by room number
            if (!string.IsNullOrWhiteSpace(RoomNumberTextBox.Text) &&
                reservation != null &&
                reservation.RoomNumber.ToString() != RoomNumberTextBox.Text)
            {
                return false;
            }
            if (reservation != null && !reservation.IsActive)
            {
                return false;
            }

            // Check if the reservation is active on the selected day
            if (SelectedDateIsWithinReservationPeriod(reservation))
            {
                return true;
            }

            // If end date has passed, update IsActive to false and return false
            if (reservation != null && reservation.EndDateTime.Date < DateTime.Now.Date)
            {
                reservation.IsActive = false;
                // Assuming you have a method to update the reservation in your repository, like reservationRepository.Update(reservation);
                //reservationRepository.Update(reservation);
                reservationService.SaveReservation(reservation);
                return false;
            }

            // Filter by arrival date
            if (ArrivalDatePicker.SelectedDate.HasValue &&
                reservation != null &&
                reservation.StartDateTime.Date < ArrivalDatePicker.SelectedDate.Value.Date)
            {
                return false;
            }

            // Filter by departure date
            if (DepartureDatePicker.SelectedDate.HasValue &&
                reservation != null &&
                reservation.EndDateTime.Date > DepartureDatePicker.SelectedDate.Value.Date)
            {
                return false;
            }

            return true;
        }

        private bool SelectedDateIsWithinReservationPeriod(Reservation reservation)
        {
            // Check if the selected date is within the reservation period
            if (ArrivalDatePicker.SelectedDate.HasValue)
            {
                var selectedDate = ArrivalDatePicker.SelectedDate.Value.Date;
                return reservation != null &&
                       reservation.IsActive &&
                       selectedDate >= reservation.StartDateTime.Date &&
                       selectedDate <= reservation.EndDateTime.Date;
            }

            return false;
        }

        private void ShowAllBtn_Click(object sender, RoutedEventArgs e)
        {
            RoomNumberTextBox.Text = "";
            ArrivalDatePicker.SelectedDate = null;
            DepartureDatePicker.SelectedDate = null;

            view.Filter = null;
            view.Refresh();
        }

        private void ReservationsDG_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //if (e.PropertyName.ToLower() == "IsActive".ToLower())
            //{
            //    e.Column.Visibility = Visibility.Collapsed;
            //}
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var addReservationWindow = new AddEditReservation();

            Hide();
            if (addReservationWindow.ShowDialog() == true)
            {
                FillData();
            }
            Show();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedReservation = (Reservation)view.CurrentItem;

            if (selectedReservation != null)
            {
                var editReservationWindow = new AddEditReservation(selectedReservation);

                Hide();

                if (editReservationWindow.ShowDialog() == true)
                {
                    FillData();
                }

                Show();
            }
        }

        private void ReservationsDG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedReservation = (Reservation)view.CurrentItem;

            if (selectedReservation != null)
            {
                var detailsWindow = new ReservationDetailsWindow(selectedReservation,
                                                                roomService.GetRoomByReservation(selectedReservation),
                                                                guestService.GetGuestByReservation(selectedReservation));
                detailsWindow.ShowDialog();
            }
        }


        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (view.CurrentItem == null) { return; }

            var selectedReservation = view.CurrentItem as Reservation;

            if (MessageBox.Show($"Are you sure that you want to delete reservation {selectedReservation!.Id}?",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                selectedReservation.IsActive = false;

                reservationService = new ReservationService();
                reservationService.SaveReservation(selectedReservation);


                view.Refresh();
            }
            else
            {

            }
        }
    }
}
