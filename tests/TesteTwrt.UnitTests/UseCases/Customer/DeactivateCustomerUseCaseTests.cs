using Moq;
using Xunit;
using CustomerEntity = TesteTwrt.Domain.Entities.Customer;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;
using TesteTwrt.Application.UseCases.Customer.DeactivateCustomer;

namespace TesteTwrt.UnitTests.UseCases.Customer;

public class DeactivateCustomerUseCaseTests
{
    private readonly Mock<ICustomerRepository> _repoMock = new();
    private readonly DeactivateCustomerUseCase _sut;

    public DeactivateCustomerUseCaseTests()
    {
        _sut = new DeactivateCustomerUseCase(_repoMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithActiveCustomer_DeactivatesAndSaves()
    {
        var customerId = Guid.NewGuid();
        var customer = new CustomerEntity("João Silva", "joao@test.com", "12345678901");
        _repoMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(customer);
        _repoMock.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);

        await _sut.ExecuteAsync(customerId);

        Assert.False(customer.IsActive);
        _repoMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistingId_ThrowsNotFoundException()
    {
        var customerId = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync((CustomerEntity?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.ExecuteAsync(customerId));
    }

    [Fact]
    public async Task ExecuteAsync_WithAlreadyInactiveCustomer_ThrowsDomainException()
    {
        var customerId = Guid.NewGuid();
        var customer = new CustomerEntity("João Silva", "joao@test.com", "12345678901");
        customer.Deactivate();
        _repoMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(customer);

        await Assert.ThrowsAsync<DomainException>(() => _sut.ExecuteAsync(customerId));

        _repoMock.Verify(r => r.SaveAsync(), Times.Never);
    }
}
