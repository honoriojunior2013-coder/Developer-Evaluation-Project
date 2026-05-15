using Ambev.DeveloperEvaluation.Application.Sales.Commands;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebAPI.Features.Sales.CreateSale;

public class CreateSaleProfile : Profile
{
    public CreateSaleProfile()
    {
        // mapping correction
        CreateMap<CreateSaleRequest, Application.Sales.Commands.CreateSaleCommand>();
        CreateMap<CreateSaleItemRequest, Application.Sales.Commands.CreateSaleItemDto>();
        CreateMap<Application.Sales.Commands.CreateSaleResponse, CreateSaleResponse>();
    }
}