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

    public async Task<OneOf<List<CartItem>, NotFound>> GetItemsByCartId(CartId id) {
        var cart = await _repository.GetById(id);

        if( cart == null || cart.Count == 0 )
            return new NotFound();

        return cart;
    }

    public async Task<OneOf<Success, ValidationFailed, CartItemRegistrationFailedException>> AddItem(CartId id, CartItem item) {
        var validation = _cartItemValidator.Validate(item);

        if(!validation.IsValid)
            return new ValidationFailed(validation.Errors);

        if( (await _repository.GetCartItemById(id, item.Id)) != null)
            return new CartItemRegistrationFailedException("Item already exists in the cart.");

        try {
            await _repository.AddItem(id, item);

            return new Success();
        } catch( Exception e ) {
            return new CartItemRegistrationFailedException(e.Message);
        }
    }

    public async Task<OneOf<Success, NotFound, CartItemRemovalException>> DeleteItem(CartId cartId, int itemId) {
        if( (await _repository.GetCartItemById(cartId, itemId)) == null )
            return new NotFound();

        try {
            await _repository.DeleteItem(cartId, itemId);
            return new Success();
        } catch( Exception e ) {
            return new CartItemRemovalException(e.Message);
        }
    }
}