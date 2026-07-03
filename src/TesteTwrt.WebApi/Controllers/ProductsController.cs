using Microsoft.AspNetCore.Mvc;
using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Application.UseCases.Product.CreateProduct;
using TesteTwrt.Application.UseCases.Product.GetProductById;
using TesteTwrt.Application.UseCases.Product.GetAllProducts;
using TesteTwrt.Application.UseCases.Product.UpdateProduct;
using TesteTwrt.Application.UseCases.Product.UpdateProductPrice;
using TesteTwrt.Application.UseCases.Product.UpdateProductStock;
using TesteTwrt.Application.UseCases.Product.ActivateProduct;
using TesteTwrt.Application.UseCases.Product.DeactivateProduct;

namespace TesteTwrt.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ICreateProductUseCase _create;
    private readonly IGetProductByIdUseCase _getById;
    private readonly IGetAllProductsUseCase _getAll;
    private readonly IUpdateProductUseCase _update;
    private readonly IUpdateProductPriceUseCase _updatePrice;
    private readonly IUpdateProductStockUseCase _updateStock;
    private readonly IActivateProductUseCase _activate;
    private readonly IDeactivateProductUseCase _deactivate;

    public ProductsController(
        ICreateProductUseCase create,
        IGetProductByIdUseCase getById,
        IGetAllProductsUseCase getAll,
        IUpdateProductUseCase update,
        IUpdateProductPriceUseCase updatePrice,
        IUpdateProductStockUseCase updateStock,
        IActivateProductUseCase activate,
        IDeactivateProductUseCase deactivate)
    {
        _create = create;
        _getById = getById;
        _getAll = getAll;
        _update = update;
        _updatePrice = updatePrice;
        _updateStock = updateStock;
        _activate = activate;
        _deactivate = deactivate;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductInput input)
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

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductInput input)
    {
        var result = await _update.ExecuteAsync(id, input);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/price")]
    public async Task<IActionResult> UpdatePrice(Guid id, [FromBody] UpdateProductPriceInput input)
    {
        await _updatePrice.ExecuteAsync(id, input);
        return NoContent();
    }

    [HttpPatch("{id:guid}/stock")]
    public async Task<IActionResult> UpdateStock(Guid id, [FromBody] UpdateProductStockInput input)
    {
        await _updateStock.ExecuteAsync(id, input);
        return NoContent();
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        await _activate.ExecuteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        await _deactivate.ExecuteAsync(id);
        return NoContent();
    }
}
