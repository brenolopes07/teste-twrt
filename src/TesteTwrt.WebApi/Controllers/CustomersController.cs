using Microsoft.AspNetCore.Mvc;
using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Application.UseCases.Customer.CreateCustomer;
using TesteTwrt.Application.UseCases.Customer.GetCustomerById;
using TesteTwrt.Application.UseCases.Customer.GetAllCustomers;
using TesteTwrt.Application.UseCases.Customer.DeactivateCustomer;

namespace TesteTwrt.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICreateCustomerUseCase _create;
    private readonly IGetCustomerByIdUseCase _getById;
    private readonly IGetAllCustomersUseCase _getAll;
    private readonly IDeactivateCustomerUseCase _deactivate;

    public CustomersController(
        ICreateCustomerUseCase create,
        IGetCustomerByIdUseCase getById,
        IGetAllCustomersUseCase getAll,
        IDeactivateCustomerUseCase deactivate)
    {
        _create = create;
        _getById = getById;
        _getAll = getAll;
        _deactivate = deactivate;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerInput input)
    {
        var result = await _create.ExecuteAsync(input);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getById.ExecuteAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getAll.ExecuteAsync();
        return Ok(result);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        await _deactivate.ExecuteAsync(id);
        return NoContent();
    }
}
