using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using RealEstates.Data;
using RealEstates.Services.Models;

namespace RealEstates.Services
{
    public class DistrictsService : BaseService, IDistrictsService
    {
        private readonly RealEstatesDbContext dbContext;

        public DistrictsService(RealEstatesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<DistrictInfoDto> GetMostExpensiveDistricts(int count)
        {
            var mostExpensiveDistricts = this.dbContext.Districts
                .ProjectTo<DistrictInfoDto>(this.Mapper.ConfigurationProvider)
                .OrderByDescending(d => d.AveragePricePerSquareMeter).Take(count).ToList();

            return mostExpensiveDistricts;
        }
    }
}