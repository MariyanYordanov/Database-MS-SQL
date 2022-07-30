using AutoMapper;
using ProductShop.DTOs.CategoryProduct;
using ProductShop.DTOs.Category;
using ProductShop.DTOs.Product;
using ProductShop.DTOs.User;
using ProductShop.Models;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //             source         destination
            this.CreateMap<ImportUserDto, User>();
            this.CreateMap<ImportProductDto, Product>();
            this.CreateMap<ImportCategoryDto, Category>();
            this.CreateMap<ImportCategoryProductDto, CategoryProduct>();

            //             source   destination
            this.CreateMap<Product, ExportProductsInRangeDto>()
                .ForMember(destinationMember => destinationMember.SellerFullName,
                           memberOption => memberOption.MapFrom(
                           sourceMember => $"{sourceMember.Seller.FirstName} " +
                                           $"{sourceMember.Seller.LastName}"));

            // For 6 task where two DTO's are Nested
            // Child DTO 
            this.CreateMap<Product, ExportUserSoldProductsDto>()
                .ForMember(d => d.BuyerFirstName, mo => mo.MapFrom(s => s.Buyer.FirstName))
                .ForMember(d => d.BuyerLastName, mo => mo.MapFrom(s => s.Buyer.LastName));

            // Parent DTO 
            this.CreateMap<User, ExportUserWithSoldProductsDto>()
                .ForMember(d => d.SoldProducts, mo => mo.MapFrom(s => s.ProductsSold.Where(p => p.BuyerId.HasValue)));

            //this.CreateMap<Category, ExportAllCaregory>()
            //    .ForMember(d => d.ProductsCount, mo => mo.MapFrom(s => s.CategoryProducts.Count()))
            //    .ForMember(d => d.AveragePrice, mo => mo.MapFrom(s => s.CategoryProducts.Sum(cp => cp.Product.Price) / s.CategoryProducts.Count()))
            //    .ForMember(d => d.TotalRevenue, mo => mo.MapFrom(s => s.CategoryProducts.Sum(cp => cp.Product.Price)));
        }
    }
}
