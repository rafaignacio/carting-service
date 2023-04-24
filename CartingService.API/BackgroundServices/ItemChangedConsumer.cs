using CartingService.API.Configurations;
using CartingService.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CartingService.API.BackgroundServices
{
    public class ItemChangedConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly ICartRepository _repository;

        public ItemChangedConsumer(IOptions<QueueConfiguration> queueConfig, ICartRepository repository)
        {
            var factory = new ConnectionFactory() { HostName = queueConfig.Value.ConnectionString };
            _connection = factory.CreateConnection();
            _repository = repository;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () => {
                using var channel = _connection.CreateModel();
                channel.QueueDeclare("item-changes-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) => {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var itemChangedMessage = JsonSerializer.Deserialize<ItemChangedMessage>(message);

                        switch(ea.Exchange)
                        {
                            case "ItemUpdated":
                                _repository.UpdateItemsData(itemChangedMessage.Id, itemChangedMessage.Name, itemChangedMessage.Price);
                                break;
                            case "ItemDeleted":
                                _repository.RemoveItemFromCarts(itemChangedMessage.Id);
                                break;
                        };

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
                        } catch { }
                    }
                };

                while(!stoppingToken.IsCancellationRequested)
                {
                    channel.BasicConsume(queue: "item-changes-queue", autoAck: false, consumer: consumer);
                    await Task.Delay(1000);
                }
            }, stoppingToken);
        }
    }
}
