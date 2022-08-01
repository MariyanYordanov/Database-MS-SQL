namespace ProductShop
{
    using System.IO;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using CarDealer.XmlHelper;
    using ProductShop.Data;
    using ProductShop.Dtos.Export;
    using ProductShop.Dtos.Import;
    using ProductShop.Models;

    public class StartUp
    {
        static IMapper mapper;

        
        public static void Main(string[] args)
        {
            ProductShopContext dbContext = new ProductShopContext();
            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            //string inputXml = File.ReadAllText("../../../Datasets/categories-products.xml");

            string result = GetProductsInRange(dbContext);
            File.WriteAllText("../../../Outputs/products-in-range.xml", result);
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

        // Query 3. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var categoriesDto = XmlConverter.Deserializer<ImportCategoryDto>(inputXml,"Categories");

            var categories = categoriesDto.Select(c => new Category { Name = c.Name }).ToArray();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        // Query 4. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var categoriesProducts = XmlConverter.Deserializer<ImportCategoriesProductsDto>(inputXml, "CategoryProducts");

            var categoryProducts = categoriesProducts
                .Where(x => context.Products
                    .Any(y => y.Id == x.ProductId) && context.Categories.
                        Any(y => y.Id == x.CategoryId))
                .Select(cp => new CategoryProduct
                {
                    CategoryId = cp.CategoryId,
                    ProductId = cp.ProductId,
                })
                .ToArray();

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Length}";
        }

        // Query 5. Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            ExportProductsInRangeDto[] productsInRange = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new ExportProductsInRangeDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    FullName = $"{p.Buyer.FirstName} {p.Buyer.LastName}" 
                })
                .OrderBy(p => p.Price)
                .Take(10)
                .ToArray();

            return XmlConverter.Serialize<ExportProductsInRangeDto>(productsInRange, "Products");
        }

    }
}