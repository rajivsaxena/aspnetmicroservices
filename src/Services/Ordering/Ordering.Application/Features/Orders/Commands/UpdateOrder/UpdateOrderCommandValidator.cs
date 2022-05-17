using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
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
