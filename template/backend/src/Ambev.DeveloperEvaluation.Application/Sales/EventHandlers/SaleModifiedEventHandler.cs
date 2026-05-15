using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

public class SaleModifiedEventHandler : INotificationHandler<SaleModifiedEvent>
{
    private readonly ILogger<SaleModifiedEventHandler> _logger;
    private readonly AsyncRetryPolicy _retryPolicy;

    public SaleModifiedEventHandler(ILogger<SaleModifiedEventHandler> logger)
    {
        _logger = logger;
        _retryPolicy = Policy.Handle<Exception>()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
    }

    public async Task Handle(SaleModifiedEvent notification, CancellationToken cancellationToken)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Processing SaleModifiedEvent for Sale {SaleId}", notification.SaleId);
            await Task.Delay(100, cancellationToken);
            _logger.LogInformation("Successfully updated caches/logs for Sale {SaleId}", notification.SaleId);
        });
    }
}