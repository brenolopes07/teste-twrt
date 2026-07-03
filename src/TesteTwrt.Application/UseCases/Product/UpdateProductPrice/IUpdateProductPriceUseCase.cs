using TesteTwrt.Application.DTOs.Input;

namespace TesteTwrt.Application.UseCases.Product.UpdateProductPrice;

public interface IUpdateProductPriceUseCase
{
    Task ExecuteAsync(Guid id, UpdateProductPriceInput input);
}
