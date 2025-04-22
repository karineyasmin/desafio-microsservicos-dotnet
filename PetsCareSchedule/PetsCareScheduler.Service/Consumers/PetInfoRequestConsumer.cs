using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PetsCareScheduler.Service.DTOs;
using PetsCareScheduler.Service.Services;
using PetsCareScheduler.Service.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace PetsCareScheduler.Service.Consumers
{
    public class PetInfoRequestConsumer : BackgroundService
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly IServiceProvider _serviceProvider;

        public PetInfoRequestConsumer(IOptions<RabbitMqSettings> rabbitMqSettings, IServiceProvider serviceProvider)
        {
            var factory = new ConnectionFactory() { HostName = rabbitMqSettings.Value.HostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                queue: "pet_info_request",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var petInfoRequest = JsonSerializer.Deserialize<PetInfoRequestDto>(message);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var scheduleService = scope.ServiceProvider.GetRequiredService<ScheduleService>();
                    await scheduleService.HandlePetInfoRequestAsync(petInfoRequest);
                }
            };

            _channel.BasicConsume(queue: "pet_info_request", autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}