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

            string inputXml = GetSalesWithAppliedDiscount(carDealerContext);
            File.WriteAllText("../../../Outputs/sales-discounts.xml", inputXml);
            Console.WriteLine("The file was created!");
        }

        // Query 9. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            const string root = "Suppliers";

            var suppliersDto = XmlConverter.Deserializer<SupplierImportDto>(inputXml, root);

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

            var partsDtos = XmlConverter.Deserializer<PartsImportDto>(inputXml, root);

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
            var carsModel = XmlConverter.Deserializer<CarsImportDto>(inputXml, "Cars");

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
            var customersModel = XmlConverter.Deserializer<CustomersImportDto>(inputXml, "Customers");

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
            var salesDto = XmlConverter.Deserializer<SalesImportDto>(inputXml, "Sales");

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
                .Select(c => new CarWithDistanceExportDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                })
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToArray();

            return XmlConverter.Serialize<CarWithDistanceExportDto>(cars, "cars");
        }

        // Query 15. Export Cars from make BMW
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "BMW")
                .Select(c => new CarFromMakeBmwExportDto
                {
                    Id = c.Id,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            return XmlConverter.Serialize<CarFromMakeBmwExportDto>(cars,"cars");
        }

        // Query 16. Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => !s.IsImporter)
                .Select(s => new LocalSuppliersExportDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count(),
                })
                .ToArray();
            return XmlConverter.Serialize<LocalSuppliersExportDto>(suppliers, "suppliers");
        }

        // Query 17. Export Cars with Their List of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new CarsListPartsExportDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TravelledDistance,
                    PartsList = c.PartCars
                        .Select(p => new CarListPartsNestedExportDto
                        {
                            Name = p.Part.Name,
                            Price = p.Part.Price,
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                })
                .OrderByDescending(c => c.TraveledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            return XmlConverter.Serialize<CarsListPartsExportDto>(cars, "cars");
        }

        // Query 18. Export Total Sales by Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new TotalSalesExportDto
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count(),
                    SpentMoney = c.Sales
                        .Select(c => c.Car)
                        .SelectMany(c => c.PartCars)
                        .Sum(c => c.Part.Price)
                })
                .OrderByDescending(c => c.SpentMoney)
                .ToArray();

            return XmlConverter.Serialize<TotalSalesExportDto>(customers, "customers");
        }

        // Query 19. Export Sales with Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new SalesDiscountsExportDto
                {
                    CarModel = new SalesDiscountCarAttributeExportDto
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance,
                    },

                    Discount = s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price),
                    PriceWithDiscount = s.Car.PartCars
                        .Sum(pc => pc.Part.Price) * (1 - s.Discount / 100)
                })
                .ToArray();

            return XmlConverter.Serialize<SalesDiscountsExportDto>(sales, "sales");
        }
    }
}