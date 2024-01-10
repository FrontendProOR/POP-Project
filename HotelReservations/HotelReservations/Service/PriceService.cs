using HotelReservations.Model;
using HotelReservations.Repository;

namespace HotelReservations.Service
{
    public class PriceService
    {
        IPriceRepository priceRepository;

        public PriceService()
        {
            priceRepository = new PriceRepository();
        }

        public List<Price> GetPriceList()
        {
            return priceRepository.GetAll();
        }

        public List<Price> GetSortedPriceList()
        {
            var priceList = Hotel.GetInstance().PriceList;
            priceList.Sort((r1, r2) => r1.PriceValue.CompareTo(r2.PriceValue));
            return priceList;
        }

        public void SavePrice(Price price)
        {
            if (price.Id == 0)
            {
                price.Id = priceRepository.Insert(price);
                //Hotel.GetInstance().PriceList.Add(price);
            }
            else
            {
                priceRepository.Update(price);
                //var index = Hotel.GetInstance().PriceList.FindIndex(r => r.Id == price.Id);
                //Hotel.GetInstance().PriceList[index] = price;
            }
        }
        public double GetPriceValueByRoomTypeName(string roomTypeName)
        {
            var priceList = GetPriceList();
            var price = priceList.FirstOrDefault(p => p.RoomType.Name == roomTypeName && p.IsActive);

            return price?.PriceValue ?? 0;
        }
    }
}