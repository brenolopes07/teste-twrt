using TesteTwrt.Application.DTOs.Output;
using TesteTwrt.Domain.Interfaces.Repositories;

namespace TesteTwrt.Application.UseCases.Order.GetAllOrders;

public class GetAllOrdersUseCase : IGetAllOrdersUseCase
{
    private readonly IOrderRepository _repository;

    public GetAllOrdersUseCase(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<OrderOutput>> ExecuteAsync()
    {
        var orders = await _repository.GetAllAsync();
        return orders.Select(OrderOutput.FromEntity);
    }
}
