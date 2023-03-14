using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import.CategoryProduct;

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
        // User
        this.CreateMap<ImportUserDto, User>();

        // Product
        this.CreateMap<ImportProductDto, Product>();
        this.CreateMap<Product, ExportProductInRangeDto>()
            .ForMember(d => d.ProductName,
                opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.ProductPrice,
                opt => opt.MapFrom(s => s.Price))
            .ForMember(d => d.SellerName,
                opt => opt.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));

        // Category
        this.CreateMap<ImportCategoryDto, Category>();

        // CategpryProduct
        this.CreateMap<ImportCategoryProduct, CategoryProduct>();
    }
}