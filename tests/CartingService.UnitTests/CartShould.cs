using CartingService.Entities;
using CartingService.Interfaces;
using CartingService.Validators;
using CartingService.ValueObjects;
using FluentAssertions;
using Moq;
using OneOf.Types;

namespace CartingService.UnitTests;

public class CartShould {
    [Fact]
    public void Be_successfully_initiated_and_contain_one_item() {
        var registerCartMock = new Mock<IRegisterCart>();
        var addItemToCartMock = new Mock<IAddItemToCart>();

        registerCartMock.Setup(x => x.Register(It.IsAny<CartId>()))
            .Returns(new Success());

        addItemToCartMock.Setup(x => x.Add(It.IsAny<CartId>(), It.IsAny<CartItem>()))
            .Returns(new Success());

        var sut = new Cart( registerCartMock.Object, addItemToCartMock.Object, new CartItemValidator() );
        var id = new CartId();

        sut.InitiateWithId(id).Switch( 
            _ => sut.Id.Should().Be(id),
            _ => throw new Exception() );

        sut.AddItem(new CartItem(1, "Mouse", null, 1.0M, 1)).Match<bool>(
            _ => true,
            _ => false,
            _ => false
        ).Should().BeTrue();
    }
}