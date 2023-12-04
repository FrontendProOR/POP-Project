using HotelReservations.Model;
using HotelReservations.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        }

        public void FillData()
        {
            var roomService = new RoomService();
            var rooms = roomService.GetAllRooms();
            
            view = CollectionViewSource.GetDefaultView(rooms);
            view.Filter = DoFilter;
            
            RoomsDG.ItemsSource = null;
            RoomsDG.ItemsSource = view;
            RoomsDG.IsSynchronizedWithCurrentItem = true;
        }

        private bool DoFilter(object roomObject)
        {
            var room = roomObject as Room;

            var roomNumberSearchParam = RoomNumberSearchTB.Text;

            if (room.RoomNumber.Contains(roomNumberSearchParam))
            {
                return true;
            }

            return false;
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
