namespace CartingService.Entities;

using CartingService.Exceptions;
using CartingService.Interfaces;
using CartingService.ValueObjects;
using FluentValidation;
using OneOf;
using OneOf.Types;

public class Cart {
    private List<CartItem> _items;
    private readonly IRegisterCart _registerCart;
    private readonly IAddItemToCart _addItemToCart;
    private readonly IValidator<CartItem> _cartItemValidator;

    public Cart(IRegisterCart registerCart, IAddItemToCart addItemToCart, IValidator<CartItem> cartItemValidator) {
        _registerCart = registerCart;
        _addItemToCart = addItemToCart;
        _cartItemValidator = cartItemValidator;
    }

    public CartId Id { get; private set; }
    public IReadOnlyList<CartItem> Items { 
        get => _items.AsReadOnly(); 
    }

    public OneOf<Success, CartRegistrationFailedException> InitiateWithId(CartId id) {
        var result = _registerCart.Register(id);

        result.Switch(
            _ => Id = id, 
            _ => {});

        return result;
    }

    public OneOf<Success, ValidationFailed, CartItemRegistrationFailedException> AddItem(CartItem item) {
        var validation = _cartItemValidator.Validate(item);

        if(!validation.IsValid)
            return new ValidationFailed(validation.Errors);

        var result = _addItemToCart.Add(this.Id, item);

        if(result.IsT1)
            return (CartItemRegistrationFailedException) result.Value;

        return new Success();
    }
}