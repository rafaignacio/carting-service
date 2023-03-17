using CartingService.ValueObjects;
using FluentValidation;

namespace CartingService.Validators;

public class CartItemValidator : AbstractValidator<CartItem> {
    public CartItemValidator() {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.Quantity)
            .GreaterThan(0);
    }
}