using TesteTwrt.Application.DTOs.Output;

namespace TesteTwrt.Application.UseCases.Customer.GetCustomerById;

public interface IGetCustomerByIdUseCase
{
    Task<CustomerOutput> ExecuteAsync(Guid id);
}
