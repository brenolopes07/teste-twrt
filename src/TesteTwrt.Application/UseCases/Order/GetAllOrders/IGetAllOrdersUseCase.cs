using TesteTwrt.Application.DTOs.Output;

namespace TesteTwrt.Application.UseCases.Order.GetAllOrders;

public interface IGetAllOrdersUseCase
{
    Task<IEnumerable<OrderOutput>> ExecuteAsync();
}
