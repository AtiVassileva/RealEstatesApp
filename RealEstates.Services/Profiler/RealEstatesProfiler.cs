using System.Linq;
using AutoMapper;
using RealEstates.Models;
using RealEstates.Services.Models;

namespace RealEstates.Services.Profiler
{
    public class RealEstatesProfiler : Profile
    {
        public RealEstatesProfiler()
        {
            this.CreateMap<Property, PropertyInfoDto>()
                .ForMember(x => x.BuildingType, y=> y.MapFrom(p => p.BuildingType.Name))
                .ForMember(x => x.PropertyType, y => y.MapFrom(p => p.PropertyType.Name));

            this.CreateMap<District, DistrictInfoDto>()
                .ForMember(x => x.AveragePricePerSquareMeter, y => y.MapFrom(d => d.Properties
                    .Where(p => p.Price.HasValue)
                    .Average(p => p.Price / (decimal)p.Size) ?? 0));

            this.CreateMap<Property, PropertyFullInfoDto>()
                .ForMember(x => x.BuildingType, y=> y.MapFrom(p => p.BuildingType.Name))
                .ForMember(x => x.PropertyType, y => y.MapFrom(p => p.PropertyType.Name));

            this.CreateMap<Tag, TagInfoDto>();
        }
    }
}
