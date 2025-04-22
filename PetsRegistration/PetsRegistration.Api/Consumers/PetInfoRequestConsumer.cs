using System.Text.Json;
using PetsRegistration.Api.Interfaces;
using PetsRegistration.Api.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PetsRegistration.Api.Consumers
{
    public class PetInfoRequestConsumer : IHostedService
    {
        private readonly ILogger<PetInfoRequestConsumer> _logger;
        private readonly IPetService _petService;
        private readonly IRabbitMqService _rabbitMqService;

        public PetInfoRequestConsumer(ILogger<PetInfoRequestConsumer> logger, IPetService petService, IRabbitMqService rabbitMqService)
        {
            _logger = logger;
            _petService = petService;
            _rabbitMqService = rabbitMqService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "pet_info_request", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Encoding.UTF8.GetString(body);
                var petInfoRequest = JsonSerializer.Deserialize<PetInfoRequest>(message);

                if (petInfoRequest != null)
                {
                    await HandlePetInfoRequest(petInfoRequest);
                }
            };

            channel.BasicConsume(queue: "pet_info_request", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Implementar lógica de parada, se necessário
            return Task.CompletedTask;
        }

        private async Task HandlePetInfoRequest(PetInfoRequest petInfoRequest)
        {
            var pet = await _petService.GetPetByIdAsync(petInfoRequest.PetId);
            if (pet != null)
            {
                var petInfoResponse = new PetInfoResponse
                {
                    PetId = pet.Id,
                    Name = pet.Name,
                    Species = pet.Species,
                    Breed = pet.Breed,
                    Age = pet.Age,
                    ImageUrl = pet.ImageUrl
                };

                _rabbitMqService.Publish(petInfoResponse, "pet_info_response");
            }
        }
    }
}