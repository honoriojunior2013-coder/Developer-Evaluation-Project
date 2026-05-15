namespace Ambev.DeveloperEvaluation.Domain.Events;

public record SaleModifiedEvent(Guid SaleId, DateTime OccurredOn) : IDomainEvent;