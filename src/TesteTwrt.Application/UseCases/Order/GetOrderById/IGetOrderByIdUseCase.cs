using TesteTwrt.Application.DTOs.Output;

namespace TesteTwrt.Application.UseCases.Order.GetOrderById;

public interface IGetOrderByIdUseCase
{
    Task<OrderOutput> ExecuteAsync(Guid id);
}
