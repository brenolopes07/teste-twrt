using TesteTwrt.Domain.Entities;

namespace TesteTwrt.Domain.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<bool> IsLinkedToOrderAsync(Guid productId);
    Task AddAsync(Product product);    Task SaveAsync();}
