using Ambev.DeveloperEvaluation.Application.Sales.DTOs;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands;

public record CreateSaleCommand(
    string Customer,
    string Branch,
    List<CreateSaleItemDto> Items
) : IRequest<CreateSaleResponse>;

public record CreateSaleItemDto(
    string ProductName,
    int Quantity,
    decimal UnitPrice
); 

public record CreateSaleResponse(Guid Id, string SaleNumber);