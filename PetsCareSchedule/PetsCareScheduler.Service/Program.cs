using Microsoft.OpenApi.Models;
using PetsCareScheduler.Service.Interfaces;
using PetsCareScheduler.Service.Repositories;
using PetsCareScheduler.Service.Services;
using PetsCareScheduler.Service.Settings;
using PetsCareScheduler.Service.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(nameof(MongoDbSettings)));
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection(nameof(RabbitMqSettings)));

builder.Services.AddSingleton<IScheduleRepository, ScheduleRepository>();
builder.Services.AddSingleton<ScheduleService>();

builder.Services.AddHostedService<PetCreatedConsumer>();
builder.Services.AddHostedService<PetInfoRequestConsumer>();
builder.Services.AddHostedService<PetInfoResponseConsumer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "PetsCareScheduler API", 
        Version = "v1",
        Description = "API for managing pet care schedules",
        Contact = new OpenApiContact
        {
            Name = "Karine Ribeiro",
            Email = "karine.ribeiro@gft.com",
            Url = new Uri("https://www.github.com/karineyasmin")
        }
    });
    c.EnableAnnotations(); 
});

var app = builder.Build();

// Enable Swagger for all environments
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetsCareScheduler API v1");
    c.RoutePrefix = string.Empty; // Set the Swagger UI at the root
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();