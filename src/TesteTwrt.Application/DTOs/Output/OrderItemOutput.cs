namespace TesteTwrt.Application.DTOs.Output;

public record OrderItemOutput(
    Guid Id,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalValue);
