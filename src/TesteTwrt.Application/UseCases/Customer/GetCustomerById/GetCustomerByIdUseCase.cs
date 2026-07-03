using TesteTwrt.Application.DTOs.Output;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;

namespace TesteTwrt.Application.UseCases.Customer.GetCustomerById;

public class GetCustomerByIdUseCase : IGetCustomerByIdUseCase
{
    private readonly ICustomerRepository _repository;

    public GetCustomerByIdUseCase(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerOutput> ExecuteAsync(Guid id)
    {
        var customer = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Cliente com ID '{id}' não encontrado.");

        return CustomerOutput.FromEntity(customer);
    }
}
