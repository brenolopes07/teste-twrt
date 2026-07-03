using Moq;
using Xunit;
using CustomerEntity = TesteTwrt.Domain.Entities.Customer;
using TesteTwrt.Domain.Interfaces.Repositories;
using TesteTwrt.Application.UseCases.Customer.GetAllCustomers;

namespace TesteTwrt.UnitTests.UseCases.Customer;

public class GetAllCustomersUseCaseTests
{
    private readonly Mock<ICustomerRepository> _repoMock = new();
    private readonly GetAllCustomersUseCase _sut;

    public GetAllCustomersUseCaseTests()
    {
        _sut = new GetAllCustomersUseCase(_repoMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsAllCustomers()
    {
        var customers = new List<CustomerEntity>
        {
            new CustomerEntity("Cliente A", "a@test.com", "11111111111"),
            new CustomerEntity("Cliente B", "b@test.com", "22222222222"),
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);

        var result = await _sut.ExecuteAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ExecuteAsync_WithNoCustomers_ReturnsEmptyList()
    {
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<CustomerEntity>());

        var result = await _sut.ExecuteAsync();

        Assert.Empty(result);
    }
}
