using HotelReservations.Exceptions;
using HotelReservations.Model;
using HotelReservations.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                Hotel.GetInstance().PriceList.Add(price);
            }
            else
            {
                priceRepository.Update(price);
                var index = Hotel.GetInstance().PriceList.FindIndex(r => r.Id == price.Id);
                Hotel.GetInstance().PriceList[index] = price;
            }
        }

    }
}