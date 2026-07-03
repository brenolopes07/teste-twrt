using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;

namespace TesteTwrt.Application.UseCases.Customer.DeactivateCustomer;

public class DeactivateCustomerUseCase : IDeactivateCustomerUseCase
{
    private readonly ICustomerRepository _repository;

    public DeactivateCustomerUseCase(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(Guid id)
    {
        var customer = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Cliente com ID '{id}' não encontrado.");

        customer.Deactivate();
        await _repository.SaveAsync();
    }
}
