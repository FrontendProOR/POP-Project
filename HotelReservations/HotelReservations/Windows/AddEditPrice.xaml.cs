using HotelReservations.Model;
using HotelReservations.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for AddEditPrice.xaml
    /// </summary>

    public partial class AddEditPrice : Window
    {
        private PriceService priceService;

        private Price contextPrice;
        public AddEditPrice(Price? price = null)
        {
            if (price == null)
            {
                contextPrice = new Price();
            }
            else
            {
                contextPrice = price.Clone();
            }

            InitializeComponent();
            priceService = new PriceService();

            AdjustWindow(price);

            this.DataContext = contextPrice;
        }

        public void AdjustWindow(Price? price = null)
        {
            if (price != null)
            {
                Title = "Edit Price";
            }
            else
            {
                Title = "Add Price";
            }

            var roomTypes = Hotel.GetInstance().RoomTypes;
            RoomTypesCB.ItemsSource = roomTypes;

            ReservationTypesCB.ItemsSource = Enum.GetValues(typeof(ReservationType));
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

            if (contextPrice.PriceValue == 0)
            {
                MessageBox.Show("Fill required fields.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (Price current in priceService.GetPriceList())
            {
                if (current.RoomType.Equals(contextPrice.RoomType) && current.ReservationType.Equals(contextPrice.ReservationType) && current.IsActive == true)
                {
                    MessageBox.Show("Price with this Room and Reservation type already exist!.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            priceService.SavePrice(contextPrice);

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
