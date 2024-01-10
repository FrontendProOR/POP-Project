using HotelReservations.Model;

namespace HotelReservations.Repository
{
    public interface IPriceRepository
    {
        List<Price> GetAll();
        int Insert(Price price);
        void Update(Price price);
        void Save(List<Price> priceList);
    }
}
