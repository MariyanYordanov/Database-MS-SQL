using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO.Import;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile<CarDealerProfile>();
            });

            CarDealerContext dbContext = new CarDealerContext();
            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();
            //Console.WriteLine("Datababe copy was created!");

            //string inputJson = File.ReadAllText("../../../Datasets/sales.json");
            //string output = ImportSales(dbContext, inputJson);
            //Console.WriteLine(output);

            string json = GetTotalSalesByCustomer(dbContext);
            File.WriteAllText("../../../Outputs/customers-total-sales.json", json);
        }


        // Query 9. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            SuppliersImportDto[] suppliersDto = JsonConvert.DeserializeObject<SuppliersImportDto[]>(inputJson);
            ICollection<Supplier> validSuppliers = new List<Supplier>();
            foreach (var sDto in suppliersDto)
            {
                if (!IsAttributeValid(sDto))
                {
                    continue;
                }

                Supplier supplier = Mapper.Map<Supplier>(sDto);
                validSuppliers.Add(supplier);
            }

            context.Suppliers.AddRange(validSuppliers);
            context.SaveChanges();

            return $"Successfully imported {validSuppliers.Count}.";
        }

        // Query 10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            PartsImportDto[] partsDtos = JsonConvert.DeserializeObject<PartsImportDto[]>(inputJson);
            ICollection<Part> parts = new List<Part>();
            foreach (var pDtos in partsDtos)
            {
                if (!IsAttributeValid(pDtos))
                {
                    continue;
                }

                Part part = Mapper.Map<Part>(pDtos);
                if ( !context.Suppliers.Select(x => x.Id).Contains(part.SupplierId))
                {
                    continue;
                }

                parts.Add(part);
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}.";
        }

        // Query 11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            CarsImportDto[] carsDtos = JsonConvert.DeserializeObject<CarsImportDto[]>(inputJson);
            List<Car> cars = new List<Car>();
            foreach (var dtoCar in carsDtos)
            {
                Car car = new Car
                {
                    Make = dtoCar.Make,
                    Model = dtoCar.Model,
                    TravelledDistance = dtoCar.TravelledDistance,
                };

                foreach (int partId in dtoCar.PartsId.Distinct())
                {
                    car.PartCars.Add(new PartCar
                    {
                        PartId = partId
                    });
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count()}.";
        }

        // Query 12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            CustomersImportDto[] customersDtos = JsonConvert.DeserializeObject<CustomersImportDto[]>(inputJson);
            ICollection<Customer> customers = new List<Customer>();
            foreach (var cDto in customersDtos)
            {
                if (!IsAttributeValid(cDto))
                {
                    continue;
                }

                Customer customer = new Customer
                {
                    Name = cDto.Name,
                    BirthDate = cDto.BirthDate,
                    IsYoungDriver = cDto.IsYoungDriver,
                };

                customers.Add(customer);
            }

            context.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        // Query 13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            SalesImportDto[] salesDtos = JsonConvert.DeserializeObject<SalesImportDto[]>(inputJson);
            ICollection<Sale> sales = new List<Sale>();
            foreach (var sDto in salesDtos)
            {
                if (!IsAttributeValid(sDto))
                {
                    continue;
                }

                Sale sale = new Sale
                {
                    CarId = sDto.CarId,
                    CustomerId = sDto.CustomerId,
                    Discount = sDto.Discount,
                };

                sales.Add(sale);
            }

            context.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

        // Query 14. Export Ordered Customers
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .Select(c => new 
                {
                    c.Name,
                    c.BirthDate,
                    c.IsYoungDriver,
                })
                .OrderBy(c => c.BirthDate)
                .ThenByDescending(c => c.IsYoungDriver)
                .ToArray();

            var settings = new JsonSerializerSettings { DateFormatString = "dd/MM/yyyy" };

            return JsonConvert.SerializeObject(customers, Formatting.Indented, settings);
        }

        // Query 15. Export Cars from Make Toyota
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toyotas = context.Cars
                .Where(c => c.Make == "Toyota")
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TravelledDistance,
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            return JsonConvert.SerializeObject(toyotas, Formatting.Indented);
        }

        // Query 16. Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => !s.IsImporter)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count,
                })
                .ToArray();

            return JsonConvert.SerializeObject(suppliers, Formatting.Indented);
        }

        // Query 17. Export Cars with Their List of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TravelledDistance,
                    },
                    parts = c.PartCars
                        .Select(pc => new 
                        {
                            Name = pc.Part.Name,
                            Price = pc.Part.Price.ToString("f2"),
                        })
                        .ToArray(),
                })
                .ToArray();

            return JsonConvert.SerializeObject(cars, Formatting.Indented);
        }

        // Query 18. Export Total Sales by Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count > 0)
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count,
                    spentMoney = Decimal.Parse(c.Sales
                        .Select(s => s.Car)
                        .SelectMany(s => s.PartCars)
                        .Sum(s => s.Part.Price).ToString("f2"))
                })
                .OrderByDescending(c => c.spentMoney)
                .ThenByDescending(c => c.boughtCars)
                .ToArray();

            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }


        private static bool IsAttributeValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);

            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult);

            return isValid;
        }
    }

}