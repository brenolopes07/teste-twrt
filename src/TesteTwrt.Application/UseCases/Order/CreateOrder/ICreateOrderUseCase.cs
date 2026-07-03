using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Application.DTOs.Output;

namespace TesteTwrt.Application.UseCases.Order.CreateOrder;

public interface ICreateOrderUseCase
{
    Task<OrderOutput> ExecuteAsync(CreateOrderInput input);
}
