using CartingService.Interfaces;
using CartingService.Models;
using CartingService.ValueObjects;
using LiteDB;
namespace CartingService.Repositories;

public class CartRepository : ICartRepository
{
    private const string ConnectionString = "./cart.db";

    public Task AddItem(CartId cartId, CartItem item)
    {
        using var db = new LiteDatabase(ConnectionString);
        var cartCollection = db.GetCollection<CartModel>("carts");

        var cartModel = cartCollection.FindById(new BsonValue((string)cartId));

        if(cartModel == null) {
            cartModel = new( cartId, new(){ item } );
        } else {
            cartModel.Items.Add(item);
        }

        cartCollection.Upsert(new BsonValue((string)cartId), cartModel);

        return Task.CompletedTask;
    }

    public Task DeleteItem(CartId cartId, int itemId)
    {
        using var db = new LiteDatabase(ConnectionString);
        var cartCollection = db.GetCollection<CartModel>("carts");
        var cartModel = cartCollection.FindById(new BsonValue((string)cartId));

        cartModel.Items.RemoveAll( i => i.Id == itemId );
        cartCollection.Update(new BsonValue((string)cartId), cartModel);

        return Task.CompletedTask;
    }

    public Task<List<CartItem>> GetById(CartId id)
    {
        using var db = new LiteDatabase(ConnectionString);
        var cartCollection = db.GetCollection<CartModel>("carts");
        var cart = cartCollection.FindById(new BsonValue((string)id));

        if( cart == null )
            return Task.FromResult(new List<CartItem>());

        return Task.FromResult( cart.Items );
    }

    public Task<CartItem?> GetCartItemById(CartId cartId, int id)
    {
        var cart = GetById(cartId).Result;

        if(cart == null || cart.Count == 0)
            return Task.FromResult<CartItem?>(null);

        return Task.FromResult(cart.SingleOrDefault(i => i.Id == id));
    }

    public Task RemoveItemFromCarts(int itemId)
    {
        using var db = new LiteDatabase(ConnectionString);
        var cartCollection = db.GetCollection<CartModel>("carts");

        var carts = cartCollection.Find(c =>
            c.Items.Where(i => i.Id == itemId).Count() > 0);

        foreach (var cart in carts)
        {
            var item = cart.Items.First(i => i.Id == itemId);
            cart.Items.Remove(item);
            
            cartCollection.Update(cart);
        }

        return Task.CompletedTask;
    }

    public Task UpdateItemsData(int id, string name, decimal price)
    {
        using var db = new LiteDatabase(ConnectionString);
        var cartCollection = db.GetCollection<CartModel>("carts");

        var carts = cartCollection.Find(c => 
            c.Items.Where(i => i.Id == id).Count() > 0);
        
        foreach(var cart in carts)
        {
            var item = cart.Items.First(i => i.Id == id);
            item.Name = name;
            item.Price = price;

            cartCollection.Update(cart);
        }

        return Task.CompletedTask;
    }
}