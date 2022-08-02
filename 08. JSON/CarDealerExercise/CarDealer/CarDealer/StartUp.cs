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
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            Console.WriteLine("Datababe copy was created!");

            string inputJson = File.ReadAllText("../../../Datasets/cars.json");
            string output = ImportCars(dbContext, inputJson);
            Console.WriteLine(output);

            //string json = GetUsersWithProducts(dbContext);
            //File.WriteAllText(filePath, json);
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




        private static bool IsAttributeValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);

            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult);

            return isValid;
        }
    }

}