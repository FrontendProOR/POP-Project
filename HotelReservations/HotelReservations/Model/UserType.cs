namespace HotelReservations.Model
{
    [Serializable]
    public class UserType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            var other = obj as UserType;
            if (other == null) return false;

            return Id == other.Id;
        }

        public static implicit operator UserType(string v)
        {
            throw new NotImplementedException();
        }

        public UserType Clone()
        {   
            var clone = new UserType();
            clone.Id = Id;
            clone.Name = Name;
            clone.IsActive = IsActive;
            return clone;
        }
    }
}
