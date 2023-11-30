using HotelReservations.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HotelReservations.Windows
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _surname;
        private string _username;
        private string _jmbg;
        private string _password;
        private string _userType;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Surname
        {
            get { return _surname; }
            set
            {
                if (_surname != value)
                {
                    _surname = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public string JMBG
        {
            get { return _jmbg; }
            set
            {
                if (_jmbg != value)
                {
                    _jmbg = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        public string UserType
        {
            get { return _userType; }
            set
            {
                if (_userType != value)
                {
                    _userType = value;
                    OnPropertyChanged();
                }
            }
        }
        private UserType _userTypeObject;

        public UserType UserTypeObject
        {
            get { return _userTypeObject; }
            set
            {
                if (_userTypeObject != value)
                {
                    //_userTypeObject = value;
                    //OnPropertyChanged();
                    _userTypeObject = value;
                    OnPropertyChanged(nameof(UserTypeObject));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
