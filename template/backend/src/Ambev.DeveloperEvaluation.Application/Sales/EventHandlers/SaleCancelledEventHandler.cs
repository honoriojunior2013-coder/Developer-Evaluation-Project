using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

public class SaleCancelledEventHandler : INotificationHandler<SaleCancelledEvent>
{
    private readonly ILogger<SaleCancelledEventHandler> _logger;
    private readonly AsyncRetryPolicy _retryPolicy;

    public SaleCancelledEventHandler(ILogger<SaleCancelledEventHandler> logger)
    {
        _logger = logger;
        _retryPolicy = Policy.Handle<Exception>()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
    }

    public async Task Handle(SaleCancelledEvent notification, CancellationToken cancellationToken)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            _logger.LogWarning("Processing SaleCancelledEvent for Sale {SaleId}", notification.SaleId);
            // Aqui você dispararia estornos ou notificações de cancelamento
            await Task.Delay(100, cancellationToken);
            _logger.LogInformation("Successfully processed SaleCancelledEvent for Sale {SaleId}", notification.SaleId);
        });
    }
}