using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Application.DTOs.Output;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;
using OrderEntity = TesteTwrt.Domain.Entities.Order;
using OrderItemEntity = TesteTwrt.Domain.Entities.OrderItem;
using ProductEntity = TesteTwrt.Domain.Entities.Product;

namespace TesteTwrt.Application.UseCases.Order.CreateOrder;

public class CreateOrderUseCase : ICreateOrderUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;

    public CreateOrderUseCase(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
    }

    public async Task<OrderOutput> ExecuteAsync(CreateOrderInput input)
    {
        var customer = await _customerRepository.GetByIdAsync(input.CustomerId)
            ?? throw new NotFoundException($"Cliente com ID '{input.CustomerId}' não encontrado.");

        if (!customer.IsActive)
            throw new DomainException("Clientes inativos não podem criar pedidos.");

        var productMap = new Dictionary<Guid, ProductEntity>();
        foreach (var item in input.Items)
        {
            if (!productMap.ContainsKey(item.ProductId))
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId)
                    ?? throw new NotFoundException($"Produto com ID '{item.ProductId}' não encontrado.");

                if (!product.IsActive)
                    throw new DomainException($"O produto '{product.Name}' está inativo e não pode ser adicionado a pedidos.");

                productMap[item.ProductId] = product;
            }
        }

        var stockRequired = input.Items
            .GroupBy(i => i.ProductId)
            .ToDictionary(g => g.Key, g => g.Sum(i => i.Quantity));

        foreach (var (productId, qty) in stockRequired)
        {
            var product = productMap[productId];
            if (qty > product.Stock)
                throw new DomainException(
                    $"Estoque insuficiente para o produto '{product.Name}'. Disponível: {product.Stock}, Solicitado: {qty}.");
        }

        var order = new OrderEntity(customer.Id);

        foreach (var item in input.Items)
        {
            var product = productMap[item.ProductId];
            product.DebitStock(item.Quantity);
            order.AddItem(new OrderItemEntity(order.Id, product.Id, item.Quantity, product.Price));
        }

        await _orderRepository.AddAsync(order);

        var created = await _orderRepository.GetByIdWithDetailsAsync(order.Id);
        return OrderOutput.FromEntity(created!);
    }
}
