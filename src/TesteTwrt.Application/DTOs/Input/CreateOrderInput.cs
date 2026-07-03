using FluentValidation;

namespace TesteTwrt.Application.DTOs.Input;

public class CreateOrderInput
{
    public Guid CustomerId { get; set; }
    public IList<CreateOrderItemInput> Items { get; set; } = new List<CreateOrderItemInput>();
}

public class CreateOrderItemInput
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class CreateOrderInputValidator : AbstractValidator<CreateOrderInput>
{
    public CreateOrderInputValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("O ID do cliente é obrigatório.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("O pedido deve possuir ao menos um item.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId)
                .NotEmpty().WithMessage("O ID do produto é obrigatório.");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("A quantidade de cada item deve ser maior que zero.");
        });
    }
}
