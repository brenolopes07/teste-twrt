using TesteTwrt.Domain.Entities;
using TesteTwrt.Domain.Enums;

namespace TesteTwrt.Application.DTOs.Output;

public record OrderOutput(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    DateTime CreatedAt,
    OrderStatus Status,
    string StatusName,
    decimal TotalValue,
    IEnumerable<OrderItemOutput> Items,
    IEnumerable<OrderStatusHistoryOutput> StatusHistory)
{
    public static string StatusLabel(OrderStatus status) => status switch
    {
        OrderStatus.Created   => "Criado",
        OrderStatus.Paid      => "Pago",
        OrderStatus.Shipped   => "Enviado",
        OrderStatus.Cancelled => "Cancelado",
        _                     => status.ToString()
    };

    public static OrderOutput FromEntity(Order order) => new(
        order.Id,
        order.CustomerId,
        order.Customer?.Name ?? string.Empty,
        order.CreatedAt,
        order.Status,
        StatusLabel(order.Status),
        order.TotalValue,
        order.Items.Select(i => new OrderItemOutput(
            i.Id, i.ProductId, i.Product?.Name ?? string.Empty,
            i.Quantity, i.UnitPrice, i.TotalValue)),
        order.StatusHistory
            .OrderBy(h => h.ChangedAt)
            .Select(h => new OrderStatusHistoryOutput(
                h.Id,
                h.PreviousStatus, StatusLabel(h.PreviousStatus),
                h.NewStatus, StatusLabel(h.NewStatus),
                h.ChangedAt, h.Reason)));
}
