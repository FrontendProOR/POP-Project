using HotelReservations.Model;
using HotelReservations.Service;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddEditUserTypes.xaml
    /// </summary>
    public partial class AddEditUserTypes : Window
    {
        private UserTypeService userTypeService;

        private UserType contextUserType;

        public AddEditUserTypes(UserType? userType = null)
        {
            if (userType == null)
            {
                contextUserType = new UserType();

                contextUserType.IsActive = true;
                
            }
            else
            {
                contextUserType = userType.Clone();
            }

            InitializeComponent();
            userTypeService = new UserTypeService();
            
            AdjustWindow(userType);

            this.DataContext = contextUserType;
        }

        private void AdjustWindow(UserType userType = null)
        {
            if (userType != null)
            {
                Title = "Edit user type";
            }
            else
            {
                Title = "Add user type";
            }
        }

        //private void SaveBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    contextUserType.Name = UserTypeNameTB.Text;
        //    userTypeService.SaveUserType(contextUserType);

        //    DialogResult = true;
        //    Close();
        //}

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            // Update the contextUserType.Name with the value from the TextBox
            contextUserType.Name = UserTypeNameTB.Text;

            // Check if the name is not empty before saving
            if (!string.IsNullOrWhiteSpace(contextUserType.Name))
            {
                userTypeService.SaveUserType(contextUserType);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("User type name cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
