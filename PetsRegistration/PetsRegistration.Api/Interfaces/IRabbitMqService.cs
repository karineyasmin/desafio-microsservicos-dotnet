namespace PetsRegistration.Api.Interfaces;
public interface IRabbitMqService
{
    void Publish<T>(T message, string queueName);
}