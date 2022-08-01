namespace ProductShop
{
    using System;
    using System.IO;
    using System.Linq;
    using CarDealer.XmlHelper;
    using ProductShop.Data;
    using ProductShop.Dtos.Import;
    using ProductShop.Models;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            ProductShopContext dbContext = new ProductShopContext();

            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            string inputXml = File.ReadAllText("../../../Datasets/products.xml");
            string result = ImportProducts(dbContext, inputXml);
            Console.WriteLine(result);
        }

        // Query 1. Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var usersDto = XmlConverter.Deserializer<ImportUsersDto>(inputXml, "Users");

            var users = usersDto
                .Select(u => new User
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                })
                .ToArray();

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        // Query 2. Import Products
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var productsDto = XmlConverter.Deserializer<ImportProductsDto>(inputXml, "Products");

            var products = productsDto
                .Select(u => new Product
                {
                    Name = u.Name,
                    Price = u.Price,
                    SellerId = u.SellerId,
                    BuyerId = u.BuyerId,
                })
                .ToArray();

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }
    }
}