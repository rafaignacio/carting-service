namespace CartingService.Exceptions;

public class CartItemRemovalException : Exception {
    public CartItemRemovalException() : base() {

    }

    public CartItemRemovalException(string message) : base(message) {
        
    }
}