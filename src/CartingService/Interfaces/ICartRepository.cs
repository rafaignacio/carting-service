using CartingService.Exceptions;
using CartingService.ValueObjects;
using OneOf;
using OneOf.Types;

namespace CartingService.Interfaces;

public interface ICartRepository {
    OneOf<Success, CartItemRegistrationFailedException> AddItem(CartId cartId, CartItem item);
    OneOf<List<CartItem>, None> GetById(CartId cartId);
}