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
    /// Interaction logic for UserTypes.xaml
    /// </summary>
    public partial class UserTypes : Window
    {
        public UserTypes()
        {
            InitializeComponent();
            FillData();
        }
        private ICollectionView view;
        private UserTypeService userTypeService;
        

        public void FillData()
        {
            var userTypeService = new UserTypeService();
            var userTypes = userTypeService.GetAllUserTypes();

            view = CollectionViewSource.GetDefaultView(userTypes);
            view.Filter = DoFilter;


            UserTypesDG.ItemsSource = null;
            UserTypesDG.ItemsSource = view;
            UserTypesDG.IsSynchronizedWithCurrentItem = true;
        }

        private bool DoFilter(object userTypeObject)
        {
            var userType = userTypeObject as UserType;

            if (userType != null && userType.IsActive)
            {
                var userTypesSearchParam = UserTypesSearchTB.Text;

                // Check if the UserType's Name starts with the search parameter letter by letter
                if (IsStartsWith(userType.Name, userTypesSearchParam))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsStartsWith(string source, string prefix)
        {
            // Iterate over each character in the prefix
            for (int i = 0; i < prefix.Length; i++)
            {
                // Check if the corresponding character in the source matches
                if (i >= source.Length || source[i] != prefix[i])
                {
                    return false;
                }
            }

            return true;
        }

        private void UserTypesDG_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.ToLower() == "IsActive".ToLower())
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var UserTypesWindow = new AddEditUserTypes();

            Hide();
            if (UserTypesWindow.ShowDialog() == true)
            {
                FillData();
            }
            Show();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedUserType = (UserType)view.CurrentItem;
            selectedUserType.IsActive = true;

            if (selectedUserType != null)
            {
                var editUserTypeWindow = new AddEditUserTypes(selectedUserType);

                Hide();

                if (editUserTypeWindow.ShowDialog() == true)
                {
                    FillData();
                }

                Show();
            }
        }

        private void UserTypesSearchTB_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            view.Refresh();
        }


        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (view.CurrentItem == null) { return; }

            var selectedUserType = view.CurrentItem as UserType;

            if (MessageBox.Show($"Are you sure that you want to delete room {selectedUserType!.Name}?",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                selectedUserType.IsActive = false;

                userTypeService = new UserTypeService();
                userTypeService.SaveUserType(selectedUserType);


                view.Refresh();
            }
            else
            {

            }
        }
    
    }
}
