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
    /// Interaction logic for RoomTypes.xaml
    /// </summary>
    public partial class RoomTypes : Window
    {
        private ICollectionView view;
        private RoomTypeService roomTypeService;
        public RoomTypes()
        {
            InitializeComponent();
            FillData();
        }

        public void FillData()
        {
            var roomTypeService = new RoomTypeService();
            var roomTypes = roomTypeService.GetAllRoomTypes();

            view = CollectionViewSource.GetDefaultView(roomTypes);
            view.Filter = DoFilter;


            RoomTypesDG.ItemsSource = null;
            RoomTypesDG.ItemsSource = view;
            RoomTypesDG.IsSynchronizedWithCurrentItem = true;
        }

        private bool DoFilter(object roomObject)
        {
            var roomType = roomObject as RoomType;

            if (roomType != null && roomType.IsActive)
            {
                var nameSearchParam = NameSearchTB.Text;

                if (roomType.Name.Contains(nameSearchParam))
                {
                    return true;
                }
            }

            return false;
        }

        private void RoomTypesDG_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.ToLower() == "IsActive".ToLower())
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var addRoomTypeWindow = new AddEditRoomType();

            Hide();
            if (addRoomTypeWindow.ShowDialog() == true)
            {
                FillData();
            }
            Show();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedRoomType = (RoomType)view.CurrentItem;

            if (selectedRoomType != null)
            {
                var editRoomTypeWindow = new AddEditRoomType(selectedRoomType);

                Hide();

                if (editRoomTypeWindow.ShowDialog() == true)
                {
                    FillData();
                }

                Show();
            }
        }

        private void NameSearchTB_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            view.Refresh();
        }


        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (view.CurrentItem == null) { return; }

            var selectedRoomType = view.CurrentItem as RoomType;

            if (MessageBox.Show($"Are you sure that you want to delete room {selectedRoomType!.Name}?",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                selectedRoomType.IsActive = false;

                roomTypeService = new RoomTypeService();
                roomTypeService.SaveRoomType(selectedRoomType);


                view.Refresh();
            }
            else
            {

            }
        }
    }
}
