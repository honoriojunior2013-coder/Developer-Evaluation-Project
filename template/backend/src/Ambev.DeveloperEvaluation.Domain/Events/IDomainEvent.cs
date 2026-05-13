namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Interface para eventos de domínio, garantindo que todos os eventos tenham uma data de ocorrência.
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}