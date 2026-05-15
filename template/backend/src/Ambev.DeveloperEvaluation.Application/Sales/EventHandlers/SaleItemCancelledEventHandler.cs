using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

public class SaleItemCancelledEventHandler : INotificationHandler<SaleItemCancelledEvent>
{
    private readonly ILogger<SaleItemCancelledEventHandler> _logger;
    private readonly AsyncRetryPolicy _retryPolicy;

    public SaleItemCancelledEventHandler(ILogger<SaleItemCancelledEventHandler> logger)
    {
        _logger = logger;
        _retryPolicy = Policy.Handle<Exception>()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
    }

    public async Task Handle(SaleItemCancelledEvent notification, CancellationToken cancellationToken)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            _logger.LogWarning("Processing SaleItemCancelledEvent: Item {ItemId} from Sale {SaleId}", 
                notification.ItemId, notification.SaleId);
            
            await Task.Delay(100, cancellationToken);
            
            _logger.LogInformation("Successfully recalculated totals for Sale {SaleId} after item cancellation", 
                notification.SaleId);
        });
    }
}