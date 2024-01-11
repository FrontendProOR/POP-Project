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
    /// Interaction logic for Guests.xaml
    /// </summary>
    public partial class Guests : Window
    {
        private ICollectionView view;
        private GuestService guestService;
        public Guests()
        {
            InitializeComponent();
            FillData();
        }

        public void FillData()
        {
            var guestService = new GuestService();
            var guests = guestService.GetAllGuests();

            view = CollectionViewSource.GetDefaultView(guests);
            view.Filter = DoFilter;


            GuestsDG.ItemsSource = null;
            GuestsDG.ItemsSource = view;
            GuestsDG.IsSynchronizedWithCurrentItem = true;
        }

        private bool DoFilter(object guestObject)
        {
            var guest = guestObject as Guest;

            if (guest != null && guest.IsActive)
            {
                var IDNumberSearchParam = IDNumberSearchTB.Text;

                if (guest.IDNumber.Contains(IDNumberSearchParam))
                {
                    return true;
                }
            }

            return false;
        }

        private void GuestsDG_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.ToLower() == "IsActive".ToLower())
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var addGuestWindow = new AddEditGuest();

            Hide();
            if (addGuestWindow.ShowDialog() == true)
            {
                FillData();
            }
            Show();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedGuest = (Guest)view.CurrentItem;

            if (selectedGuest != null)
            {
                var editGuestWindow = new AddEditGuest(selectedGuest);

                Hide();

                if (editGuestWindow.ShowDialog() == true)
                {
                    FillData();
                }

                Show();
            }
        }

        private void IDNumberSearchTB_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            view.Refresh();
        }


        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (view.CurrentItem == null) { return; }

            var selectedGuest = view.CurrentItem as Guest;

            if (MessageBox.Show($"Are you sure that you want to delete guest {selectedGuest!.IDNumber}?",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                selectedGuest.IsActive = false;

                guestService = new GuestService();
                guestService.SaveGuest(selectedGuest);


                view.Refresh();
            }
            else
            {

            }
        }
    }
}
