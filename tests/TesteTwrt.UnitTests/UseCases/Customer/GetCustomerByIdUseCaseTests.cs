using Moq;
using Xunit;
using CustomerEntity = TesteTwrt.Domain.Entities.Customer;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;
using TesteTwrt.Application.UseCases.Customer.GetCustomerById;

namespace TesteTwrt.UnitTests.UseCases.Customer;

public class GetCustomerByIdUseCaseTests
{
    private readonly Mock<ICustomerRepository> _repoMock = new();
    private readonly GetCustomerByIdUseCase _sut;

    public GetCustomerByIdUseCaseTests()
    {
        _sut = new GetCustomerByIdUseCase(_repoMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithExistingId_ReturnsCustomerOutput()
    {
        var customerId = Guid.NewGuid();
        var customer = new CustomerEntity("Maria Oliveira", "maria@test.com", "98765432100");
        _repoMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(customer);

        var result = await _sut.ExecuteAsync(customerId);

        Assert.Equal("Maria Oliveira", result.Name);
        Assert.Equal("maria@test.com", result.Email);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistingId_ThrowsNotFoundException()
    {
        var customerId = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync((CustomerEntity?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.ExecuteAsync(customerId));
    }
}
