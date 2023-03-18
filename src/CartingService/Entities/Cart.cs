namespace CartingService.Entities;

using CartingService.Exceptions;
using CartingService.Interfaces;
using CartingService.ValueObjects;
using FluentValidation;
using OneOf;
using OneOf.Types;

public class Cart {
    private readonly ICartRepository _repository;
    private readonly IValidator<CartItem> _cartItemValidator;

    public Cart(ICartRepository cartRepository, IValidator<CartItem> cartItemValidator) {
        _repository = cartRepository;
        _cartItemValidator = cartItemValidator;
    }

    public OneOf<Success, ValidationFailed, CartItemRegistrationFailedException> AddItem(CartId id, CartItem item) {
        var validation = _cartItemValidator.Validate(item);

        if(!validation.IsValid)
            return new ValidationFailed(validation.Errors);

        var result = _repository.AddItem(id, item);

        if(result.IsT1)
            return (CartItemRegistrationFailedException) result.Value;

        return new Success();
    }
}