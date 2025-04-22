using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using NotificationService.Models;
using Microsoft.Extensions.Logging;

namespace NotificationService.Services;

public class RabbitMQListener : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly EmailService _emailService;
    private readonly ILogger<RabbitMQListener> _logger;
    private IConnection _connection;
    private IModel _channel;

    public RabbitMQListener(IConfiguration configuration, EmailService emailService, ILogger<RabbitMQListener> logger)
    {
        _configuration = configuration;
        _emailService = emailService;
        _logger = logger;
        InitializeRabbitMQ();
    }

    private void InitializeRabbitMQ()
    {
        var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQ:HostName"] };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "appointment_created", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("Received message: {Message}", message);

            try
            {
                var appointment = JsonSerializer.Deserialize<Appointment>(message);
                await _emailService.SendEmailAsync(appointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message: {Message}", message);
            }
        };
        _channel.BasicConsume(queue: "appointment_created", autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}