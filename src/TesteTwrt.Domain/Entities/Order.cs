using TesteTwrt.Domain.Enums;
using TesteTwrt.Domain.Exceptions;

namespace TesteTwrt.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public virtual Customer? Customer { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalValue { get; private set; }

    public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();
    public ICollection<OrderStatusHistory> StatusHistory { get; private set; } = new List<OrderStatusHistory>();

    protected Order() { }

    public Order(Guid customerId)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        Status = OrderStatus.Created;
        CreatedAt = DateTime.UtcNow;
        TotalValue = 0;
    }

    public void AddItem(OrderItem item)
    {
        Items.Add(item);
        RecalculateTotal();
    }

    private void RecalculateTotal()
    {
        TotalValue = Items.Sum(i => i.TotalValue);
    }

    public void ChangeStatus(OrderStatus newStatus, string? reason = null)
    {
        if (Status == newStatus)
            throw new DomainException($"O pedido já está no status '{GetStatusName(newStatus)}'.");

        ValidateTransition(newStatus);

        var history = new OrderStatusHistory(Id, Status, newStatus, reason);
        StatusHistory.Add(history);

        Status = newStatus;
    }

    private void ValidateTransition(OrderStatus newStatus)
    {
        var allowed = Status switch
        {
            OrderStatus.Created => new[] { OrderStatus.Paid, OrderStatus.Cancelled },
            OrderStatus.Paid    => new[] { OrderStatus.Shipped },
            _                   => Array.Empty<OrderStatus>()
        };

        if (!allowed.Contains(newStatus))
            throw new DomainException(
                $"Transição de '{GetStatusName(Status)}' para '{GetStatusName(newStatus)}' não é permitida.");
    }

    public bool CanReturnStock() => Status != OrderStatus.Shipped;

    private static string GetStatusName(OrderStatus status) => status switch
    {
        OrderStatus.Created   => "Criado",
        OrderStatus.Paid      => "Pago",
        OrderStatus.Shipped   => "Enviado",
        OrderStatus.Cancelled => "Cancelado",
        _                     => status.ToString()
    };
}
