using HotelReservations.Model;

[Serializable]
public class Room
{
    public Room(int id, string roomNumber, bool hasTV, bool hasMiniBar, RoomType roomType, bool isActive, bool isDeleted)
    {
        Id = id;
        RoomNumber = roomNumber;
        HasTV = hasTV;
        HasMiniBar = hasMiniBar;
        RoomType = roomType;
        IsActive = isActive;
        IsDeleted = isDeleted;
    }

    public Room()
    {
    }

    public int Id { get; set; }
    public string roomNumber = string.Empty;
    public bool HasTV { get; set; }
    public bool HasMiniBar { get; set; }
    public RoomType RoomType { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }

    public string RoomNumber
    {
        get { return roomNumber; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("It's required");
            }

            roomNumber = value;
        }
    }

    public override string ToString()
    {
        return $"Room number: {RoomNumber}";
    }

    public Room Clone()
    {
        var clone = new Room();
        clone.Id = Id;
        clone.RoomNumber = RoomNumber;
        clone.HasTV = HasTV;
        clone.HasMiniBar = HasMiniBar;
        clone.IsActive = IsActive;
        clone.RoomType = RoomType;
        clone.IsDeleted = IsDeleted;
        return clone;
    }
}
