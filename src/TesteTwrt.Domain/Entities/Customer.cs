using TesteTwrt.Domain.Exceptions;

namespace TesteTwrt.Domain.Entities;

public class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Document { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    protected Customer() { }

    public Customer(string name, string email, string document)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Document = document;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException("O cliente já está inativo.");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
