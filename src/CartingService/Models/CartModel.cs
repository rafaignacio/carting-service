using CartingService.ValueObjects;

namespace CartingService.Models;

public class CartModel {
    public string Id { get; set; }
    public List<CartItem> Items { get; set; }

    public CartModel(string id, List<CartItem> items) {
        Id = id;
        Items = items;
    }

    public CartModel() {
        Items = new();
    }

}