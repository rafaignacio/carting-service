using CartingService.Entities;
using CartingService.UnitTests.Fakes;
using CartingService.Validators;
using CartingService.ValueObjects;
using FluentAssertions;

namespace CartingService.UnitTests;

public class CartShould {
    [Fact]
    public async Task Successfully_add_item() {
        var repository = new FakeCartRepository();
        var sut = new Cart( repository, new CartItemValidator() );
        var id = new CartId();

        var addItemResult = await sut.AddItem(id, new CartItem(1, "Mouse", null, 1.0M, 1));
        addItemResult.Match<bool>(
            _ => true,
            _ => false,
            _ => false ).Should().BeTrue();

        var cart = await sut.GetById(id);
        cart.Match(
            list => list.Count,
            _ => 0 ).Should().Be(1);
    }

    [Fact]
    public async Task Successfully_remove_item() {
        var repository = new FakeCartRepository();
        var sut = new Cart( repository, new CartItemValidator() );
        var id = new CartId();

        var addItemResult = await sut.AddItem(id, new CartItem(1, "Mouse", null, 1.0M, 1));
        addItemResult.Match<bool>(
            _ => true,
            _ => false,
            _ => false ).Should().BeTrue();

        await sut.DeleteItem(id, 1);

        var cartResult = await sut.GetById(id);
        cartResult.Match(
            list => list.Count,
            _ => 0 ).Should().Be(0);
    }

    [Fact]
    public async Task Indicate_errors_when_item_does_not_have_required_fields() {
        var repository = new FakeCartRepository();
        var sut = new Cart( repository, new CartItemValidator() );

        var addItemResult = await sut.AddItem(new CartId(), new CartItem(0, string.Empty, null, 0.0M, 0));
        addItemResult.Match<int>(
            _ => 0,
            errors => errors.Errors.Count(),
            _ => 0 ).Should().Be(4);
    }
}