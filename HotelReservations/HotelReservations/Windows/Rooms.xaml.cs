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
    /// Interaction logic for Rooms.xaml
    /// </summary>
    public partial class Rooms : Window
    {
        private ICollectionView view;
        private RoomService roomService;
        public Rooms()
        {
            roomService = new RoomService();
            InitializeComponent();
            FillData();
            LoadRoomTypes();
        }

        public void FillData()
        {
            var roomService = new RoomService();
            var rooms = roomService.GetAllRooms().Where(r => !r.IsDeleted).ToList();
            
            view = CollectionViewSource.GetDefaultView(rooms);
            view.Filter = DoFilter;
            
            RoomsDG.ItemsSource = null;
            RoomsDG.ItemsSource = view;
            RoomsDG.IsSynchronizedWithCurrentItem = true;
        }

        private void LoadRoomTypes()
        {
            List<RoomType> roomTypes = new RoomService().GetAllRoomTypes();
            RoomTypeSearchCB.ItemsSource = roomTypes;
            RoomTypeSearchCB.DisplayMemberPath = "Name";
        }
        private void RoomTypeSearchCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplySearchFilters();
        }

        private void IsActiveCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ApplySearchFilters();
        }

        private void IsActiveCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ApplySearchFilters();
        }
        private void ApplySearchFilters()
        {
            string roomNumber = RoomNumberSearchTB.Text;
            string roomType = (RoomTypeSearchCB.SelectedItem as RoomType)?.Name;
            bool isActive = IsActiveCheckBox.IsChecked ?? false;

            // Call your service methods to get the filtered list based on search criteria
            List<Room> filteredRooms = roomService.GetFilteredRooms(roomNumber, roomType, isActive);

            // Update the DataGrid with the filtered list
            RoomsDG.ItemsSource = filteredRooms;
        }
        //private bool DoFilter(object roomObject)
        //{
        //    var room = roomObject as Room;

        //    var roomNumberSearchParam = RoomNumberSearchTB.Text;

        //    if (room.RoomNumber.Contains(roomNumberSearchParam))
        //    {
        //        return true;
        //    }

        //    return false;
        //}
        public List<Room> GetFilteredRooms(string roomNumber, string roomTypeName, bool isActive)
        {
            var rooms = Hotel.GetInstance().Rooms;

            var filteredRooms = rooms.Where(r =>
                (string.IsNullOrEmpty(roomNumber) || r.RoomNumber.ToLower().Contains(roomNumber.ToLower())) &&
                (string.IsNullOrEmpty(roomTypeName) || r.RoomType.Name.ToLower().Contains(roomTypeName.ToLower())) &&
                (isActive ? r.IsActive : !r.IsDeleted)
            ).ToList();

            return filteredRooms;
        }

        private bool DoFilter(object roomObject)
        {
            var room = roomObject as Room;

            var roomNumberSearchParam = RoomNumberSearchTB.Text.ToLower();

            // Check if the room number contains the search parameter
            return room.RoomNumber.ToLower().Contains(roomNumberSearchParam);
        }


        private void RoomsDG_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //if (e.PropertyName.ToLower() == "IsActive".ToLower())
            //{
            //    e.Column.Visibility = Visibility.Collapsed;
            //}
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var addRoomWindow = new AddEditRoom();

            Hide();
            if (addRoomWindow.ShowDialog() == true)
            {
                FillData();
            }
            Show();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedRoom = (Room)view.CurrentItem;

            if (selectedRoom != null)
            {
                var editRoomWindow = new AddEditRoom(selectedRoom);

                Hide();

                if (editRoomWindow.ShowDialog() == true)
                {
                    FillData();
                }

                Show();
            }
        }

        private void RoomNumberSearchTB_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            view.Refresh();
        }
        private void PermanentDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedRoom = RoomsDG.SelectedItem as Room;
            if (selectedRoom == null)
            {
                MessageBox.Show("Please select a room to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

                if (MessageBox.Show($"Are you sure that you want to permanently delete room {SelectedRoomId}?",
                    "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    roomService.PermanentDeleteRoom(SelectedRoomId);
                    FillData();
                }
            
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedRoom = RoomsDG.SelectedItem as Room;
            if (selectedRoom == null)
            {
                MessageBox.Show("Please select a room to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show($"Are you sure that you want to delete room {SelectedRoomId}?",
                    "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    roomService.DeleteRoom(SelectedRoomId);
                    FillData();
                }
            
        }
        
        private int SelectedRoomId
        {
            get
            {
                if (RoomsDG.SelectedValue != null)
                {
                    return (int)RoomsDG.SelectedValue;
                }
                return 0; // or any default value
            }
        }
        private void RoomsDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RoomNumberSearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
