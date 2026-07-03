using TesteTwrt.Application.DTOs.Output;
using TesteTwrt.Domain.Interfaces.Repositories;

namespace TesteTwrt.Application.UseCases.Customer.GetAllCustomers;

public class GetAllCustomersUseCase : IGetAllCustomersUseCase
{
    private readonly ICustomerRepository _repository;

    public GetAllCustomersUseCase(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CustomerOutput>> ExecuteAsync()
    {
        var customers = await _repository.GetAllAsync();
        return customers.Select(CustomerOutput.FromEntity);
    }
}
