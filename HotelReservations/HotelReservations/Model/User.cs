using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Model
{
    using System;

    namespace HotelReservations.Model
    {
        public class User
        {
            public User()
            {
            }

            public User(int id, string name, string surname, string jMBG, string username, string password, UserType userType)
            {
                Id = id;
                Name = name ?? throw new ArgumentNullException(nameof(name));
                Surname = surname ?? throw new ArgumentNullException(nameof(surname));
                JMBG = jMBG ?? throw new ArgumentNullException(nameof(jMBG));
                Username = username ?? throw new ArgumentNullException(nameof(username));
                Password = password ?? throw new ArgumentNullException(nameof(password));
                UserType = userType ?? throw new ArgumentNullException(nameof(userType));
            }

            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string JMBG { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public UserType UserType { get; set; }

            public override string ToString()
            {
                return $"{Name} {Surname} ({Username}) - {UserType}";
            }
        }
    }


}
