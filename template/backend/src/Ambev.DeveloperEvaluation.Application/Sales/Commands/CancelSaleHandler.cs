using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, Unit>
{
    private readonly ISaleRepository _repository;
    private readonly ILogger<CancelSaleHandler> _logger;
    private readonly IMediator _mediator;

    public CancelSaleHandler(
        ISaleRepository repository,
        ILogger<CancelSaleHandler> logger,
        IMediator mediator)
    {
        _repository = repository;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(
        CancelSaleCommand request,
        CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sale {request.Id} not found");

        sale.Cancel();
        
        _repository.Update(sale);
        await _repository.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in sale.DomainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        sale.ClearDomainEvents();
        
        _logger.LogInformation("Sale {SaleId} cancelled successfully", request.Id);

        return Unit.Value;
    }
}