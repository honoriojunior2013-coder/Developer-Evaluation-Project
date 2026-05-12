using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a single item in a sale
/// </summary>
public class SaleItem : BaseEntity
{
    public Guid SaleId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; private set; }

    // EF Core constructor
    private SaleItem() { }

    /// <summary>
    /// Creates a new sale item with discount validation
    /// </summary>
    public static SaleItem Create(
        string productName,
        int quantity,
        decimal unitPrice,
        decimal discount)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name cannot be empty", nameof(productName));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        if (unitPrice <= 0)
            throw new ArgumentException("Unit price must be positive", nameof(unitPrice));

        if (discount < 0 || discount > 100)
            throw new ArgumentException("Discount must be between 0 and 100%", nameof(discount));

        var item = new SaleItem
        {
            Id = Guid.NewGuid(),
            ProductName = productName,
            Quantity = quantity,
            UnitPrice = unitPrice,
            Discount = discount,
            IsCancelled = false
        };

        item.CalculateTotalAmount();
        return item;
    }

    /// <summary>
    /// Business rule: Total = (Quantity × UnitPrice) × (1 - Discount/100)
    /// </summary>
    public void CalculateTotalAmount()
    {
        var subtotal = Quantity * UnitPrice;
        var discountAmount = subtotal * (Discount / 100);
        TotalAmount = subtotal - discountAmount;
    }

    /// <summary>
    /// Business rule: Apply discount with validation
    /// </summary>
    public void ApplyDiscount(decimal discountPercentage)
    {
        if (discountPercentage < 0 || discountPercentage > 100)
            throw new ArgumentException("Discount must be between 0 and 100%");

        Discount = discountPercentage;
        CalculateTotalAmount();
    }

    /// <summary>
    /// Business rule: Cancel item (soft delete)
    /// </summary>
    public void Cancel()
    {
        if (IsCancelled)
            throw new InvalidOperationException("Item is already cancelled");

        IsCancelled = true;
    }
}