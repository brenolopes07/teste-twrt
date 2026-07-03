using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;

namespace TesteTwrt.Application.UseCases.Product.UpdateProductPrice;

public class UpdateProductPriceUseCase : IUpdateProductPriceUseCase
{
    private readonly IProductRepository _repository;

    public UpdateProductPriceUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(Guid id, UpdateProductPriceInput input)
    {
        var product = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Produto com ID '{id}' não encontrado.");

        product.UpdatePrice(input.Price);
        await _repository.SaveAsync();
    }
}
