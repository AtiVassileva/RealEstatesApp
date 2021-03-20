using System;
using System.Collections.Generic;
using System.Linq;
using RealEstates.Data;
using RealEstates.Models;

namespace RealEstates.Services
{
    public class TagsService : BaseService, ITagsService
    {
        private readonly RealEstatesDbContext dbContext;
        private readonly IPropertiesService propertiesService;

        public TagsService(RealEstatesDbContext dbContext, IPropertiesService propertiesService)
        {
            this.dbContext = dbContext;
            this.propertiesService = propertiesService;
        }

        public void Add(string name, int? importance = null)
        {
            var tag = new Tag
            {
                Name = name,
                Importance = importance
            };

            this.dbContext.Tags.Add(tag);
            this.dbContext.SaveChanges();
        }

        public void BulkTagToProperties()
        {
            var allProperties = this.dbContext.Properties
                .ToList();
            
            foreach (var property in allProperties)
            {
                var propertyTags = new List<Tag>();

                var districtAveragePrice = this.propertiesService.GetAveragePricePerSquareMeter(property.DistrictId);

                if (property.Price >= districtAveragePrice)
                {
                    propertyTags.Add(this.GetTag("скъп-имот"));
                }
                else if (property.Price < districtAveragePrice)
                {
                    propertyTags.Add(this.GetTag("евтин-имот"));
                }

                var currentDate = DateTime.Now.AddYears(-15);

                if (property.Year.HasValue && property.Year <= currentDate.Year)
                {
                    propertyTags.Add(this.GetTag("старо-строителство"));
                }
                else if (property.Year.HasValue && property.Year > currentDate.Year)
                {
                    propertyTags.Add(this.GetTag("ново-строителство"));
                }

                var averagePropertySize = this.propertiesService.GetAverageSizeForDistrict(property.DistrictId);

                if (property.Size >= averagePropertySize)
                {
                    propertyTags.Add(this.GetTag("голям-имот"));
                }
                else if (property.Size < averagePropertySize)
                {
                    propertyTags.Add(this.GetTag("малък-имот"));
                }

                if (property.Floor.HasValue && property.Floor == 1)
                {
                    propertyTags.Add(this.GetTag("първи-етаж"));
                }
                else if (property.Floor.HasValue && property.TotalFloors.HasValue &&
                         property.Floor.Value == property.TotalFloors)
                {
                    propertyTags.Add(this.GetTag("последен-етаж"));
                }

                foreach (var tag in propertyTags)
                {
                    property.Tags.Add(tag);
                }

            }

            this.dbContext.SaveChanges();
        }

        private Tag GetTag(string tagName) =>
            this.dbContext.Tags
                .FirstOrDefault(t => t.Name == tagName);
    }
}
