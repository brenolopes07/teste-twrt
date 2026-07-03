using TesteTwrt.Domain.Exceptions;

namespace TesteTwrt.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    protected Product() { }

    public Product(string name, string? description, decimal price, int stock)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new DomainException("O preço deve ser maior que zero.");

        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStock(int quantity)
    {
        if (quantity < 0)
            throw new DomainException("O estoque não pode ser negativo.");

        Stock = quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void DebitStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("A quantidade deve ser maior que zero.");

        if (quantity > Stock)
            throw new DomainException(
                $"Estoque insuficiente para o produto '{Name}'. Disponível: {Stock}, Solicitado: {quantity}.");

        Stock -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ReturnStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("A quantidade deve ser maior que zero.");

        Stock += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
