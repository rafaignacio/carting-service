namespace CartingService.ValueObjects;

public record struct CartId {
    public CartId() => Id = Guid.NewGuid().ToString();
    private string Id;
    public static implicit operator CartId(string id) => 
        new CartId() {
            Id = id
        };

    public static implicit operator string(CartId id) => id.Id;

}