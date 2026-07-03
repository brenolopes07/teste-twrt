using Microsoft.EntityFrameworkCore;
using TesteTwrt.Domain.Entities;
using TesteTwrt.Domain.Interfaces.Repositories;
using TesteTwrt.Infrastructure.Data;

namespace TesteTwrt.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id) =>
        await _context.Products.FindAsync(id);

    public async Task<IEnumerable<Product>> GetAllAsync() =>
        await _context.Products.AsNoTracking().ToListAsync();

    public async Task<bool> IsLinkedToOrderAsync(Guid productId) =>
        await _context.OrderItems.AnyAsync(i => i.ProductId == productId);

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public Task SaveAsync() => _context.SaveChangesAsync();
}
