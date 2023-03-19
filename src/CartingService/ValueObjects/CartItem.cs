namespace CartingService.ValueObjects;

public class CartItem {
    public int Id { get; set; } 
    public string Name { get; set; } 
    public CartImage? Image { get; set; } 
    public decimal Price { get; set; } 
    public int Quantity { get; set; }
    public CartItem() {}

    public CartItem(int id, string name, CartImage? image, decimal price, int quantity) {
        Id = id;
        Name = name;
        Image = image;
        Price = price;
        Quantity = quantity;
    }
}