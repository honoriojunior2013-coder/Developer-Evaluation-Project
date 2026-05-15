using Ambev.DeveloperEvaluation.Application.Sales.Commands;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.UnitTests.Application;

public class CreateSaleCommandValidatorTests
{
    private readonly CreateSaleCommandValidator _validator;

    public CreateSaleCommandValidatorTests()
    {
        _validator = new CreateSaleCommandValidator();
    }

    [Fact(DisplayName = "Validator should fail when quantity is above 20")]
    public void Should_Fail_When_Quantity_Is_Above_20()
    {
        // Arrange
        var command = new CreateSaleCommand("Customer", "Branch", new List<CreateSaleItemDto> 
        { 
            new("Product", 21, 100m) // 21 itens
        });

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Quantity"));
    }

    [Fact(DisplayName = "Validator should fail when customer is empty")]
    public void Should_Fail_When_Customer_Is_Empty()
    {
        // Arrange
        var command = new CreateSaleCommand("", "Branch", new List<CreateSaleItemDto> 
        { 
            new("Product", 1, 100m) 
        });

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}