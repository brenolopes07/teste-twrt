using FluentValidation;

namespace TesteTwrt.Application.DTOs.Input;

public class UpdateProductPriceInput
{
    public decimal Price { get; set; }
}

public class UpdateProductPriceInputValidator : AbstractValidator<UpdateProductPriceInput>
{
    public UpdateProductPriceInputValidator()
    {
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("O preço deve ser maior que zero.");
    }
}
