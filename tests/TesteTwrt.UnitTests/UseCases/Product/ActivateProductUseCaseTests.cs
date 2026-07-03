using Moq;
using Xunit;
using ProductEntity = TesteTwrt.Domain.Entities.Product;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;
using TesteTwrt.Application.UseCases.Product.ActivateProduct;

namespace TesteTwrt.UnitTests.UseCases.Product;

public class ActivateProductUseCaseTests
{
    private readonly Mock<IProductRepository> _repoMock = new();
    private readonly ActivateProductUseCase _sut;

    public ActivateProductUseCaseTests()
    {
        _sut = new ActivateProductUseCase(_repoMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithExistingProduct_ActivatesAndSaves()
    {
        var productId = Guid.NewGuid();
        var product = new ProductEntity("Produto A", null, 50m, 10);
        product.Deactivate();
        _repoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
        _repoMock.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);

        await _sut.ExecuteAsync(productId);

        Assert.True(product.IsActive);
        _repoMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistingId_ThrowsNotFoundException()
    {
        var productId = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((ProductEntity?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.ExecuteAsync(productId));
    }
}
