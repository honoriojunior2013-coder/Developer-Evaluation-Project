using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, Unit>
{
    private readonly ISaleRepository _repository;
    private readonly ILogger<CancelSaleHandler> _logger;

    public CancelSaleHandler(
        ISaleRepository repository,
        ILogger<CancelSaleHandler> logger)
    {
        _repository = repository;
        _logger = logger;
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

        _logger.LogInformation("Sale {SaleId} cancelled successfully", request.Id);

        return Unit.Value;
    }
}