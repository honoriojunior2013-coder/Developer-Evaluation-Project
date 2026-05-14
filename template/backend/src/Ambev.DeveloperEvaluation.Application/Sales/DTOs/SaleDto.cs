namespace Ambev.DeveloperEvaluation.Application.Sales.DTOs;

public record SaleDto(
    Guid Id,
    string SaleNumber,
    DateTime SaleDate,
    string Customer,
    string Branch,
    decimal TotalAmount,
    bool IsCancelled,
    List<SaleItemDto> Items
);

public record SaleItemDto(
    Guid Id,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal Discount,
    decimal TotalAmount,
    bool IsCancelled
);