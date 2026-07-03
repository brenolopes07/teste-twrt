using TesteTwrt.Application.DTOs.Input;

namespace TesteTwrt.Application.UseCases.Order.ChangeOrderStatus;

public interface IChangeOrderStatusUseCase
{
    Task ExecuteAsync(Guid id, ChangeOrderStatusInput input);
}
