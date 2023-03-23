using CartingService.ValueObjects;
using FluentAssertions;

namespace CartingService.UnitTests;

public class CartIdShould
{
    [Fact]
    public void Create_implicitly_from_guid_formatted_string() {
        var sut = () => (CartId) "28b6f374-06cc-4029-81c0-ca1210a28909";

        sut.Should().NotThrow();
    }

}