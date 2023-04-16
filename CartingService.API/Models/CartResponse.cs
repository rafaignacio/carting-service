using CartingService.ValueObjects;

namespace CartingService.API.Models;

public record CartResponse(string Id, List<CartItem> Items);
