using FluentValidation;

namespace TesteTwrt.Application.DTOs.Input;

public class CreateCustomerInput
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
}

public class CreateCustomerInputValidator : AbstractValidator<CreateCustomerInput>
{
    public CreateCustomerInputValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do cliente é obrigatório.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail informado não possui formato válido.");

        RuleFor(x => x.Document)
            .NotEmpty().WithMessage("O documento é obrigatório.")
            .Must(doc =>
            {
                var digits = new string(doc.Where(char.IsDigit).ToArray());
                return digits.Length == 11 || digits.Length == 14;
            })
            .WithMessage("O documento deve ser um CPF (11 dígitos) ou CNPJ (14 dígitos).");
    }
}

