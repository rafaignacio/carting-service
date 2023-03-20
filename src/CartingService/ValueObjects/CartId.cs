namespace CartingService.ValueObjects;

public record struct CartId {
    public CartId() => Id = Guid.NewGuid();
    private Guid Id;
    public static implicit operator CartId(string id) => 
        new CartId() {
            Id = Guid.Parse(
                new ReadOnlySpan<char>( id.ToCharArray() ) 
            )
        };

    public static implicit operator CartId(Guid id) => 
        new CartId() {
            Id = id
        };

    public static implicit operator string(CartId id) => id.Id.ToString("N");

    public static implicit operator Guid(CartId id) => id.Id;
}