using CartingService.ValueObjects;

namespace CartingService.API.Models;

public class AddCartItemRequest
{
    public AddCartItemRequest() { }
    public string Id { get; set; } = string.Empty;
    public CartItem Item { get; set; } = new();
}
