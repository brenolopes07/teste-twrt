using Moq;
using Xunit;
using CustomerEntity = TesteTwrt.Domain.Entities.Customer;
using ProductEntity = TesteTwrt.Domain.Entities.Product;
using OrderEntity = TesteTwrt.Domain.Entities.Order;
using OrderItemEntity = TesteTwrt.Domain.Entities.OrderItem;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;
using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Application.UseCases.Order.CreateOrder;

namespace TesteTwrt.UnitTests.UseCases.Order;

public class CreateOrderUseCaseTests
{
    private readonly Mock<IOrderRepository> _orderRepoMock = new();
    private readonly Mock<ICustomerRepository> _customerRepoMock = new();
    private readonly Mock<IProductRepository> _productRepoMock = new();
    private readonly CreateOrderUseCase _sut;

    public CreateOrderUseCaseTests()
    {
        _sut = new CreateOrderUseCase(
            _orderRepoMock.Object,
            _customerRepoMock.Object,
            _productRepoMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidInput_CreatesOrderAndDebitsStock()
    {
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var customer = new CustomerEntity("João Silva", "joao@test.com", "12345678901");
        var product = new ProductEntity("Produto A", null, 50m, 10);

        var input = new CreateOrderInput
        {
            CustomerId = customerId,
            Items = new List<CreateOrderItemInput>
            {
                new CreateOrderItemInput { ProductId = productId, Quantity = 3 }
            }
        };

        _customerRepoMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(customer);
        _productRepoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
        _orderRepoMock.Setup(r => r.AddAsync(It.IsAny<OrderEntity>())).Returns(Task.CompletedTask);

        var returnedOrder = new OrderEntity(customerId);
        returnedOrder.AddItem(new OrderItemEntity(returnedOrder.Id, productId, 3, 50m));
        _orderRepoMock.Setup(r => r.GetByIdWithDetailsAsync(It.IsAny<Guid>())).ReturnsAsync(returnedOrder);

        var result = await _sut.ExecuteAsync(input);

        Assert.Equal(7, product.Stock);
        Assert.NotNull(result);
        _orderRepoMock.Verify(r => r.AddAsync(It.IsAny<OrderEntity>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistingCustomer_ThrowsNotFoundException()
    {
        var customerId = Guid.NewGuid();
        var input = new CreateOrderInput
        {
            CustomerId = customerId,
            Items = new List<CreateOrderItemInput> { new CreateOrderItemInput { ProductId = Guid.NewGuid(), Quantity = 1 } }
        };
        _customerRepoMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync((CustomerEntity?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.ExecuteAsync(input));
    }

    [Fact]
    public async Task ExecuteAsync_WithInactiveCustomer_ThrowsDomainException()
    {
        var customerId = Guid.NewGuid();
        var customer = new CustomerEntity("João Silva", "joao@test.com", "12345678901");
        customer.Deactivate();
        var input = new CreateOrderInput
        {
            CustomerId = customerId,
            Items = new List<CreateOrderItemInput> { new CreateOrderItemInput { ProductId = Guid.NewGuid(), Quantity = 1 } }
        };
        _customerRepoMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(customer);

        await Assert.ThrowsAsync<DomainException>(() => _sut.ExecuteAsync(input));
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistingProduct_ThrowsNotFoundException()
    {
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var customer = new CustomerEntity("João Silva", "joao@test.com", "12345678901");
        var input = new CreateOrderInput
        {
            CustomerId = customerId,
            Items = new List<CreateOrderItemInput> { new CreateOrderItemInput { ProductId = productId, Quantity = 1 } }
        };
        _customerRepoMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(customer);
        _productRepoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((ProductEntity?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.ExecuteAsync(input));
    }

    [Fact]
    public async Task ExecuteAsync_WithInactiveProduct_ThrowsDomainException()
    {
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var customer = new CustomerEntity("João Silva", "joao@test.com", "12345678901");
        var product = new ProductEntity("Produto A", null, 50m, 10);
        product.Deactivate();
        var input = new CreateOrderInput
        {
            CustomerId = customerId,
            Items = new List<CreateOrderItemInput> { new CreateOrderItemInput { ProductId = productId, Quantity = 1 } }
        };
        _customerRepoMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(customer);
        _productRepoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);

        await Assert.ThrowsAsync<DomainException>(() => _sut.ExecuteAsync(input));
    }

    [Fact]
    public async Task ExecuteAsync_WithInsufficientStock_ThrowsDomainException()
    {
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var customer = new CustomerEntity("João Silva", "joao@test.com", "12345678901");
        var product = new ProductEntity("Produto A", null, 50m, 2);
        var input = new CreateOrderInput
        {
            CustomerId = customerId,
            Items = new List<CreateOrderItemInput> { new CreateOrderItemInput { ProductId = productId, Quantity = 5 } }
        };
        _customerRepoMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(customer);
        _productRepoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);

        await Assert.ThrowsAsync<DomainException>(() => _sut.ExecuteAsync(input));
    }
}
