using Ambev.DeveloperEvaluation.Application.Sales.DTOs;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries;

public class GetSalesHandler : IRequestHandler<GetSalesQuery, PagedResult<SaleDto>>
{
    private readonly ISaleRepository _repository;

    public GetSalesHandler(ISaleRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<SaleDto>> Handle(
        GetSalesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _repository.GetAll();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.Customer))
            query = query.Where(s => s.Customer.Contains(request.Customer));

        if (request.IsCancelled.HasValue)
            query = query.Where(s => s.IsCancelled == request.IsCancelled.Value);

        // Get total count
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination
        var sales = await query
            .OrderByDescending(s => s.SaleDate)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var items = sales.Select(s => new SaleDto(
            s.Id,
            s.SaleNumber,
            s.SaleDate,
            s.Customer,
            s.Branch,
            s.TotalAmount,
            s.IsCancelled,
            s.Items.Select(i => new SaleItemDto(
                i.Id,
                i.ProductName,
                i.Quantity,
                i.UnitPrice,
                i.Discount,
                i.TotalAmount,
                i.IsCancelled
            )).ToList()
        )).ToList();

        return new PagedResult<SaleDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}