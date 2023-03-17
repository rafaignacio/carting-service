using CartingService.Exceptions;
using CartingService.ValueObjects;
using OneOf;
using OneOf.Types;

namespace CartingService.Interfaces;

public interface IAddItemToCart {
    OneOf<Success, CartItemRegistrationFailedException> Add(CartId cartId, CartItem item);
}