using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResponse>
{
    private readonly ISaleRepository _repository;
    private readonly ILogger<CreateSaleHandler> _logger;

    public CreateSaleHandler(ISaleRepository repository, ILogger<CreateSaleHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<CreateSaleResponse> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing new sale for customer {Customer}", request.Customer);

        var items = request.Items.Select(dto => {
            // Business Rule: Apply discount tiers
            decimal discount = 0;
            if (dto.Quantity >= 10) discount = 20;      // 10-20 items: 20%
            else if (dto.Quantity >= 4) discount = 10; // 4-9 items: 10%
            
            return SaleItem.Create(dto.ProductName, dto.Quantity, dto.UnitPrice, discount);
        }).ToList();

        var sale = Sale.Create(request.Customer, request.Branch, items);

        await _repository.AddAsync(sale, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new CreateSaleResponse(sale.Id, sale.SaleNumber);
    }
}