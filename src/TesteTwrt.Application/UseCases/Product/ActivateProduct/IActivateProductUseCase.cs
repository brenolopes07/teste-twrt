namespace TesteTwrt.Application.UseCases.Product.ActivateProduct;

public interface IActivateProductUseCase
{
    Task ExecuteAsync(Guid id);
}
