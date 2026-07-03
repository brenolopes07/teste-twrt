using Moq;
using Xunit;
using OrderEntity = TesteTwrt.Domain.Entities.Order;
using TesteTwrt.Domain.Interfaces.Repositories;
using TesteTwrt.Application.UseCases.Order.GetAllOrders;

namespace TesteTwrt.UnitTests.UseCases.Order;

public class GetAllOrdersUseCaseTests
{
    private readonly Mock<IOrderRepository> _repoMock = new();
    private readonly GetAllOrdersUseCase _sut;

    public GetAllOrdersUseCaseTests()
    {
        _sut = new GetAllOrdersUseCase(_repoMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsAllOrders()
    {
        var customerId = Guid.NewGuid();
        var orders = new List<OrderEntity>
        {
            new OrderEntity(customerId),
            new OrderEntity(customerId),
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

        var result = await _sut.ExecuteAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ExecuteAsync_WithNoOrders_ReturnsEmptyList()
    {
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<OrderEntity>());

        var result = await _sut.ExecuteAsync();

        Assert.Empty(result);
    }
}
