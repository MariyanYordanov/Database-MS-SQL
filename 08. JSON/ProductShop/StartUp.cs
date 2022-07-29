using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.User;
using ProductShop.Models;
using System.ComponentModel.DataAnnotations;
using ProductShop.DTOs.Product;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile<ProductShopProfile>();
            });

            ProductShopContext dbContext = new ProductShopContext();

            string inputJson = File.ReadAllText("../../../Datasets/products.json");

            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            //Console.WriteLine("Datababe copy was created!");

            string output = ImportProducts(dbContext, inputJson);
            Console.WriteLine(output);
        }

        // Query 1.Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            ImportUserDto[] userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);

            ICollection<User> validUsers = new List<User>();

            foreach (ImportUserDto userDto in userDtos)
            {
                if (!IsAttributeValid(userDto))
                {
                    continue;
                }

                User user = Mapper.Map<User>(userDto);
                validUsers.Add(user);
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return $"Successfully imported {validUsers.Count}";
        }

        // Query 2.Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            ImportProductDto[] productDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);
            ICollection<Product> validProducts = new List<Product>();
            foreach (ImportProductDto productDto in productDtos)
            {
                if (!IsAttributeValid(productDto))
                {
                    continue;
                }

                Product product = Mapper.Map<Product>(productDto);
                validProducts.Add(product);
            }

            context.Products.AddRange(validProducts);
            context.SaveChanges();

            return $"Successfully imported {validProducts.Count}";
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