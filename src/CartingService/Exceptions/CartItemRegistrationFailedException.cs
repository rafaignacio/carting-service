namespace CartingService.Exceptions;

public class CartItemRegistrationFailedException : Exception {
    public CartItemRegistrationFailedException() : base() {

    }

    public CartItemRegistrationFailedException(string message) : base(message) {
        
    }
}