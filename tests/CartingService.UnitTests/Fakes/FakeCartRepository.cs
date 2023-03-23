using CartingService.Interfaces;
using CartingService.ValueObjects;

namespace CartingService.UnitTests.Fakes;

public record FakeCartRepository : ICartRepository
{
    private Dictionary<CartId, List<CartItem>> _data = new();
    private bool _throw_exception_add_item = false;
    private bool _throw_exception_remove_item = false;

    public FakeCartRepository ThrowExceptionWhenCallingAddItem() => 
        this with {
            _throw_exception_add_item = true
        };

    public FakeCartRepository ThrowExceptionWhenCallingRemoveItem() => 
        this with {
            _throw_exception_remove_item = true
        };

    private bool IsCartRegistered(CartId id) => 
        _data.ContainsKey( id );

    private void RegisterCartIfNotPresent(CartId id) {
        if( !IsCartRegistered( id ) )
            _data.Add(id, new());
    }

    public Task AddItem(CartId cartId, CartItem item)
    {
        if(_throw_exception_add_item)
            throw new Exception("Failed to add item.");

        RegisterCartIfNotPresent(cartId);

        var cart = _data[cartId];
        cart.Add(item);

        return Task.CompletedTask;
    }

    public Task DeleteItem(CartId cartId, int itemId)
    {
        if(_throw_exception_remove_item)
            throw new Exception("Failed to remove item.");

        RegisterCartIfNotPresent(cartId);
        _data[cartId].RemoveAll( item => item.Id == itemId );

        return Task.CompletedTask;
    }

    public Task<List<CartItem>> GetById(CartId id)
    {
        RegisterCartIfNotPresent(id);

        return Task.FromResult(_data[id]);
    }

    public Task<CartItem?> GetCartItemById(CartId cartId, int id)
    {
        RegisterCartIfNotPresent(cartId);

        return Task.FromResult( _data[cartId].SingleOrDefault( i => i.Id == id ) );
    }
}