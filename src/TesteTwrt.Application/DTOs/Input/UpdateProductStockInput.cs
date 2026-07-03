using FluentValidation;

namespace TesteTwrt.Application.DTOs.Input;

public class UpdateProductStockInput
{
    public int Stock { get; set; }
}

public class UpdateProductStockInputValidator : AbstractValidator<UpdateProductStockInput>
{
    public UpdateProductStockInputValidator()
    {
        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("O estoque não pode ser negativo.");
    }
}
