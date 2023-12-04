using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace HotelReservations.Model
{
    public class User
    {
        public User()
        {
        }

        public User(int id, string name, string surname, string jMBG, string username, string password, UserType userType, bool isDeleted = false)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Surname = surname ?? throw new ArgumentNullException(nameof(surname));
            JMBG = jMBG ?? throw new ArgumentNullException(nameof(jMBG));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            UserType = userType ?? throw new ArgumentNullException(nameof(userType));
            IsDeleted = isDeleted;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string JMBG { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
        public bool IsDeleted { get; set; } // Updated property name to follow C# naming conventions

        public override string ToString()
        {
            return $"{Name} {Surname} ({Username}) - {UserType}";
        }
    }

}
