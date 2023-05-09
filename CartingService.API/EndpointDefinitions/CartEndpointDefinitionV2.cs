using CartingService.API.Core;
using CartingService.Entities;
using CartingService.Interfaces;
using CartingService.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace CartingService.API.EndpointDefinitions;

public class CartEndpointDefinitionV2 : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/v2/carts/{id}", GetCartById)
            .RequireAuthorization("default")
            .WithGroupName("v2")
            .WithOpenApi(opt =>
            {
                opt.Summary = "Return items from cart";
                return opt;
            })
            .Produces<List<CartItem>>(200)
            .Produces(404); ;
    }

    public void DefineServices(IServiceCollection services)
    {
    }

    private static async Task<IResult> GetCartById([FromRoute] string id, [FromServices] ICartRepository repository)
    {
        var cart = new Cart(repository, null);
        var result = await cart.GetItemsByCartId(id);

        return result.Match(
            list => Results.Ok(list),
            _ => Results.NotFound());
    }
}
