using System.Text.Json;
using PetsRegistration.Api.Interfaces;
using PetsRegistration.Api.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace PetsRegistration.Api.Consumers
{
    public class PetInfoResponseConsumer : IHostedService
    {
        private readonly ILogger<PetInfoResponseConsumer> _logger;
        private readonly IPetService _petService;

        public PetInfoResponseConsumer(ILogger<PetInfoResponseConsumer> logger, IPetService petService)
        {
            _logger = logger;
            _petService = petService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "pet_info_response", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Encoding.UTF8.GetString(body);
                var petInfoResponse = JsonSerializer.Deserialize<PetInfoResponse>(message);

                if (petInfoResponse != null)
                {
                    await HandlePetInfoResponse(petInfoResponse);
                }
            };

            channel.BasicConsume(queue: "pet_info_response", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Implementar lógica de parada, se necessário
            return Task.CompletedTask;
        }

        private async Task HandlePetInfoResponse(PetInfoResponse petInfoResponse)
        {
            // Implementar lógica para lidar com a resposta do pet_info_response
            _logger.LogInformation($"Received pet info response for PetId: {petInfoResponse.PetId}");
            // Exemplo: Atualizar o banco de dados com as informações recebidas
            var pet = await _petService.GetPetByIdAsync(petInfoResponse.PetId);
            if (pet != null)
            {
                pet.Name = petInfoResponse.Name;
                pet.Species = petInfoResponse.Species;
                pet.Breed = petInfoResponse.Breed;
                pet.Age = petInfoResponse.Age;
                pet.ImageUrl = petInfoResponse.ImageUrl;
                await _petService.UpdateAsync(pet);
            }
        }
    }
}