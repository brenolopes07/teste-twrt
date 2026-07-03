using Microsoft.EntityFrameworkCore;
using TesteTwrt.Domain.Entities;
using TesteTwrt.Domain.Interfaces.Repositories;
using TesteTwrt.Infrastructure.Data;

namespace TesteTwrt.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(Guid id) =>
        await _context.Orders.FindAsync(id);

    public async Task<Order?> GetByIdWithDetailsAsync(Guid id) =>
        await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Include(o => o.StatusHistory)
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<IEnumerable<Order>> GetAllAsync() =>
        await _context.Orders
            .AsNoTracking()
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Include(o => o.StatusHistory)
            .ToListAsync();

    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public Task SaveAsync() => _context.SaveChangesAsync();
}
