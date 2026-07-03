using FluentValidation;

namespace TesteTwrt.Application.DTOs.Input;

public class UpdateProductInput
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateProductInputValidator : AbstractValidator<UpdateProductInput>
{
    public UpdateProductInputValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do produto é obrigatório.");
    }
}
