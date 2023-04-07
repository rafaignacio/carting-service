using CartingService.API.Core;
using CartingService.API.Models;
using CartingService.Entities;
using CartingService.Exceptions;
using CartingService.Interfaces;
using CartingService.Repositories;
using CartingService.Validators;
using CartingService.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CartingService.API.EndpointDefinitions;

public class CartEndpointDefinitionV1 : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/v1/carts/{id}", GetCartById)
            .WithGroupName("v1")
            .WithOpenApi( opt =>
            {
                opt.Summary = "Return cart with items";
                return opt;
            })
            .Produces<CartResponse>(200)
            .Produces(404);
        app.MapPost("/v1/carts", AddItemToCart)
            .Accepts<AddCartItemRequest>("application/json")
            .WithGroupName("v1")
            .WithOpenApi(opt =>
            {
                opt.Summary = "Add item to cart, if cart does not exist, creates it.";
                return opt;
            })
            .Produces(200)
            .Produces<ValidationFailed>(400)
            .Produces<ProblemDetails>(500);
        app.MapDelete("/v1/carts/{id}/{itemId}", DeleteItemFromCart)
            .WithGroupName("v1")
            .WithOpenApi(opt =>
                {
                    opt.Summary = "Delete item from cart";
                    return opt;
                })
            .Produces(200)
            .Produces<ValidationFailed>(400)
            .Produces<ProblemDetails>(500);
    }

    private static async Task<IResult> GetCartById([FromRoute] string id, [FromServices] ICartRepository repository)
    {
        var cart = new Cart(repository, null);
        var result = await cart.GetItemsByCartId(id);

        return result.Match(
            list => Results.Ok(new CartResponse(id, list)),
            _ => Results.NotFound());
    }

    private static async Task<IResult> AddItemToCart([FromBody] AddCartItemRequest body, [FromServices] ICartRepository repository, [FromServices] IValidator<CartItem> validator)
    {
        var cart = new Cart(repository, validator);
        var result = await cart.AddItem(body.Id, body.Item);

        return result.Match(
            _ => Results.Ok(),
            errors => Results.BadRequest(errors),
            ex => Results.Problem( ex.Message ));
    }

    private static async Task<IResult> DeleteItemFromCart([FromRoute] string id, [FromRoute] int itemId, [FromServices] ICartRepository repository)
    {
        var cart = new Cart(repository, null);
        var result = await cart.DeleteItem(id, itemId);

        return result.Match(
            _ => Results.Ok(),
            _ => Results.NotFound(),
            ex => Results.Problem( ex.Message ));
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddSingleton<ICartRepository, CartRepository>();
        services.AddSingleton<IValidator<CartItem>, CartItemValidator>();
    }
}
