using TesteTwrt.Domain.Entities;

namespace TesteTwrt.Domain.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<bool> ExistsActiveWithEmailAsync(string email, Guid? excludeId = null);
    Task<bool> ExistsActiveWithDocumentAsync(string document, Guid? excludeId = null);
    Task AddAsync(Customer customer);    Task SaveAsync();}
