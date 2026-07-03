using Moq;
using Xunit;
using CustomerEntity = TesteTwrt.Domain.Entities.Customer;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;
using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Application.UseCases.Customer.CreateCustomer;

namespace TesteTwrt.UnitTests.UseCases.Customer;

public class CreateCustomerUseCaseTests
{
    private readonly Mock<ICustomerRepository> _repoMock = new();
    private readonly CreateCustomerUseCase _sut;

    public CreateCustomerUseCaseTests()
    {
        _sut = new CreateCustomerUseCase(_repoMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidInput_CreatesAndReturnsCustomer()
    {
        var input = new CreateCustomerInput { Name = "João Silva", Email = "joao@test.com", Document = "123.456.789-09" };
        _repoMock.Setup(r => r.ExistsActiveWithEmailAsync(input.Email, It.IsAny<Guid?>())).ReturnsAsync(false);
        _repoMock.Setup(r => r.ExistsActiveWithDocumentAsync("12345678909", It.IsAny<Guid?>())).ReturnsAsync(false);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<CustomerEntity>())).Returns(Task.CompletedTask);

        var result = await _sut.ExecuteAsync(input);

        Assert.Equal("João Silva", result.Name);
        Assert.Equal("joao@test.com", result.Email);
        Assert.Equal("12345678909", result.Document);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<CustomerEntity>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithDuplicateEmail_ThrowsDomainException()
    {
        var input = new CreateCustomerInput { Name = "João Silva", Email = "joao@test.com", Document = "12345678901" };
        _repoMock.Setup(r => r.ExistsActiveWithEmailAsync(input.Email, It.IsAny<Guid?>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<DomainException>(() => _sut.ExecuteAsync(input));

        _repoMock.Verify(r => r.AddAsync(It.IsAny<CustomerEntity>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WithDuplicateDocument_ThrowsDomainException()
    {
        var input = new CreateCustomerInput { Name = "João Silva", Email = "joao@test.com", Document = "12345678901" };
        _repoMock.Setup(r => r.ExistsActiveWithEmailAsync(input.Email, It.IsAny<Guid?>())).ReturnsAsync(false);
        _repoMock.Setup(r => r.ExistsActiveWithDocumentAsync("12345678901", It.IsAny<Guid?>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<DomainException>(() => _sut.ExecuteAsync(input));

        _repoMock.Verify(r => r.AddAsync(It.IsAny<CustomerEntity>()), Times.Never);
    }
}
