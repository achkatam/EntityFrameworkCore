namespace ProductShop;

using DTOs.Import.Category;
using DTOs.Import.Product;
using DTOs.Import.User;
using Models;
using AutoMapper;

public class ProductShopProfile : Profile
{
    public ProductShopProfile()
    {
        this.CreateMap<ImportUserDto, User>();
        this.CreateMap<ImportProductDto, Product>();
        this.CreateMap<ImportCategoryDto, Category>();
    }
}