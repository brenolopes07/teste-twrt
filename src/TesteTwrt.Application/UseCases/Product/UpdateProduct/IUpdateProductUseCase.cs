using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Application.DTOs.Output;

namespace TesteTwrt.Application.UseCases.Product.UpdateProduct;

public interface IUpdateProductUseCase
{
    Task<ProductOutput> ExecuteAsync(Guid id, UpdateProductInput input);
}
