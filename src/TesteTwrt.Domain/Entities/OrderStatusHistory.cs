using TesteTwrt.Domain.Enums;

namespace TesteTwrt.Domain.Entities;

public class OrderStatusHistory
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public virtual Order? Order { get; private set; }
    public OrderStatus PreviousStatus { get; private set; }
    public OrderStatus NewStatus { get; private set; }
    public DateTime ChangedAt { get; private set; }
    public string? Reason { get; private set; }

    protected OrderStatusHistory() { }

    public OrderStatusHistory(Guid orderId, OrderStatus previousStatus, OrderStatus newStatus, string? reason)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        PreviousStatus = previousStatus;
        NewStatus = newStatus;
        ChangedAt = DateTime.UtcNow;
        Reason = reason;
    }
}
