using TesteTwrt.Application.DTOs.Input;

namespace TesteTwrt.Application.UseCases.Product.UpdateProductStock;

public interface IUpdateProductStockUseCase
{
    Task ExecuteAsync(Guid id, UpdateProductStockInput input);
}
