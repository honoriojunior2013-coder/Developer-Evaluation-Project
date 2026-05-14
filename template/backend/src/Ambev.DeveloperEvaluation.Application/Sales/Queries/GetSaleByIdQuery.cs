using Ambev.DeveloperEvaluation.Application.Sales.DTOs;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries;

public record GetSaleByIdQuery(Guid Id) : IRequest<SaleDto?>;