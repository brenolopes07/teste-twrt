using TesteTwrt.Application.DTOs.Output;
using TesteTwrt.Domain.Interfaces.Repositories;

namespace TesteTwrt.Application.UseCases.Product.GetAllProducts;

public class GetAllProductsUseCase : IGetAllProductsUseCase
{
    private readonly IProductRepository _repository;

    public GetAllProductsUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductOutput>> ExecuteAsync()
    {
        var products = await _repository.GetAllAsync();
        return products.Select(ProductOutput.FromEntity);
    }
}
