using CartingService.Entities;
using CartingService.UnitTests.Fakes;
using CartingService.Validators;
using CartingService.ValueObjects;
using FluentAssertions;

namespace CartingService.UnitTests;

public class CartShould {
    [Fact]
    public void Be_successfully_initiated_and_contain_one_item() {
        var repository = new FakeCartRepository();
        var sut = new Cart( repository, new CartItemValidator() );
        var id = new CartId();

        sut.AddItem(id, new CartItem(1, "Mouse", null, 1.0M, 1)).Match<bool>(
            _ => true,
            _ => false,
            _ => false
        ).Should().BeTrue();

        repository.GetById(id).Match(
            list => list.Count,
            _ => 0
        ).Should().Be(1);
    }
}