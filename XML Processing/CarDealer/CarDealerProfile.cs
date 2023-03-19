using CarDealer.DTOs.Import.Customer;

namespace CarDealer;

using Models;
using DTOs.Import;
using AutoMapper;

public class CarDealerProfile : Profile
{
    public CarDealerProfile()
    {
        // Car
        this.CreateMap<ImportCarDto, Car>()
            .ForSourceMember(s => s.Parts, opt => opt.DoNotValidate());

        // PartCar
        this.CreateMap<ImportPartCarDto, PartCar>();

        // Customer
        this.CreateMap<ImportCustomerDto, Customer>();
    }
}