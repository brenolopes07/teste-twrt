using TesteTwrt.Domain.Entities;

namespace TesteTwrt.Application.DTOs.Output;

public record ProductOutput(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    int Stock,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static ProductOutput FromEntity(Product p) =>
        new(p.Id, p.Name, p.Description, p.Price, p.Stock, p.IsActive, p.CreatedAt, p.UpdatedAt);
}
