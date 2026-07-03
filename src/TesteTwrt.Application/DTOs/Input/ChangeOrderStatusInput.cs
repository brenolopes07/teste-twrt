using TesteTwrt.Domain.Enums;

namespace TesteTwrt.Application.DTOs.Input;

public class ChangeOrderStatusInput
{
    public OrderStatus NewStatus { get; set; }
    public string? Reason { get; set; }
}
