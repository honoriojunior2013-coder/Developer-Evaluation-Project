using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.UnitTests.Domain.Entities;

public class SaleTests
{
    [Fact(DisplayName = "Sale total should be the sum of non-cancelled items")]
    public void Should_Calculate_Total_Correctly_With_Multiple_Items()
    {
        var item1 = SaleItem.Create("Prod 1", 5, 100m, 10); // 500 - 10% = 450
        var item2 = SaleItem.Create("Prod 2", 11, 100m, 20); // 1100 - 20% = 880
        var items = new List<SaleItem> { item1, item2 };

        // Act
        var sale = Sale.Create("Customer", "Branch", items);

        // Assert
        sale.TotalAmount.Should().Be(1330m); // 450 + 880
    }

    [Fact(DisplayName = "Cancelling an item should recalculate sale total")]
    public void Should_Recalculate_Total_When_Item_Is_Cancelled()
    {
        // Arrange
        var item1 = SaleItem.Create("Prod 1", 2, 100m, 0); // 200
        var item2 = SaleItem.Create("Prod 2", 2, 100m, 0); // 200
        var sale = Sale.Create("Customer", "Branch", new List<SaleItem> { item1, item2 });
        var originalTotal = sale.TotalAmount; // 400

        // Act
        sale.CancelItem(item1.Id);

        // Assert
        sale.TotalAmount.Should().Be(200m);
        item1.IsCancelled.Should().BeTrue();
    }
}