using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Domain.Enums;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;

namespace TesteTwrt.Application.UseCases.Order.ChangeOrderStatus;

public class ChangeOrderStatusUseCase : IChangeOrderStatusUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public ChangeOrderStatusUseCase(
        IOrderRepository orderRepository,
        IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task ExecuteAsync(Guid id, ChangeOrderStatusInput input)
    {
        var order = await _orderRepository.GetByIdWithDetailsAsync(id)
            ?? throw new NotFoundException($"Pedido com ID '{id}' não encontrado.");

        var shouldReturnStock = input.NewStatus == OrderStatus.Cancelled && order.CanReturnStock();

        order.ChangeStatus(input.NewStatus, input.Reason);

        if (shouldReturnStock)
        {
            foreach (var item in order.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                product?.ReturnStock(item.Quantity);
            }
        }

        await _orderRepository.SaveAsync();
    }
}
