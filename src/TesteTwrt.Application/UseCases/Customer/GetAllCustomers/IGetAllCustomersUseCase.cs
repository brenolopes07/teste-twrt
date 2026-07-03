using TesteTwrt.Application.DTOs.Output;

namespace TesteTwrt.Application.UseCases.Customer.GetAllCustomers;

public interface IGetAllCustomersUseCase
{
    Task<IEnumerable<CustomerOutput>> ExecuteAsync();
}
