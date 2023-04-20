namespace CartingService.API.BackgroundServices;

public class ItemChangedMessage
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public long Amount { get; set; }
}
