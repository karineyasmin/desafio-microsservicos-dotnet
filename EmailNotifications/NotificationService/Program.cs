using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationService.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<EmailService>();
        services.AddHostedService<RabbitMQListener>();
        services.AddLogging();
    })
    .Build();

await host.RunAsync();