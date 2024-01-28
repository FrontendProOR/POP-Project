using HotelReservations.Model;
using HotelReservations.Service;
using System.Windows;

namespace HotelReservations.Windows
{

    /// <summary>
    /// Interaction logic for AddEditPrice.xaml
    /// </summary>

    public partial class AddEditPrice : Window
    {
        private PriceService priceService;
        private RoomService roomService;
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
            roomService = new RoomService();

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

            if(roomService != null)
            {
            Hotel.GetInstance().RoomTypes = roomService.GetAllRoomTypes();
            }
            var roomTypes = Hotel.GetInstance().RoomTypes;
            


            RoomTypesCB.ItemsSource = roomTypes;

            ReservationTypesCB.ItemsSource = Enum.GetValues(typeof(ReservationType));
        }

        //private void SaveBtn_Click(object sender, RoutedEventArgs e)
        //{

        //    if (contextPrice.PriceValue == 0)
        //    {
        //        MessageBox.Show("Fill required fields.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }

        //    foreach (Price current in priceService.GetPriceList())
        //    {
        //        if (current.RoomType.Equals(contextPrice.RoomType) && current.ReservationType.Equals(contextPrice.ReservationType) && current.IsActive == true)
        //        {
        //            MessageBox.Show("Price with this Room and Reservation type already exist!.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //            return;
        //        }
        //    }

        //    priceService.SavePrice(contextPrice);

        //    DialogResult = true;
        //    Close();
        //}
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            // Validate PriceValue
            if (!ValidatePriceValue())
            {
                MessageBox.Show("Please enter a valid Price Value.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Check if PriceValue is zero
            if (contextPrice.PriceValue == 0)
            {
                MessageBox.Show("Price Value cannot be zero.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Check if a price with the same RoomType and ReservationType already exists
            foreach (Price current in priceService.GetPriceList())
            {
                if (current.RoomType.Equals(contextPrice.RoomType) && current.ReservationType.Equals(contextPrice.ReservationType) && current.IsActive)
                {
                    MessageBox.Show("Price with this Room and Reservation type already exists!", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Save the price if all validations pass
            priceService.SavePrice(contextPrice);

            DialogResult = true;
            Close();
        }

        private bool ValidatePriceValue()
        {
            // Check if PriceValueTB is empty or null
            if (string.IsNullOrEmpty(PriceValueTB.Text) || string.IsNullOrWhiteSpace(PriceValueTB.Text))
            {
                return false;
            }

            // Try parsing the PriceValue as a decimal
            if (decimal.TryParse(PriceValueTB.Text, out decimal priceValue))
            {
                // Check if the parsed value is greater than or equal to zero
                return priceValue >= 0;
            }

            return false;
        }


        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
