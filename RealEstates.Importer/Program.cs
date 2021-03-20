using System;
using System.Collections.Generic;
using System.IO;
using RealEstates.Data;
using RealEstates.Services;
using System.Text.Json;

namespace RealEstates.Importer
{
    public class Program
    {
        public static void Main()
        {
            ImportJsonFile("imot.bg-houses-Sofia-raw-data-2021-03-18.json");
            Console.WriteLine();
            ImportJsonFile("imot.bg-raw-data-2021-03-18.json");
        }

        private static void ImportJsonFile(string fileName)
        {
            var context = new RealEstatesDbContext();
            var propertyService = new PropertiesService(context);

            var properties = JsonSerializer.Deserialize<IEnumerable<PropertyAsJson>>(
                File.ReadAllText(fileName));

            foreach (var jsonProp in properties)
            {
                propertyService.Add(jsonProp.District, jsonProp.Price, jsonProp.Floor, jsonProp.TotalFloors, jsonProp.Size,
                    jsonProp.YardSize, jsonProp.Year, jsonProp.Type, jsonProp.BuildingType);
                Console.Write(".");
            }
        }
    }
}