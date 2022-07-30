using AutoMapper;
using ProductShop.DTOs.CategoryProduct;
using ProductShop.DTOs.Category;
using ProductShop.DTOs.Product;
using ProductShop.DTOs.User;
using ProductShop.Models;

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
        }
    }
}
