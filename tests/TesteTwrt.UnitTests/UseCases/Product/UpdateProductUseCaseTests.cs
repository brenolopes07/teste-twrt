using Moq;
using Xunit;
using ProductEntity = TesteTwrt.Domain.Entities.Product;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;
using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Application.UseCases.Product.UpdateProduct;

namespace TesteTwrt.UnitTests.UseCases.Product;

public class UpdateProductUseCaseTests
{
    private readonly Mock<IProductRepository> _repoMock = new();
    private readonly UpdateProductUseCase _sut;

    public UpdateProductUseCaseTests()
    {
        _sut = new UpdateProductUseCase(_repoMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithExistingProduct_UpdatesAndReturns()
    {
        var productId = Guid.NewGuid();
        var product = new ProductEntity("Nome Antigo", "Desc Antiga", 50m, 10);
        var input = new UpdateProductInput { Name = "Nome Novo", Description = "Desc Nova" };
        _repoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
        _repoMock.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);

        var result = await _sut.ExecuteAsync(productId, input);

        Assert.Equal("Nome Novo", result.Name);
        Assert.Equal("Desc Nova", result.Description);
        _repoMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistingId_ThrowsNotFoundException()
    {
        var productId = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((ProductEntity?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _sut.ExecuteAsync(productId, new UpdateProductInput { Name = "X" }));
    }
}
