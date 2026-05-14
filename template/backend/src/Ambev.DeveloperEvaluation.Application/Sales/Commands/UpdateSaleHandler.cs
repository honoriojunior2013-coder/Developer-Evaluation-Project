using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, Unit>
{
    private readonly ISaleRepository _repository;
    private readonly ILogger<UpdateSaleHandler> _logger;

    public UpdateSaleHandler(
        ISaleRepository repository,
        ILogger<UpdateSaleHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        UpdateSaleCommand request,
        CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sale {request.Id} not found");


        sale.Update(request.Customer, request.Branch);
        
        _repository.Update(sale);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Sale {SaleId} updated successfully", request.Id);

        return Unit.Value;
    }
}