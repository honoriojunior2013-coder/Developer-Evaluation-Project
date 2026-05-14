using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResponse>
{
    private readonly ISaleRepository _repository;
    private readonly ILogger<CreateSaleHandler> _logger;

    public CreateSaleHandler(
        ISaleRepository repository,
        ILogger<CreateSaleHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<CreateSaleResponse> Handle(
        CreateSaleCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating sale for customer: {Customer}", request.Customer);

        // Map DTOs to domain entities
        var items = request.Items.Select(dto =>
            SaleItem.Create(
                dto.ProductName,
                dto.Quantity,
                dto.UnitPrice,
                dto.Discount
            )).ToList();

        // Create sale aggregate
        var sale = Sale.Create(
            request.Customer,
            request.Branch,
            items
        );

        // Persist
        await _repository.AddAsync(sale, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Sale created successfully. ID: {SaleId}, Number: {SaleNumber}",
            sale.Id, sale.SaleNumber);

        return new CreateSaleResponse(sale.Id, sale.SaleNumber);
    }
}