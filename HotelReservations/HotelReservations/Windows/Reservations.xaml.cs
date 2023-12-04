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
    /// Interaction logic for Reservations.xaml
    /// </summary>
    public partial class Reservations : Window
    {
        private ICollectionView view;
        private ReservationService reservationService;
        public Reservations()
        {
            InitializeComponent();
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

        private bool DoFilter(object reservationObject)
        {
            var reservation = reservationObject as Reservation;

            if (reservation != null && reservation.IsActive)
            {
                return true;
            }

            return false;
        }

        private void ReservationsDG_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.ToLower() == "IsActive".ToLower())
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
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
