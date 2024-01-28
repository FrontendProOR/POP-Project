using HotelReservations.Model;
using HotelReservations.Service;
using System.Windows;

namespace HotelReservations.Windows
{
    /// <summary>
    /// Interaction logic for AddEditGuest.xaml
    /// </summary>
    public partial class AddEditGuest : Window
    {
        private GuestService guestService;
        private Guest contextGuest;

        public AddEditGuest(Guest? guest = null)
        {
            if (guest == null)
            {
                contextGuest = new Guest();
            }
            else
            {
                contextGuest = guest.Clone();
            }

            InitializeComponent();
            guestService = new GuestService();

            AdjustWindow(guest);

            this.DataContext = contextGuest;
        }

        public void AdjustWindow(Guest? guest = null)
        {
            if (guest != null)
            {
                Title = "Edit Guest";
            }
            else
            {
                Title = "Add Guest";
            }

        }

        //private void SaveBtn_Click(object sender, RoutedEventArgs e)
        //{

        //    if (string.IsNullOrEmpty(contextGuest.Name))
        //    {
        //        MessageBox.Show("Fill required fields.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }

        //    guestService.SaveGuest(contextGuest);

        //    DialogResult = true;
        //    Close();
        //}

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            // Validate Name
            if (string.IsNullOrEmpty(contextGuest.Name))
            {
                MessageBox.Show("Name is a required field.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate Surname
            if (string.IsNullOrEmpty(contextGuest.Surname))
            {
                MessageBox.Show("Surname is a required field.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate IDNumber
            if (string.IsNullOrEmpty(contextGuest.IDNumber))
            {
                MessageBox.Show("ID Number is a required field.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate IDNumber format (example: assuming it should be numeric)
            if (!IsNumeric(contextGuest.IDNumber))
            {
                MessageBox.Show("ID Number should be a numeric value.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            guestService.SaveGuest(contextGuest);

            DialogResult = true;
            Close();
        }

        private bool IsNumeric(string value)
        {
            return int.TryParse(value, out _);
        }


        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
