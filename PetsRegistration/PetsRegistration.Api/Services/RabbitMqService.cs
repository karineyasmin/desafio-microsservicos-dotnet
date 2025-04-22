using System.Text;
using System.Text.Json;
using PetsRegistration.Api.Interfaces;
using RabbitMQ.Client;

namespace PetsRegistration.Api.Services;
public class RabbitMqService : IRabbitMqService
{
    private readonly ConnectionFactory _factory;

    public RabbitMqService()
    {
        _factory = new ConnectionFactory() { HostName = "localhost" };
    }

    public void Publish<T>(T message, string queueName)
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
    }
}