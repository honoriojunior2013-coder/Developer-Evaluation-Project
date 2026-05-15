using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands;

public record DeleteSaleCommand(Guid Id) : IRequest<bool>;