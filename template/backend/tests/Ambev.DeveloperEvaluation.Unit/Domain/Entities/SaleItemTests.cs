using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.UnitTests.Domain.Entities;

public class SaleItemTests
{
    [Theory(DisplayName = "Should apply correct discount based on quantity")]
    [InlineData(3, 100, 0, 300)]   // < 4 itens: 0% desconto
    [InlineData(4, 100, 10, 360)]  // 4 itens: 10% desconto
    [InlineData(9, 100, 10, 810)]  // 9 itens: 10% desconto
    [InlineData(10, 100, 20, 800)] // 10 itens: 20% desconto
    [InlineData(20, 100, 20, 1600)]// 20 itens: 20% desconto
    public void Should_Apply_Correct_Discount_Tiers(int quantity, decimal unitPrice, decimal discountPercent, decimal expectedTotal)
    {
        // Act
        var item = SaleItem.Create("Product", quantity, unitPrice, discountPercent);

        // Assert
        item.TotalAmount.Should().Be(expectedTotal);
        item.Discount.Should().Be(discountPercent);
    }

    [Fact(DisplayName = "Should throw exception when quantity is above 20")]
    public void Should_Throw_Exception_When_Quantity_Above_20()
    {
        // Act
        Action act = () => SaleItem.Create("Product", 21, 100m, 20);

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage("*It's not possible to sell above 20 identical items*"); 
    }

    [Fact(DisplayName = "Should throw exception when discount is applied to less than 4 items")]
    public void Should_Throw_Exception_When_Discount_Below_4_Items()
    {
        // Act
        // Try forcing 10% discount in 3 itens
        Action act = () => SaleItem.Create("Product", 3, 100m, 10);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}