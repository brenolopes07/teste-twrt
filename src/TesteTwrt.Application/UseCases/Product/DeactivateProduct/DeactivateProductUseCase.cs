using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;

namespace TesteTwrt.Application.UseCases.Product.DeactivateProduct;

public class DeactivateProductUseCase : IDeactivateProductUseCase
{
    private readonly IProductRepository _repository;

    public DeactivateProductUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Produto com ID '{id}' não encontrado.");

        product.Deactivate();
        await _repository.SaveAsync();
    }
}
