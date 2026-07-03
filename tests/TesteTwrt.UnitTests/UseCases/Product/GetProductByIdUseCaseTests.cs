using Moq;
using Xunit;
using ProductEntity = TesteTwrt.Domain.Entities.Product;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;
using TesteTwrt.Application.UseCases.Product.GetProductById;

namespace TesteTwrt.UnitTests.UseCases.Product;

public class GetProductByIdUseCaseTests
{
    private readonly Mock<IProductRepository> _repoMock = new();
    private readonly GetProductByIdUseCase _sut;

    public GetProductByIdUseCaseTests()
    {
        _sut = new GetProductByIdUseCase(_repoMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithExistingId_ReturnsProductOutput()
    {
        var productId = Guid.NewGuid();
        var product = new ProductEntity("Produto A", "Desc", 50m, 10);
        _repoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);

        var result = await _sut.ExecuteAsync(productId);

        Assert.Equal("Produto A", result.Name);
        Assert.Equal(50m, result.Price);
        Assert.Equal(10, result.Stock);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistingId_ThrowsNotFoundException()
    {
        var productId = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((ProductEntity?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.ExecuteAsync(productId));
    }
}
