using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Application.DTOs.Output;

namespace TesteTwrt.Application.UseCases.Customer.CreateCustomer;

public interface ICreateCustomerUseCase
{
    Task<CustomerOutput> ExecuteAsync(CreateCustomerInput input);
}
