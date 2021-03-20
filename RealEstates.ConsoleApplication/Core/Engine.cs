using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using RealEstates.Data;
using RealEstates.Services;
using RealEstates.Services.Models;

namespace RealEstates.ConsoleApplication.Core
{
    public class Engine
    {
        public Engine()
        {
        }
        public void Run()
        {
            Console.OutputEncoding = Encoding.Unicode;
            var context = new RealEstatesDbContext();
            context.Database.Migrate();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose an option:");
                Console.WriteLine("0. EXIT");
                Console.WriteLine("1. Property search");
                Console.WriteLine("2. Most expensive districts");
                Console.WriteLine("3. Average price per square meter");
                Console.WriteLine("4. Add tag");
                Console.WriteLine("5. Bulk tags to properties");
                Console.WriteLine("6. Get full data about properties");

                var parsed = int.TryParse(Console.ReadLine(), out var option);

                if (parsed && option == 0)
                {
                    break;
                }

                if (parsed && (option >= 1 && option <= 6))
                {
                    switch (option)
                    {
                        case 1:
                            PropertySearch(context);
                            break;
                        case 2:
                            MostExpensiveDistricts(context);
                            break;
                        case 3:
                            AveragePricePerSquareMeter(context);
                            break;
                        case 4:
                            AddTag(context);
                            break;
                        case 5:
                            BulkTagsToProperties(context);
                            break;
                        case 6:
                            GetFullDataAboutProperties(context);
                            break;
                    }
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        private static void GetFullDataAboutProperties(RealEstatesDbContext context)
        {
            Console.Write("Property count:");
            var count = int.Parse(Console.ReadLine());

            var service = new PropertiesService(context);
            var propertiesInfo = service.GetFullData(count);

            var serializer = new XmlSerializer(typeof(List<PropertyFullInfoDto>), new XmlRootAttribute("Properties"));

            var namespacesSettings = new XmlSerializerNamespaces();
            namespacesSettings.Add("", "");

            var textWriter = new StringWriter();
            serializer.Serialize(textWriter, propertiesInfo, namespacesSettings);

            Console.WriteLine(textWriter.ToString().TrimEnd());
        }

        private static void BulkTagsToProperties(RealEstatesDbContext context)
        {
            Console.WriteLine("Bulk operation started...");
            var propertyService = new PropertiesService(context);
            var service = new TagsService(context, propertyService);
            service.BulkTagToProperties();
            Console.WriteLine("Bulk operation finished successfully!");
        }

        private static void AddTag(RealEstatesDbContext context)
        {
            Console.Write("Tag name:");
            var name = Console.ReadLine();
            Console.Write("Importance (optional):");
            var parsed = int.TryParse(Console.ReadLine(), out var importance);

            var propertyService = new PropertiesService(context);
            var service = new TagsService(context, propertyService);

            var tagImportance = parsed ? importance : (int?)null;
            service.Add(name, tagImportance);
        }

        private static void AveragePricePerSquareMeter(RealEstatesDbContext dbContext)
        {
            var service = new PropertiesService(dbContext);
            Console.WriteLine($"Average price per square meter is {service.GetAveragePricePerSquareMeter():F2}€/m²");
        }

        private static void MostExpensiveDistricts(RealEstatesDbContext context)
        {
            Console.Write("Districts count:");
            var count = int.Parse(Console.ReadLine());

            var service = new DistrictsService(context);
            var districts = service.GetMostExpensiveDistricts(count);

            var serializer = new XmlSerializer(typeof(List<DistrictInfoDto>), new XmlRootAttribute("Districts"));

            var namespacesSettings = new XmlSerializerNamespaces();
            namespacesSettings.Add("", "");

            var textWriter = new StringWriter();
            serializer.Serialize(textWriter, districts, namespacesSettings);

            Console.WriteLine(textWriter.ToString().TrimEnd());
        }

        private static void PropertySearch(RealEstatesDbContext context)
        {
            Console.Write("Min price:");
            var minPrice = int.Parse(Console.ReadLine());
            Console.Write("Max price:");
            var maxPrice = int.Parse(Console.ReadLine());
            Console.Write("Min size:");
            var minSize = int.Parse(Console.ReadLine());
            Console.Write("Max size:");
            var maxSize = int.Parse(Console.ReadLine());

            var service = new PropertiesService(context);
            var properties = service.Search(minPrice, maxPrice, minSize, maxSize);

            var serializer = new XmlSerializer(typeof(List<PropertyInfoDto>), new XmlRootAttribute("Properties"));

            var namespacesSettings = new XmlSerializerNamespaces();
            namespacesSettings.Add("", "");

            var textWriter = new StringWriter();
            serializer.Serialize(textWriter, properties, namespacesSettings);

            Console.WriteLine(textWriter.ToString().TrimEnd());
        }
    }
}