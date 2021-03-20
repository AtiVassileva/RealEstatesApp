using System.Collections.Generic;
using RealEstates.Services.Models;

namespace RealEstates.Services
{
    public interface IPropertiesService
    {
        void Add(string district, int price, int floor, int totalFloors,
            int size, int yardSize, int year,
            string propertyType, string buildingType);

        IEnumerable<PropertyInfoDto> Search(int minPrice, int maxPrice, int minSize, int maxSize);

        decimal GetAveragePricePerSquareMeter();

        decimal GetAveragePricePerSquareMeter(int districtId);

        double GetAverageSizeForDistrict(int districtId);

        IEnumerable<PropertyFullInfoDto> GetFullData(int count);

    }
}
