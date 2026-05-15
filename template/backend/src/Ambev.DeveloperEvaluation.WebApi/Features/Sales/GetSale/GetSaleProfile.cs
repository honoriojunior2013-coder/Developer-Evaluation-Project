using Ambev.DeveloperEvaluation.Application.Sales.DTOs;
using Ambev.DeveloperEvaluation.WebAPI.Features.Sales.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public class GetSaleProfile : Profile
{
    public GetSaleProfile()
    {
        // Mapeia o DTO que vem da Application para a Resposta da API
        // Usamos o caminho completo para evitar qualquer ambiguidade
        CreateMap<SaleDto, GetSaleResponse>();
        
        // Também precisamos garantir que os itens internos da lista sejam mapeados
        CreateMap<SaleItemDto, GetSaleItemResponse>();
    }
}