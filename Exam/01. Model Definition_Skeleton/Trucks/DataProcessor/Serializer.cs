namespace Trucks.DataProcessor
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using Data;
    using Newtonsoft.Json;
    using Trucks.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;
    using Trucks.XmlConverter;
    using Trucks.Data.Models.Enums;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            var despatchers = context.Despatchers
                .Where(d => d.Trucks.Any())
                .ToArray()
                .Select(d => new ExportDespatchersDto()
                {
                    DespatcherName = d.Name,
                    TrucksCount = d.Trucks.Count(),
                    Trucks = d.Trucks
                        .Select(t => new ExportTrucksDto()
                        {
                            RegistrationNumber = t.RegistrationNumber,
                            Make = t.MakeType.ToString(),
                        })
                        .OrderBy(t => t.RegistrationNumber)
                        .ToArray(),
                })
                .OrderByDescending(d => d.TrucksCount)
                .ThenBy(d => d.DespatcherName)
                .ToArray();

            return XmlConverter.Serialize(despatchers, "Despatchers");
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clients = context.Clients
                .Where(c => c.ClientsTrucks.Any(t => t.Truck.TankCapacity >= capacity))
                .ToArray()
                .Select(c => new 
                {
                    Name = c.Name,
                    Trucks = c.ClientsTrucks
                        .Where(t => t.Truck.TankCapacity >= capacity)
                        .Select(t => new
                        {
                            TruckRegistrationNumber = t.Truck.RegistrationNumber,
                            VinNumber = t.Truck.VinNumber,
                            TankCapacity = t.Truck.TankCapacity,
                            CargoCapacity = t.Truck.CargoCapacity,
                            CategoryType = t.Truck.CategoryType.ToString(),
                            MakeType = (MakeType)t.Truck.MakeType,
                        })
                        .OrderBy(t => t.MakeType)
                        .ThenByDescending(t => t.CargoCapacity)
                        .ToArray(),
                })
                .OrderByDescending(c => c.Trucks.Count())
                .ThenBy(c => c.Name)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(clients,Formatting.Indented);
        }
    }
}
