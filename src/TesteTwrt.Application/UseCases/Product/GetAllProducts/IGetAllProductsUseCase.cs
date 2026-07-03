using TesteTwrt.Application.DTOs.Output;

namespace TesteTwrt.Application.UseCases.Product.GetAllProducts;

public interface IGetAllProductsUseCase
{
    Task<IEnumerable<ProductOutput>> ExecuteAsync();
}
