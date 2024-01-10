using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Model
{
    [Serializable]
    public class RoomType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfBeds {  get; set; }
        public bool IsActive { get; set; } = true;
        public override string ToString()
        {
            return Name;
        }

        // rt1.Equals(null)
        // rt1.Equals(hootel)
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            var other = obj as RoomType;
            if (other == null) return false;

            return Id == other.Id;
        }

        public RoomType Clone()
        {
            var clone = new RoomType();
            clone.Id = Id;
            clone.Name = Name;
            clone.NumberOfBeds = NumberOfBeds;
            clone.IsActive = IsActive;

            return clone;
        }
    }
}
