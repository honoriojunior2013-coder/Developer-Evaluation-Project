using Ambev.DeveloperEvaluation.Application.Sales.Commands;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;
using MediatR;
using Moq;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.UnitTests.Application;

public class CreateSaleHandlerTests
{
    private readonly Mock<ISaleRepository> _repositoryMock;
    private readonly Mock<ILogger<CreateSaleHandler>> _loggerMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _repositoryMock = new Mock<ISaleRepository>();
        _loggerMock = new Mock<ILogger<CreateSaleHandler>>();
        _mediatorMock = new Mock<IMediator>();
        _handler = new CreateSaleHandler(_repositoryMock.Object, _loggerMock.Object, _mediatorMock.Object);
    }

    [Fact(DisplayName = "Handler should automatically apply 10% discount for 5 items")]
    public async Task Handle_Should_Apply_Discount_Automatically()
    {
        // Arrange
        var command = new CreateSaleCommand(
            "Customer", "Branch", 
            new List<CreateSaleItemDto> { new("Product", 5, 100m) }
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.AddAsync(It.Is<Sale>(s => 
            s.Items.First().Discount == 10), It.IsAny<CancellationToken>()), Times.Once);
    }
}