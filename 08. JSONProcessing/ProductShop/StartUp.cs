using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.User;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private static IMapper mapper;

        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile<ProductShopProfile>();
            });

            ProductShopContext dbContext = new ProductShopContext();

            string inputJson = File.ReadAllText("../../../Datasets/users.json");

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            Console.WriteLine("Datababe copy was created!");

            string output = ImportUsers(dbContext, inputJson);
            Console.WriteLine(output);
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            ImportUserDto[] userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);

            ICollection<User> users = new List<User>();

            foreach (var userDto in userDtos)
            {
                User user = Mapper.Map<User>(userDto);
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }
    }
}