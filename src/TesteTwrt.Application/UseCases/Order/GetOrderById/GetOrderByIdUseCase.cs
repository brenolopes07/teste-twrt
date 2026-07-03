using TesteTwrt.Application.DTOs.Output;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;

namespace TesteTwrt.Application.UseCases.Order.GetOrderById;

public class GetOrderByIdUseCase : IGetOrderByIdUseCase
{
    private readonly IOrderRepository _repository;

    public GetOrderByIdUseCase(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<OrderOutput> ExecuteAsync(Guid id)
    {
        var order = await _repository.GetByIdWithDetailsAsync(id)
            ?? throw new NotFoundException($"Pedido com ID '{id}' não encontrado.");

        return OrderOutput.FromEntity(order);
    }
}
