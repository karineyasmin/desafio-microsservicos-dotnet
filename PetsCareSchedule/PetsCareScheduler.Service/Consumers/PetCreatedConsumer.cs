using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PetsCareScheduler.Service.DTOs;
using PetsCareScheduler.Service.Services;
using PetsCareScheduler.Service.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace PetsCareScheduler.Service.Consumers
{
    public class PetCreatedConsumer : BackgroundService
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly IServiceProvider _serviceProvider;

        public PetCreatedConsumer(IOptions<RabbitMqSettings> rabbitMqSettings, IServiceProvider serviceProvider)
        {
            var factory = new ConnectionFactory()
            {
                HostName = rabbitMqSettings.Value.HostName,
                UserName = rabbitMqSettings.Value.UserName,
                Password = rabbitMqSettings.Value.Password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                queue: "pet_created", // Certifique-se de que o nome da fila está correto
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
                var pet = JsonConvert.DeserializeObject<PetDto>(message);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var scheduleService = scope.ServiceProvider.GetRequiredService<ScheduleService>();
                    await scheduleService.CreateAutomaticAppointmentAsync(pet);
                }
            };

            _channel.BasicConsume(queue: "pet_created", autoAck: true, consumer: consumer); // Certifique-se de que o nome da fila está correto
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