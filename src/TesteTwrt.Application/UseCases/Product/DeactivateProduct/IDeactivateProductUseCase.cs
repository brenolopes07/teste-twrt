namespace TesteTwrt.Application.UseCases.Product.DeactivateProduct;

public interface IDeactivateProductUseCase
{
    Task ExecuteAsync(Guid id);
}
