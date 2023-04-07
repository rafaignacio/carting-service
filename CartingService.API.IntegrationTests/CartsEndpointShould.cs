using CartingService.API.Models;
using CartingService.ValueObjects;
using FluentAssertions;
using System.Net.Http.Json;

namespace CartingService.API.IntegrationTests;

public class CartsEndpointShould
{
    private const string CartId = "TEST";

    [Test]
    [Order(1)]
    public async Task Add_new_item_successfully()
    {
        var client = TestHttpClientBuilder.CreateClient();
        var model = new AddCartItemRequest
        {
            Id = CartId,
            Item = new(1, "item 1", null, 10, 10)
        };

        var response = await client.PostAsync("/v1/carts",
            JsonContent.Create(model));

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Test]
    [Order(2)]
    public async Task List_all_items_using_v1()
    {
        var client = TestHttpClientBuilder.CreateClient();

        var response = await client.GetAsync($"/v1/carts/{CartId}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<CartResponse>();

        result.Items.Count.Should().Be(1);
    }

    [Test]
    [Order(3)]
    public async Task List_all_items_using_v2()
    {
        var client = TestHttpClientBuilder.CreateClient();

        var response = await client.GetAsync($"/v2/carts/{CartId}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var list = await response.Content.ReadFromJsonAsync<List<CartItem>>();

        list.Count.Should().Be(1);
    }

    [Test]
    [Order(4)]
    public async Task Delete_category_successfully_and_remove_items()
    {
        var client = TestHttpClientBuilder.CreateClient();
        var response = await client.DeleteAsync($"/v1/carts/{CartId}/1");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}
