using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;

namespace TesteTwrt.Application.UseCases.Product.ActivateProduct;

public class ActivateProductUseCase : IActivateProductUseCase
{
    private readonly IProductRepository _repository;

    public ActivateProductUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Produto com ID '{id}' não encontrado.");

        product.Activate();
        await _repository.SaveAsync();
    }
}
