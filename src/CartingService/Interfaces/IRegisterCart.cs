using CartingService.Exceptions;
using CartingService.ValueObjects;
using OneOf;
using OneOf.Types;

namespace CartingService.Interfaces;

public interface IRegisterCart {
    OneOf<Success, CartRegistrationFailedException> Register(CartId Id);
}