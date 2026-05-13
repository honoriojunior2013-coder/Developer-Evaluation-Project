namespace Ambev.DeveloperEvaluation.Domain.Events;

public record SaleItemCancelledEvent(
    Guid SaleId,
    Guid ItemId,
    DateTime OccurredOn
) : IDomainEvent;