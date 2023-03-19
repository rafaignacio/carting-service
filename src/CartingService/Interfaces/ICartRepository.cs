using CartingService.ValueObjects;

namespace CartingService.Interfaces;

public interface ICartRepository {
    Task AddItem(CartId cartId, CartItem item);
    Task<List<CartItem>> GetById(CartId id);
    Task<CartItem?> GetCartItemById(CartId cartId, int id);
    Task DeleteItem(CartId cartId, int itemId);
}