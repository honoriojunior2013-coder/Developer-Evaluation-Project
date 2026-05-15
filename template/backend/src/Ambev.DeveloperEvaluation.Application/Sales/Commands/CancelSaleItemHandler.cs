using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands;

public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, Unit>
{
    private readonly ISaleRepository _repository;
    private readonly IMediator _mediator;
    private readonly ILogger<CancelSaleItemHandler> _logger;

    public CancelSaleItemHandler(
        ISaleRepository repository,
        IMediator mediator,
        ILogger<CancelSaleItemHandler> logger)
    {
        _repository = repository;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        CancelSaleItemCommand request,
        CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.SaleId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Sale), request.SaleId);

        sale.CancelItem(request.ItemId);

        _repository.Update(sale);
        await _repository.SaveChangesAsync(cancellationToken);

        // Publish domain events
        foreach (var domainEvent in sale.DomainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        sale.ClearDomainEvents();

        _logger.LogInformation(
            "Sale item {ItemId} cancelled in sale {SaleId}. New total: {TotalAmount}",
            request.ItemId, request.SaleId, sale.TotalAmount);

        return Unit.Value;
    }
}