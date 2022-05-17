using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(r => r.UserName)
                .NotEmpty().WithMessage("{UserName is required.}")
                .NotNull()
                .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters");
            RuleFor(r => r.EmailAddress)
                .NotNull()
                .NotEmpty().WithMessage("{EmailAddress} is required");
            RuleFor(r => r.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} is required.")
                .GreaterThan(0).WithMessage("{TotalPrice} must be greater than zero.");

        }
    }
}
