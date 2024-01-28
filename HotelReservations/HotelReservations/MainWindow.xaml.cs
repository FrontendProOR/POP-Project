using HotelReservations.Windows;
using System.Windows;

namespace HotelReservations
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.ShowDialog();
        }
    }
}
