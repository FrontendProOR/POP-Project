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
    /// Interaction logic for ReservationGuests.xaml
    /// </summary>
    public partial class ReservationGuests : Window
    {
        private ICollectionView view;
        public Guest selectedGuest = null;

        public ReservationGuests()
        {
            InitializeComponent();
            FillData();

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnPickGuest_Click(object sender, RoutedEventArgs e)
        {
            selectedGuest = GuestsDG.SelectedItem as Guest;
            this.DialogResult = true;
            this.Close();
        }

        private void GuestsDG_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.ToLower() == "IsActive".ToLower())
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
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
                return true;
            }

            return false;
        }
    }
}
