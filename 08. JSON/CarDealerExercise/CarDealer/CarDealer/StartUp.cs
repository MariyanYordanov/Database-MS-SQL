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

            string inputJson = File.ReadAllText("../../../Datasets/suppliers.json");
            string output = ImportSuppliers(dbContext, inputJson);
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

        private static bool IsAttributeValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);

            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult);

            return isValid;
        }
    }

}