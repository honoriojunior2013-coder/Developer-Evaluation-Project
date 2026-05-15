using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands;

public record CancelSaleItemCommand(Guid SaleId, Guid ItemId) : IRequest<Unit>;