using CartingService.Repositories;
using CartingService.ValueObjects;
using FluentAssertions;

namespace CartingService.UnitTests;

public class CartRepositoryShould {
    [Fact]
    public async Task Insert_retrieve_and_delete_item_to_cart() {
        var sut = new CartRepository();
        var id = new CartId();

        var cartItem = new CartItem(1, "Mouse", null, 1.5M, 1);

        await sut.AddItem(id, cartItem);
        var cart = await sut.GetById(id);

        cart.Should().ContainEquivalentOf(cartItem);

        await sut.DeleteItem(id, 1);
        cart = await sut.GetById(id);

        cart.Count.Should().Be(0);
    }
}