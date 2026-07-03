using Moq;
using Xunit;
using ProductEntity = TesteTwrt.Domain.Entities.Product;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;
using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Application.UseCases.Product.UpdateProductPrice;

namespace TesteTwrt.UnitTests.UseCases.Product;

public class UpdateProductPriceUseCaseTests
{
    private readonly Mock<IProductRepository> _repoMock = new();
    private readonly UpdateProductPriceUseCase _sut;

    public UpdateProductPriceUseCaseTests()
    {
        _sut = new UpdateProductPriceUseCase(_repoMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithExistingProduct_UpdatesPrice()
    {
        var productId = Guid.NewGuid();
        var product = new ProductEntity("Produto A", null, 50m, 10);
        var input = new UpdateProductPriceInput { Price = 79.90m };
        _repoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
        _repoMock.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);

        await _sut.ExecuteAsync(productId, input);

        Assert.Equal(79.90m, product.Price);
        _repoMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistingId_ThrowsNotFoundException()
    {
        var productId = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((ProductEntity?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _sut.ExecuteAsync(productId, new UpdateProductPriceInput { Price = 10m }));
    }
}
