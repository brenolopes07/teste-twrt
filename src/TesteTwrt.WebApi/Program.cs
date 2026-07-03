using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TesteTwrt.Application.DTOs.Input;
using TesteTwrt.Application.UseCases.Customer.CreateCustomer;
using TesteTwrt.Application.UseCases.Customer.GetCustomerById;
using TesteTwrt.Application.UseCases.Customer.GetAllCustomers;
using TesteTwrt.Application.UseCases.Customer.DeactivateCustomer;
using TesteTwrt.Application.UseCases.Product.CreateProduct;
using TesteTwrt.Application.UseCases.Product.GetProductById;
using TesteTwrt.Application.UseCases.Product.GetAllProducts;
using TesteTwrt.Application.UseCases.Product.UpdateProduct;
using TesteTwrt.Application.UseCases.Product.UpdateProductPrice;
using TesteTwrt.Application.UseCases.Product.UpdateProductStock;
using TesteTwrt.Application.UseCases.Product.ActivateProduct;
using TesteTwrt.Application.UseCases.Product.DeactivateProduct;
using TesteTwrt.Application.UseCases.Order.CreateOrder;
using TesteTwrt.Application.UseCases.Order.GetOrderById;
using TesteTwrt.Application.UseCases.Order.GetAllOrders;
using TesteTwrt.Application.UseCases.Order.ChangeOrderStatus;
using TesteTwrt.Domain.Exceptions;
using TesteTwrt.Domain.Interfaces.Repositories;
using TesteTwrt.Infrastructure.Data;
using TesteTwrt.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<ICreateCustomerUseCase, CreateCustomerUseCase>();
builder.Services.AddScoped<IGetCustomerByIdUseCase, GetCustomerByIdUseCase>();
builder.Services.AddScoped<IGetAllCustomersUseCase, GetAllCustomersUseCase>();
builder.Services.AddScoped<IDeactivateCustomerUseCase, DeactivateCustomerUseCase>();

builder.Services.AddScoped<ICreateProductUseCase, CreateProductUseCase>();
builder.Services.AddScoped<IGetProductByIdUseCase, GetProductByIdUseCase>();
builder.Services.AddScoped<IGetAllProductsUseCase, GetAllProductsUseCase>();
builder.Services.AddScoped<IUpdateProductUseCase, UpdateProductUseCase>();
builder.Services.AddScoped<IUpdateProductPriceUseCase, UpdateProductPriceUseCase>();
builder.Services.AddScoped<IUpdateProductStockUseCase, UpdateProductStockUseCase>();
builder.Services.AddScoped<IActivateProductUseCase, ActivateProductUseCase>();
builder.Services.AddScoped<IDeactivateProductUseCase, DeactivateProductUseCase>();

builder.Services.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>();
builder.Services.AddScoped<IGetOrderByIdUseCase, GetOrderByIdUseCase>();
builder.Services.AddScoped<IGetAllOrdersUseCase, GetAllOrdersUseCase>();
builder.Services.AddScoped<IChangeOrderStatusUseCase, ChangeOrderStatusUseCase>();

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerInput>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var feature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        var exception = feature?.Error;

        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            NotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
            DomainException ex   => (StatusCodes.Status400BadRequest, ex.Message),
            _                    => (StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.")
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(new { error = message });
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
