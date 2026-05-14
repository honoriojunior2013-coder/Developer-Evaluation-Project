using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands;

public record UpdateSaleCommand(
    Guid Id,
    string Customer,
    string Branch
) : IRequest<Unit>;

