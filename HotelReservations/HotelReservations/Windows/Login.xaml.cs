using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace HotelReservations.Windows
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window, INotifyPropertyChanged
    {
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public Login()
        {
            InitializeComponent();
            DataContext = this; 
        }

        private void UserPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = UserPasswordBox.Password;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = Username;
            string password = Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username and password are required." + Username + Password);
                return;
            }

            // Perform authentication logic
            string userRole = IdentifyUserRole(username);
            if (AuthenticateUser(username, password) && userRole == "administrator")
            {
                MessageBox.Show("Administrator logged in.");
                adminWindow adminWindow = new adminWindow();
                adminWindow.Show();
                this.Close();
            }
            else if (AuthenticateUser(username, password) && userRole == "receptionist")
            {
                //MessageBox.Show("Receptionist logged in.");  
                ReceptionistWindow receptionistWindow = new ReceptionistWindow();
                receptionistWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
            //else if (AuthenticateUser(username, password) && userRole == "guest")
            //{
            //    //MessageBox.Show("Guest logged in.");  
            //    GuestWindow guestWindow = new GuestWindow();
            //    guestWindow.Show();
            //    this.Close();
            //}
        }

        private string IdentifyUserRole(string username)
        {
            string role = "";
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HotelReservationsG2;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

            string query = "SELECT user_type FROM dbo.[user] WHERE username = @Username";

            // Using statement for SqlConnection
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Add parameters to the query
                command.Parameters.AddWithValue("@Username", username);

                try
                {
                    connection.Open();

                    // ExecuteReader to get the result from the query
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Check if there is a result
                        if (reader.Read())
                        {
                            // Get the 'user_type' from the result
                            role = reader["user_type"].ToString();
                        }
                    }

                    // Return the obtained role
                    return role;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return "";
                }
            }
        }


        private bool AuthenticateUser(string username, string password)
        {
            // Connection string
            
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HotelReservationsG2;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
       
        string query = "SELECT COUNT(*) FROM dbo.[user] WHERE username = @Username AND password = @Password";

            // Using statement for SqlConnection
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Add parameters to the query
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                try
                {
                    connection.Open();

                    // ExecuteScalar returns the number of rows affected
                    int count = (int)command.ExecuteScalar();

                    return count > 0; // If count > 0, the user exists
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return false;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       

    }
}
