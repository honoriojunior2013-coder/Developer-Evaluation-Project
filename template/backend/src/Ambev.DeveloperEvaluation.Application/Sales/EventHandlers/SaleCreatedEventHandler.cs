using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Ambev.DeveloperEvaluation.Application.Sales.EventHandlers;

public class SaleCreatedEventHandler : INotificationHandler<SaleCreatedEvent>
{
    private readonly ILogger<SaleCreatedEventHandler> _logger;
    private readonly AsyncRetryPolicy _retryPolicy;

    public SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger)
    {
        _logger = logger;
        _retryPolicy = Policy.Handle<Exception>()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
            (exception, timeSpan, retryCount, context) =>
            {
                _logger.LogWarning("Retry {RetryCount} for SaleCreatedEvent after {Delay}s due to: {Exception}",
                    retryCount, timeSpan.TotalSeconds, exception.Message);
            });
    }

    public async Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Processing SaleCreatedEvent for Sale {SaleId} at {Timestamp}",
                notification.SaleId, notification.OccurredOn);
            
            await Task.Delay(100, cancellationToken); // Simula integração externa
            
            _logger.LogInformation("Successfully processed SaleCreatedEvent for Sale {SaleId}",
                notification.SaleId);
        });
    }
}