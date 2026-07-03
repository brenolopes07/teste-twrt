namespace TesteTwrt.Application.UseCases.Customer.DeactivateCustomer;

public interface IDeactivateCustomerUseCase
{
    Task ExecuteAsync(Guid id);
}
