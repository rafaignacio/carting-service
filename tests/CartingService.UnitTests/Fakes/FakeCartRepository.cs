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
            return new CartItemRegistrationFailedException("Cart does not exist on database.");

        var cart =_data[cartId];

        if( cart.Exists( i => i.Id == item.Id) )
            return new CartItemRegistrationFailedException("Item already exists in the cart.");

        cart.Add(item);

        return new Success();
    }

    public OneOf<Success, CartRegistrationFailedException> Register(CartId id)
    {
        if( _data.ContainsKey( id ) ) {
            return new CartRegistrationFailedException("Cart already exists on database.");
        }

        _data.Add(id, new());
        return new Success();
    }

    public List<CartItem> GetById(CartId id) =>
        _data[id];
}