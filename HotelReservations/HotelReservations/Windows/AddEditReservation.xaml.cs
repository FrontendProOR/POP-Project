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

            AdjustWindow(reservation);

            this.DataContext = contextReservation;
        }

        public void FillData()
        {
            var reservationService = new ReservationService();
            var rGuest = reservationService.GetAllRGuests();

            view = CollectionViewSource.GetDefaultView(rGuest);
            view.Filter = DoFilter;


            ReservationGuestsDG.ItemsSource = null;
            ReservationGuestsDG.ItemsSource = view;
            ReservationGuestsDG.IsSynchronizedWithCurrentItem = true;
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

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

            if (contextReservation.TotalPrice == 0)
            {
                MessageBox.Show("Fill required fields.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            reservationService.SaveReservation(contextReservation);

            contextReservationGuest.ReservationId = contextReservation;
            contextReservationGuest.GuestId = contextGuest;
            reservationGuestService.SaveReservationGuest(contextReservationGuest);

            DialogResult = true;
            Close();
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
                Hotel.GetInstance().RGuests.Add(rg.selectedGuest);
                contextGuest = rg.selectedGuest;

            }
            FillData();
        }

    }
}
