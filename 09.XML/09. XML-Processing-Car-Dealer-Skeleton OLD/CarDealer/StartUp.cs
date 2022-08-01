using CarDealer.Data;
using System.IO;
using System;
using System.Xml.Serialization;
using CarDealer.Dtos.Import;
using System.Linq;
using CarDealer.Models;
using CarDealer.XmlHelper;
using System.Collections.Generic;
using CarDealer.Dtos.Export;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext carDealerContext = new CarDealerContext();

            //carDealerContext.Database.EnsureDeleted();
            //carDealerContext.Database.EnsureCreated();
            //Console.WriteLine("Database is ready to work!");

            //string xml = File.ReadAllText("../../../Datasets/sales.xml");
            //string result = ImportSales(carDealerContext, xml);
            //Console.WriteLine(result);

            string inputXml = GetLocalSuppliers(carDealerContext);
            File.WriteAllText("../../../Outputs/local-suppliers.xml", inputXml);
            Console.WriteLine("The file was created!");
        }

        // Query 9. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            const string root = "Suppliers";

            var suppliersDto = XmlConverter.Deserializer<SupplierDto>(inputXml, root);

            Supplier[] suppliers = suppliersDto
                .Select(dto => new Supplier
                {
                    Name = dto.Name,
                    IsImporter = dto.IsImporter,
                })
                .ToArray();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }

        // Query 10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            const string root = "Parts";

            var partsDtos = XmlConverter.Deserializer<PartsDto>(inputXml, root);

            var validId = context.Suppliers.Select(s => s.Id).ToArray();

            var parts = partsDtos
                .Where(p => validId.Contains(p.SupplierId))
                .Select(dto => new Part
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    Quantity = dto.Quantity,
                    SupplierId = dto.SupplierId,
                })
               .ToArray();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}";
        }

        // Query 11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var carsModel = XmlConverter.Deserializer<CarsDto>(inputXml, "Cars");

            var cars = new List<Car>();

            foreach (var car in carsModel)
            {
                var partsIds = car.Parts
                    .Select(x => x.PartId)
                    .Distinct()
                    .Where(id => context.Parts.Any(x => x.Id == id))
                    .ToArray();

                var currCar = new Car()
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TraveledDistance,
                    PartCars = partsIds.Select(id => new PartCar()
                    {
                        PartId = id,
                    })
                    .ToArray()
                };

                cars.Add(currCar);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        // Query 12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var customersModel = XmlConverter.Deserializer<CustomersDto>(inputXml, "Customers");

            var customers = customersModel
                .Select(c => new Customer
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    IsYoungDriver = c.IsYoungDriver,
                })
                .ToArray();

            context.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}";
        }

        // Query 13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var salesDto = XmlConverter.Deserializer<SalesDto>(inputXml, "Sales");

            var carIds = context.Cars.Select(c => c.Id).ToArray();

            var sales = salesDto
                .Where(s => carIds.Contains(s.CarId))
                .Select(s => new Sale
                {
                    CarId = s.CarId,
                    CustomerId = s.CustomerId,
                    Discount = s.Discount,

                })
                .ToArray();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}";
        }

        // Query 14. Export Cars With Distance
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.TravelledDistance > 2_000_000)
                .Select(c => new CarWithDistanceDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                })
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToArray();

            return XmlConverter.Serialize<CarWithDistanceDto>(cars, "cars");
        }

        // Query 15. Export Cars from make BMW
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "BMW")
                .Select(c => new CarFromMakeBmwDto
                {
                    Id = c.Id,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            return XmlConverter.Serialize<CarFromMakeBmwDto>(cars,"cars");
        }

        // Query 16. Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => !s.IsImporter)
                .Select(s => new LocalSuppliersDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count(),
                })
                .ToArray();
            return XmlConverter.Serialize<LocalSuppliersDto>(suppliers, "suppliers");
        }
    }
}