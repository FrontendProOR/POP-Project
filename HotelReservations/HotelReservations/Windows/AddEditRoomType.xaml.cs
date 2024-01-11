using HotelReservations.Model;
using HotelReservations.Service;
using System.Windows;

namespace HotelReservations.Windows
{
    /// <summary>
    /// Interaction logic for AddEditRoomType.xaml
    /// </summary>
    public partial class AddEditRoomType : Window
    {

        private RoomTypeService roomTypeService;

        private RoomType contextRoomType;

        public AddEditRoomType(RoomType? roomType = null)
        {
            if (roomType == null)
            {
                contextRoomType = new RoomType();
            }
            else
            {
                contextRoomType = roomType.Clone();
            }
            InitializeComponent();

            roomTypeService = new RoomTypeService();

            AdjustWindow(roomType);

            this.DataContext = contextRoomType;
        }

        

        public void AdjustWindow(RoomType? roomType = null)
        {
            if (roomType != null)
            {
                Title = "Edit Room Type";
            }
            else
            {
                Title = "Add Room Type";
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(contextRoomType.Name))
            {
                MessageBox.Show("Fill required fields.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            roomTypeService.SaveRoomType(contextRoomType);

            DialogResult = true;
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
