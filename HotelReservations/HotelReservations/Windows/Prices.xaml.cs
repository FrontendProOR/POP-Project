using HotelReservations.Model;
using HotelReservations.Service;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HotelReservations.Windows
{
    /// <summary>
    /// Interaction logic for PriceList.xaml
    /// </summary>
    public partial class Prices : Window
    {
        private ICollectionView view;
        private PriceService priceService;
        public Prices()
        {
            InitializeComponent();
            FillData();
        }

        

        public void FillData()
        {
            var priceService = new PriceService();
            var prices = priceService.GetPriceList();

            view = CollectionViewSource.GetDefaultView(prices);
            view.Filter = DoFilter;


            PricesDG.ItemsSource = null;
            PricesDG.ItemsSource = view;
            PricesDG.IsSynchronizedWithCurrentItem = true;
        }

        private bool DoFilter(object priceObject)
        {
            var price = priceObject as Price;

            if (price != null && price.IsActive)
            {
                return true;
            }

            return false;
        }

        private void PricesDG_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.ToLower() == "IsActive".ToLower())
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var addPriceWindow = new AddEditPrice();

            Hide();
            if (addPriceWindow.ShowDialog() == true)
            {
                FillData();
            }
            Show();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedPrice = (Price)view.CurrentItem;

            if (selectedPrice != null)
            {
                var editPriceWindow = new AddEditPrice(selectedPrice);

                Hide();

                if (editPriceWindow.ShowDialog() == true)
                {
                    FillData();
                }

                Show();
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (view.CurrentItem == null) { return; }

            var selectedPrice = view.CurrentItem as Price;

            if (MessageBox.Show($"Are you sure that you want to delete price {selectedPrice!.PriceValue}?",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                selectedPrice.IsActive = false;

                priceService = new PriceService();
                priceService.SavePrice(selectedPrice);


                view.Refresh();
            }
            else
            {

            }
        }
    }
}
