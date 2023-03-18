using CartingService.Exceptions;
using CartingService.Interfaces;
using CartingService.ValueObjects;
using OneOf;
using OneOf.Types;

namespace CartingService.UnitTests.Fakes;

public record FakeCartRepository : ICartRepository
{
    private Dictionary<CartId, List<CartItem>> _data = new();

    public OneOf<Success, CartItemRegistrationFailedException> AddItem(CartId cartId, CartItem item)
    {
        if( !_data.ContainsKey( cartId ) )
            _data.Add(cartId, new());

        var cart =_data[cartId];

        if( cart.Exists( i => i.Id == item.Id) )
            return new CartItemRegistrationFailedException("Item already exists in the cart.");

        cart.Add(item);

        return new Success();
    }

    public OneOf<List<CartItem>, None> GetById(CartId cartId)
    {
        if( !_data.ContainsKey( cartId ) )
            return new None();

        return _data[cartId];
    }
}