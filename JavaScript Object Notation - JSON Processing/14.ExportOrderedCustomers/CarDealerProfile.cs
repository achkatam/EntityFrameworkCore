namespace CarDealer;

using DTOs.Import;
using Models;
using AutoMapper;

public class CarDealerProfile : Profile
{
    public CarDealerProfile()
    {
        this.CreateMap<ImportSupplierDTO, Supplier>();

        this.CreateMap<ImportPartDto, Part>();
    }
}