namespace Ambev.DeveloperEvaluation.WebAPI.Features.Sales.UpdateSale;

public class UpdateSaleResponse
{
    public Guid Id { get; set; }
    public string Customer { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}