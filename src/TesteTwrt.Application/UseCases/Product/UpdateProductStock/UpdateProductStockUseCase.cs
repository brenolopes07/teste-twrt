using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;

namespace TesteTwrt.Application.UseCases.Product.UpdateProductStock;

public class UpdateProductStockUseCase : IUpdateProductStockUseCase
{
    private readonly IProductRepository _repository;

    public UpdateProductStockUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(Guid id, UpdateProductStockInput input)
    {
        var product = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Produto com ID '{id}' não encontrado.");

        product.UpdateStock(input.Stock);
        await _repository.SaveAsync();
    }
}
