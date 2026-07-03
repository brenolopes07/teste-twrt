using Moq;
using Xunit;
using OrderEntity = TesteTwrt.Domain.Entities.Order;
using OrderItemEntity = TesteTwrt.Domain.Entities.OrderItem;
using ProductEntity = TesteTwrt.Domain.Entities.Product;
using TesteTwrt.Domain.Enums;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;
using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Application.UseCases.Order.ChangeOrderStatus;

namespace TesteTwrt.UnitTests.UseCases.Order;

public class ChangeOrderStatusUseCaseTests
{
    private readonly Mock<IOrderRepository> _orderRepoMock = new();
    private readonly Mock<IProductRepository> _productRepoMock = new();
    private readonly ChangeOrderStatusUseCase _sut;

    public ChangeOrderStatusUseCaseTests()
    {
        _sut = new ChangeOrderStatusUseCase(_orderRepoMock.Object, _productRepoMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_Created_To_Paid_ChangesStatus()
    {
        var orderId = Guid.NewGuid();
        var order = new OrderEntity(Guid.NewGuid());
        _orderRepoMock.Setup(r => r.GetByIdWithDetailsAsync(orderId)).ReturnsAsync(order);
        _orderRepoMock.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);

        var input = new ChangeOrderStatusInput { NewStatus = OrderStatus.Paid };

        await _sut.ExecuteAsync(orderId, input);

        Assert.Equal(OrderStatus.Paid, order.Status);
        _orderRepoMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCancellingCreatedOrder_ReturnsStockToProducts()
    {
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var product = new ProductEntity("Produto A", null, 50m, 5);

        var order = new OrderEntity(Guid.NewGuid());
        order.AddItem(new OrderItemEntity(order.Id, productId, 2, 50m));

        _orderRepoMock.Setup(r => r.GetByIdWithDetailsAsync(orderId)).ReturnsAsync(order);
        _productRepoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
        _orderRepoMock.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);

        var input = new ChangeOrderStatusInput { NewStatus = OrderStatus.Cancelled };

        await _sut.ExecuteAsync(orderId, input);

        Assert.Equal(OrderStatus.Cancelled, order.Status);
        Assert.Equal(7, product.Stock);
        _productRepoMock.Verify(r => r.GetByIdAsync(productId), Times.Once);
        _orderRepoMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenShipped_DoesNotReturnStock()
    {
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var product = new ProductEntity("Produto A", null, 50m, 5);

        var order = new OrderEntity(Guid.NewGuid());
        order.AddItem(new OrderItemEntity(order.Id, productId, 2, 50m));
        order.ChangeStatus(OrderStatus.Paid);
        order.ChangeStatus(OrderStatus.Shipped);

        _orderRepoMock.Setup(r => r.GetByIdWithDetailsAsync(orderId)).ReturnsAsync(order);
        _orderRepoMock.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);

        var input = new ChangeOrderStatusInput { NewStatus = OrderStatus.Paid };

        await Assert.ThrowsAsync<DomainException>(() => _sut.ExecuteAsync(orderId, input));

        _productRepoMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistingOrder_ThrowsNotFoundException()
    {
        var orderId = Guid.NewGuid();
        _orderRepoMock.Setup(r => r.GetByIdWithDetailsAsync(orderId)).ReturnsAsync((OrderEntity?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _sut.ExecuteAsync(orderId, new ChangeOrderStatusInput { NewStatus = OrderStatus.Paid }));
    }

    [Fact]
    public async Task ExecuteAsync_WithInvalidTransition_ThrowsDomainException()
    {
        var orderId = Guid.NewGuid();
        var order = new OrderEntity(Guid.NewGuid());
        order.ChangeStatus(OrderStatus.Paid);
        order.ChangeStatus(OrderStatus.Shipped);

        _orderRepoMock.Setup(r => r.GetByIdWithDetailsAsync(orderId)).ReturnsAsync(order);

        var input = new ChangeOrderStatusInput { NewStatus = OrderStatus.Cancelled };

        await Assert.ThrowsAsync<DomainException>(() => _sut.ExecuteAsync(orderId, input));
    }
}
