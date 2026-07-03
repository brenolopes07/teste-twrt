using Moq;
using Xunit;
using ProductEntity = TesteTwrt.Domain.Entities.Product;
using TesteTwrt.Domain.Interfaces.Repositories;
using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Application.UseCases.Product.CreateProduct;

namespace TesteTwrt.UnitTests.UseCases.Product;

public class CreateProductUseCaseTests
{
    private readonly Mock<IProductRepository> _repoMock = new();
    private readonly CreateProductUseCase _sut;

    public CreateProductUseCaseTests()
    {
        _sut = new CreateProductUseCase(_repoMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidInput_CreatesAndReturnsProduct()
    {
        var input = new CreateProductInput { Name = "Produto A", Description = "Desc", Price = 99.90m, Stock = 50 };
        _repoMock.Setup(r => r.AddAsync(It.IsAny<ProductEntity>())).Returns(Task.CompletedTask);

        var result = await _sut.ExecuteAsync(input);

        Assert.Equal("Produto A", result.Name);
        Assert.Equal("Desc", result.Description);
        Assert.Equal(99.90m, result.Price);
        Assert.Equal(50, result.Stock);
        Assert.True(result.IsActive);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<ProductEntity>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithNullDescription_CreatesProductWithNullDescription()
    {
        var input = new CreateProductInput { Name = "Produto B", Description = null, Price = 10m, Stock = 5 };
        _repoMock.Setup(r => r.AddAsync(It.IsAny<ProductEntity>())).Returns(Task.CompletedTask);

        var result = await _sut.ExecuteAsync(input);

        Assert.Null(result.Description);
    }
}
