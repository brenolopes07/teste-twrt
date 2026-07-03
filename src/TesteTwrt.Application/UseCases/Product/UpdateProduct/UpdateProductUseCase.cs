using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Application.DTOs.Output;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;

namespace TesteTwrt.Application.UseCases.Product.UpdateProduct;

public class UpdateProductUseCase : IUpdateProductUseCase
{
    private readonly IProductRepository _repository;

    public UpdateProductUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductOutput> ExecuteAsync(Guid id, UpdateProductInput input)
    {
        var product = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Produto com ID '{id}' não encontrado.");

        product.Update(input.Name, input.Description);
        await _repository.SaveAsync();

        return ProductOutput.FromEntity(product);
    }
}
