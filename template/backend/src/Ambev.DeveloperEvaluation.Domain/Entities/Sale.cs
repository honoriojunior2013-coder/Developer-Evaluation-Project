using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Aggregate root for sale operations
/// </summary>
public class Sale : BaseEntity
{
    public string SaleNumber { get; private set; } = string.Empty;
    public DateTime SaleDate { get; private set; }
    public string Customer { get; private set; } = string.Empty;
    public decimal TotalAmount { get; private set; }
    public string Branch { get; private set; } = string.Empty;
    public bool IsCancelled { get; private set; }

    private readonly List<SaleItem> _items = new();
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    // EF Core constructor
    private Sale() { }

    /// <summary>
    /// Creates a new sale with items
    /// </summary>
    public static Sale Create(
        string customer,
        string branch,
        List<SaleItem> items)
    {
        if (string.IsNullOrWhiteSpace(customer))
            throw new ArgumentException("Customer name is required", nameof(customer));

        if (string.IsNullOrWhiteSpace(branch))
            throw new ArgumentException("Branch is required", nameof(branch));

        if (items == null || items.Count == 0)
            throw new ArgumentException("Sale must have at least one item", nameof(items));

        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = GenerateSaleNumber(),
            SaleDate = DateTime.UtcNow,
            Customer = customer,
            Branch = branch,
            IsCancelled = false
        };

        foreach (var item in items)
        {
            sale._items.Add(item);
        }

        sale.CalculateTotalAmount();
        return sale;
    }

    /// <summary>
    /// Business rule: Calculate total from active items
    /// </summary>
    public void CalculateTotalAmount()
    {
        TotalAmount = _items
            .Where(item => !item.IsCancelled)
            .Sum(item => item.TotalAmount);
    }

    /// <summary>
    /// Business rule: Cancel entire sale
    /// </summary>
    public void Cancel()
    {
        if (IsCancelled)
            throw new InvalidOperationException("Sale is already cancelled");

        IsCancelled = true;
    }

    /// <summary>
    /// Business rule: Cancel individual item and recalculate
    /// </summary>
    public void CancelItem(Guid itemId)
    {
        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            throw new InvalidOperationException($"Item {itemId} not found in sale");

        item.Cancel();
        CalculateTotalAmount();
    }

    /// <summary>
    /// Business rule: Update sale information
    /// </summary>
    public void Update(string customer, string branch)
    {
        if (IsCancelled)
            throw new InvalidOperationException("Cannot update cancelled sale");

        if (string.IsNullOrWhiteSpace(customer))
            throw new ArgumentException("Customer name is required");

        if (string.IsNullOrWhiteSpace(branch))
            throw new ArgumentException("Branch is required");

        Customer = customer;
        Branch = branch;
    }

    private static string GenerateSaleNumber()
    {
        return $"SALE-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }
}