using Microsoft.AspNetCore.Mvc;
using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Application.UseCases.Order.CreateOrder;
using TesteTwrt.Application.UseCases.Order.GetOrderById;
using TesteTwrt.Application.UseCases.Order.GetAllOrders;
using TesteTwrt.Application.UseCases.Order.ChangeOrderStatus;

namespace TesteTwrt.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ICreateOrderUseCase _create;
    private readonly IGetOrderByIdUseCase _getById;
    private readonly IGetAllOrdersUseCase _getAll;
    private readonly IChangeOrderStatusUseCase _changeStatus;

    public OrdersController(
        ICreateOrderUseCase create,
        IGetOrderByIdUseCase getById,
        IGetAllOrdersUseCase getAll,
        IChangeOrderStatusUseCase changeStatus)
    {
        _create = create;
        _getById = getById;
        _getAll = getAll;
        _changeStatus = changeStatus;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderInput input)
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

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeOrderStatusInput input)
    {
        await _changeStatus.ExecuteAsync(id, input);
        return NoContent();
    }
}
