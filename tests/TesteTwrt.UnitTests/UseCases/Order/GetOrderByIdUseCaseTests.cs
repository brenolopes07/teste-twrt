using Moq;
using Xunit;
using OrderEntity = TesteTwrt.Domain.Entities.Order;
using TesteTwrt.Domain.Enums;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;
using TesteTwrt.Application.UseCases.Order.GetOrderById;

namespace TesteTwrt.UnitTests.UseCases.Order;

public class GetOrderByIdUseCaseTests
{
    private readonly Mock<IOrderRepository> _repoMock = new();
    private readonly GetOrderByIdUseCase _sut;

    public GetOrderByIdUseCaseTests()
    {
        _sut = new GetOrderByIdUseCase(_repoMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithExistingId_ReturnsOrderOutput()
    {
        var orderId = Guid.NewGuid();
        var order = new OrderEntity(Guid.NewGuid());
        _repoMock.Setup(r => r.GetByIdWithDetailsAsync(orderId)).ReturnsAsync(order);

        var result = await _sut.ExecuteAsync(orderId);

        Assert.NotNull(result);
        Assert.Equal(OrderStatus.Created, result.Status);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistingId_ThrowsNotFoundException()
    {
        var orderId = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdWithDetailsAsync(orderId)).ReturnsAsync((OrderEntity?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.ExecuteAsync(orderId));
    }
}
