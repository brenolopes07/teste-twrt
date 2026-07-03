using TesteTwrt.Domain.Enums;

namespace TesteTwrt.Application.DTOs.Output;

public record OrderStatusHistoryOutput(
    Guid Id,
    OrderStatus PreviousStatus,
    string PreviousStatusName,
    OrderStatus NewStatus,
    string NewStatusName,
    DateTime ChangedAt,
    string? Reason);
