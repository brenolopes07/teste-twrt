using Microsoft.EntityFrameworkCore;
using TesteTwrt.Domain.Entities;
using TesteTwrt.Domain.Interfaces.Repositories;
using TesteTwrt.Infrastructure.Data;

namespace TesteTwrt.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(Guid id) =>
        await _context.Customers.FindAsync(id);

    public async Task<IEnumerable<Customer>> GetAllAsync() =>
        await _context.Customers.AsNoTracking().ToListAsync();

    public async Task<bool> ExistsActiveWithEmailAsync(string email, Guid? excludeId = null)
    {
        var query = _context.Customers.Where(c => c.IsActive && c.Email == email);
        if (excludeId.HasValue)
            query = query.Where(c => c.Id != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task<bool> ExistsActiveWithDocumentAsync(string document, Guid? excludeId = null)
    {
        var query = _context.Customers.Where(c => c.IsActive && c.Document == document);
        if (excludeId.HasValue)
            query = query.Where(c => c.Id != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public Task SaveAsync() => _context.SaveChangesAsync();
}
