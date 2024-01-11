using HotelReservations.Model;
using HotelReservations.Service;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HotelReservations.Windows
{
    /// <summary>
    /// Interaction logic for AddEditReservation.xaml
    /// </summary>
    public partial class AddEditReservation : Window
    {
        private ICollectionView view;
        private ReservationService reservationService;
        private Reservation contextReservation;
        private ReservationGuest contextReservationGuest;
        private ReservationGuestService reservationGuestService;
        private Guest contextGuest;
        private RoomService roomService;
        public AddEditReservation(Reservation? reservation = null)
        {
            if (reservation == null)
            {
                contextReservation = new Reservation();
            }
            else
            {
                contextReservation = reservation.Clone();
            }

            InitializeComponent();
            FillData();
            reservationService = new ReservationService();
            reservationGuestService = new ReservationGuestService();
            contextReservationGuest = new ReservationGuest();
            contextGuest = new Guest();
            roomService = new RoomService();
            AdjustWindow(reservation);

            this.DataContext = contextReservation;
        }

        public void FillData()
        {
            //var reservationService = new ReservationGuestService();
            //var rGuest = reservationService.GetAllReservationGuests();

            //view = CollectionViewSource.GetDefaultView(rGuest);
            ////view.Filter = DoFilter;

            //ReservationGuestsDG.ItemsSource = null;
            //ReservationGuestsDG.ItemsSource = view;
            //ReservationGuestsDG.IsSynchronizedWithCurrentItem = true;


            var roomService = new RoomService();
            var rooms = roomService.GetAllRooms().Where(r => !r.IsDeleted).ToList();
            var roomTypes = roomService.GetAllRoomTypes();


            RoomTypeSearchCB.ItemsSource = roomTypes;
            view = CollectionViewSource.GetDefaultView(rooms);
            view.Filter = DoRoomFilter;

            AvailableRoomsDG.ItemsSource = null;
            AvailableRoomsDG.ItemsSource = view;
            AvailableRoomsDG.IsSynchronizedWithCurrentItem = true;
        }


        private bool DoFilter(object guestObject)
        {
            var guest = guestObject as Guest;

            if (guest != null && guest.IsActive)
            {
                return true;
            }

            return false;
        }
        private bool DoRoomFilter(object roomObject)
        {
            var room = roomObject as Room;

            if (room != null && !room.IsDeleted)
            {
                return true;
            }

            return false;
        }


        private void ReservationGuestsDG_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.ToLower() == "IsActive".ToLower())
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }

        public void AdjustWindow(Reservation? reservation = null)
        {
            if (reservation != null)
            {
                Title = "Edit Reservation";
            }
            else
            {
                Title = "Add Reservation";
            }

            ReservationTypesCB.ItemsSource = Enum.GetValues(typeof(ReservationType));
        }

        //private void SaveBtn_Click(object sender, RoutedEventArgs e)
        //{

        //    if (contextReservation.TotalPrice == 0)
        //    {
        //        MessageBox.Show("Fill required fields.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }

        //    reservationService.SaveReservation(contextReservation);

        //    contextReservationGuest.ReservationId = contextReservation;
        //    contextReservationGuest.GuestId = contextGuest;
        //    reservationGuestService.SaveReservationGuest(contextReservationGuest);

        //    DialogResult = true;
        //    Close();
        //}

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (contextReservation.TotalPrice == 0)
            {
                MessageBox.Show("Fill required fields.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Retrieve selected room number from the AvailableRoomsDG DataGrid
            var selectedRoom = (Room)AvailableRoomsDG.SelectedItem;

            if (selectedRoom == null)
            {
                MessageBox.Show("Please select a room.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Assign the selected room number to the contextReservation object
            contextReservation.RoomNumber = selectedRoom.RoomNumber;

            // Save reservation and associated data
            reservationService.SaveReservation(contextReservation);

            contextReservationGuest.ReservationId = contextReservation;
            contextReservationGuest.GuestId = contextGuest;
            reservationGuestService.SaveReservationGuest(contextReservationGuest);

            DialogResult = true;
            Close();
        }


        private void AvailableRoomsDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle the selection changed event here if needed
        }

        private void AvailableRoomsDG_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            // Handle the auto-generating column event here if needed
        }
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnPickGuest_Click(object sender, RoutedEventArgs e)
        {
            ReservationGuests rg = new ReservationGuests();
            if (rg.ShowDialog() == true)
            {
                contextGuest = rg.selectedGuest;
                Hotel.GetInstance().RGuests.Add(rg.selectedGuest);

            }
            FillData();
        }

        private void ReservationGuestsDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void RoomTypeSearchCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyRoomSearchFilters();
        }
        private void ApplyRoomSearchFilters()
        {
            string roomNumber = RoomNumberSearchTB.Text;
            string roomType = (RoomTypeSearchCB.SelectedItem as RoomType)?.Name;

            // Call your service methods to get the filtered list based on search criteria
            List<Room> filteredRooms = roomService.GetFilteredRoomsByRoomNumberAndRoomType(roomNumber, roomType, true);

            // Update the DataGrid with the filtered list
            AvailableRoomsDG.ItemsSource = filteredRooms;
        }
        private void RoomNumberSearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyRoomSearchFilters();
        }

    }
}
