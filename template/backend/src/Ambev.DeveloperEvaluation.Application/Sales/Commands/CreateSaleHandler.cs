using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResponse>
{
    private readonly ISaleRepository _repository;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly IMediator _mediator;
    private ISaleRepository object1;
    private ILogger<CreateSaleHandler> object2;

    public CreateSaleHandler(ISaleRepository object1, ILogger<CreateSaleHandler> object2)
    {
        this.object1 = object1;
        this.object2 = object2;
    }

    public CreateSaleHandler(ISaleRepository repository, ILogger<CreateSaleHandler> logger, IMediator mediator)
    {
        _repository = repository;
        _logger = logger;
        _mediator = mediator;
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

        foreach (var domainEvent in sale.DomainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
        sale.ClearDomainEvents();

        return new CreateSaleResponse(sale.Id, sale.SaleNumber);
    }
}