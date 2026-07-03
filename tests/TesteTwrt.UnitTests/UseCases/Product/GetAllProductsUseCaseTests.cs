using Moq;
using Xunit;
using ProductEntity = TesteTwrt.Domain.Entities.Product;
using TesteTwrt.Domain.Interfaces.Repositories;
using TesteTwrt.Application.UseCases.Product.GetAllProducts;

namespace TesteTwrt.UnitTests.UseCases.Product;

public class GetAllProductsUseCaseTests
{
    private readonly Mock<IProductRepository> _repoMock = new();
    private readonly GetAllProductsUseCase _sut;

    public GetAllProductsUseCaseTests()
    {
        _sut = new GetAllProductsUseCase(_repoMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsAllProducts()
    {
        var products = new List<ProductEntity>
        {
            new ProductEntity("Produto A", null, 10m, 5),
            new ProductEntity("Produto B", null, 20m, 3),
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

        var result = await _sut.ExecuteAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ExecuteAsync_WithNoProducts_ReturnsEmptyList()
    {
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<ProductEntity>());

        var result = await _sut.ExecuteAsync();

        Assert.Empty(result);
    }
}
