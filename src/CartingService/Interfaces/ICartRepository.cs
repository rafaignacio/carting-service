using CartingService.Exceptions;
using CartingService.ValueObjects;
using OneOf;
using OneOf.Types;

namespace CartingService.Interfaces;

public interface ICartRepository {
    OneOf<Success, CartItemRegistrationFailedException> AddItem(CartId cartId, CartItem item);
    OneOf<Success, CartRegistrationFailedException> Register(CartId Id);
}