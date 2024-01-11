using HotelReservations.Windows;
using System.Windows;

namespace HotelReservations
{

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
             base.OnStartup(e);
            DataUtil.LoadData();
            Login loginWindow = new Login();
            loginWindow.ShowDialog();

        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            //DataUtil.PersistData();
        }
    }

}
