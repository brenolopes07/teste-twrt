using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Application.DTOs.Output;
using TesteTwrt.Domain.Interfaces.Repositories;
using ProductEntity = TesteTwrt.Domain.Entities.Product;

namespace TesteTwrt.Application.UseCases.Product.CreateProduct;

public class CreateProductUseCase : ICreateProductUseCase
{
    private readonly IProductRepository _repository;

    public CreateProductUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductOutput> ExecuteAsync(CreateProductInput input)
    {
        var product = new ProductEntity(input.Name, input.Description, input.Price, input.Stock);
        await _repository.AddAsync(product);
        return ProductOutput.FromEntity(product);
    }
}
