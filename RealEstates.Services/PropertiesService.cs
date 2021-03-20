using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using RealEstates.Data;
using RealEstates.Services.Models;
using RealEstates.Models;

namespace RealEstates.Services
{
    public class PropertiesService : BaseService, IPropertiesService
    {
        private readonly RealEstatesDbContext dbContext;

        public PropertiesService(RealEstatesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(string district, int price, int floor, int totalFloors, int size, int yardSize, int year,
            string propertyType,
            string buildingType)
        {
            var property = new Property
            {
                Size = size,
                Price = price <= 0 ? null : price,
                Floor = floor <= 0 || floor > 255 ? null : (byte)floor,
                TotalFloors = totalFloors <= 0 || totalFloors > 255 ? null : (byte)totalFloors,
                YardSize = yardSize <= 0 ? null : yardSize,
                Year = year <= 1800 ? null : year,
            };

            var dbDistrict = this.dbContext.Districts.FirstOrDefault(d => d.Name == district) ?? new District { Name = district };

            property.District = dbDistrict;

            var dbPropertyType = this.dbContext.PropertyTypes
                .FirstOrDefault(pt => pt.Name == propertyType) ?? new PropertyType {Name = propertyType};

            property.PropertyType = dbPropertyType;
            
            var dbBuildingType = this.dbContext.BuildingTypes
                .FirstOrDefault(bt => bt.Name == buildingType) ?? new BuildingType {Name = buildingType};

            property.BuildingType = dbBuildingType;

            dbContext.Properties.Add(property);
            dbContext.SaveChanges();
        }

        public IEnumerable<PropertyInfoDto> Search(int minPrice, int maxPrice, int minSize, int maxSize)
        {
            var matchingProperties = this.dbContext.Properties
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice && p.Size >= minSize && p.Size <= maxSize)
                .ProjectTo<PropertyInfoDto>(this.Mapper.ConfigurationProvider)
                .ToList();

            return matchingProperties;
        }

        public decimal GetAveragePricePerSquareMeter()
        {
            return this.dbContext.Properties.Where(p => p.Price.HasValue).Average(p => p.Price / (decimal)p.Size) ?? 0;
        }
        
        public decimal GetAveragePricePerSquareMeter(int districtId)
        {
            return this.dbContext.Properties.Where(p => p.Price.HasValue && p.DistrictId == districtId).Average(p => p.Price / (decimal)p.Size) ?? 0;
        }

        public double GetAverageSizeForDistrict(int districtId)
        {
            return this.dbContext.Properties.Where(p => p.Price.HasValue && p.DistrictId == districtId).Average(p => p.Size);
        }

        public IEnumerable<PropertyFullInfoDto> GetFullData(int count)
        {
            var properties = this.dbContext.Properties
                .ProjectTo<PropertyFullInfoDto>(this.Mapper.ConfigurationProvider)
                .OrderByDescending(p => p.Size)
                .ThenBy(p => p.Size)
                .ThenBy(p => p.Year)
                .Take(count)
                .ToList();

            return properties;
        }
    }
}