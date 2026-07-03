using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Application.DTOs.Output;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;
using CustomerEntity = TesteTwrt.Domain.Entities.Customer;

namespace TesteTwrt.Application.UseCases.Customer.CreateCustomer;

public class CreateCustomerUseCase : ICreateCustomerUseCase
{
    private readonly ICustomerRepository _repository;

    public CreateCustomerUseCase(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerOutput> ExecuteAsync(CreateCustomerInput input)
    {
        var cleanedDocument = new string(input.Document.Where(char.IsDigit).ToArray());

        if (await _repository.ExistsActiveWithEmailAsync(input.Email))
            throw new DomainException("Já existe um cliente ativo cadastrado com este e-mail.");

        if (await _repository.ExistsActiveWithDocumentAsync(cleanedDocument))
            throw new DomainException("Já existe um cliente ativo cadastrado com este documento.");

        var customer = new CustomerEntity(input.Name, input.Email, cleanedDocument);
        await _repository.AddAsync(customer);

        return CustomerOutput.FromEntity(customer);
    }
}
