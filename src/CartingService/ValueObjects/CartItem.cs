namespace CartingService.ValueObjects;

public record CartItem(int Id, string Name, CartImage? Image, decimal Price, int Quantity);