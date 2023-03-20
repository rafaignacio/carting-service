namespace CartingService.Exceptions;

public class CartRegistrationFailedException : Exception {
    public CartRegistrationFailedException() : base() {

    }

    public CartRegistrationFailedException(string message) : base(message) {
    }
}