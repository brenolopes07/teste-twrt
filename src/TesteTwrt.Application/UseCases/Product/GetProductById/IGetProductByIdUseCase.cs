using TesteTwrt.Application.DTOs.Output;

namespace TesteTwrt.Application.UseCases.Product.GetProductById;

public interface IGetProductByIdUseCase
{
    Task<ProductOutput> ExecuteAsync(Guid id);
}
