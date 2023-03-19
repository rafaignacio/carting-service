using CartingService.Interfaces;
using CartingService.ValueObjects;

namespace CartingService.UnitTests.Fakes;

public record FakeCartRepository : ICartRepository
{
    private Dictionary<CartId, List<CartItem>> _data = new();

    private bool IsCartRegistered(CartId id) => 
        _data.ContainsKey( id );

    public Task AddItem(CartId cartId, CartItem item)
    {
        if( !IsCartRegistered( cartId ) )
            _data.Add(cartId, new());

        var cart = _data[cartId];
        cart.Add(item);

        return Task.CompletedTask;
    }

    public Task Delete(CartId cartId, int itemId)
    {
        _data[cartId].RemoveAll( item => item.Id == itemId );

        return Task.CompletedTask;
    }

    public Task<List<CartItem>> GetById(CartId id)
    {
        if( !IsCartRegistered( id ) )
            _data.Add(id, new());

        return Task.FromResult(_data[id]);
    }

    public Task<CartItem?> GetCartItemById(CartId cartId, int id)
    {
        if( !IsCartRegistered( cartId ) )
            _data.Add(cartId, new());

        return Task.FromResult( _data[cartId].SingleOrDefault( i => i.Id == id ) );
    }
}