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
            this.CreateMap<Product, ExportUserSoldProductsDto>();
            // Parent DTO 
            this.CreateMap<User, ExportUserWithSoldProductsDto>()
                .ForMember(d => d.SoldProducts, mo => mo.MapFrom(s => s.ProductsSold.Where(p => p.BuyerId.HasValue)));

            this.CreateMap<Product, ExportSoldProductShortInfoDto>();
            this.CreateMap<User, ExportSoldProductsFullInfo>()
                .ForMember(d => d.Products, mo => mo.MapFrom(s => s.ProductsSold.Where(p => p.BuyerId.HasValue)));
            this.CreateMap<User, ExportUsersWithFullProductInfoDto>()
                .ForMember(d => d.SoldProducts, mo => mo.MapFrom(s => s));
        }
    }
}
