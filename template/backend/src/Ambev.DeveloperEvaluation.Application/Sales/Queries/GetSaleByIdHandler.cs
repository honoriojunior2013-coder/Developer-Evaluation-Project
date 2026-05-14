using Ambev.DeveloperEvaluation.Application.Sales.DTOs;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries;

public class GetSaleByIdHandler : IRequestHandler<GetSaleByIdQuery, SaleDto?>
{
    private readonly ISaleRepository _repository;

    public GetSaleByIdHandler(ISaleRepository repository)
    {
        _repository = repository;
    }

    public async Task<SaleDto?> Handle(
        GetSaleByIdQuery request,
        CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.Id, cancellationToken);
        
        if (sale == null)
            return null;

        return new SaleDto(
            sale.Id,
            sale.SaleNumber,
            sale.SaleDate,
            sale.Customer,
            sale.Branch,
            sale.TotalAmount,
            sale.IsCancelled,
            sale.Items.Select(i => new SaleItemDto(
                i.Id,
                i.ProductName,
                i.Quantity,
                i.UnitPrice,
                i.Discount,
                i.TotalAmount,
                i.IsCancelled
            )).ToList()
        );
    }
}