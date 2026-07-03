using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Application.DTOs.Output;

namespace TesteTwrt.Application.UseCases.Product.CreateProduct;

public interface ICreateProductUseCase
{
    Task<ProductOutput> ExecuteAsync(CreateProductInput input);
}
