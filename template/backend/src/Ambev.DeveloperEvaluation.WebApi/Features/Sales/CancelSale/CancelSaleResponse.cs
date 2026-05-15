namespace Ambev.DeveloperEvaluation.WebAPI.Features.Sales.CancelSale;

public class CancelSaleResponse
{
    public Guid Id { get; set; }
    public bool IsCancelled { get; set; }
    public DateTime CancelledAt { get; set; }
}