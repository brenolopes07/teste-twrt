using TesteTwrt.Domain.Entities;

namespace TesteTwrt.Application.DTOs.Output;

public record CustomerOutput(
    Guid Id,
    string Name,
    string Email,
    string Document,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static CustomerOutput FromEntity(Customer c) =>
        new(c.Id, c.Name, c.Email, c.Document, c.IsActive, c.CreatedAt, c.UpdatedAt);
}
