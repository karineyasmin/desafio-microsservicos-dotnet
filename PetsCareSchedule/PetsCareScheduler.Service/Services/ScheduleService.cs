using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using PetsCareScheduler.Service.DTOs;
using PetsCareScheduler.Service.Models;
using PetsCareScheduler.Service.Interfaces;
using PetsCareScheduler.Service.Settings;
using Microsoft.Extensions.Options;

namespace PetsCareScheduler.Service.Services
{
    public class ScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IModel _channel;

        public ScheduleService(IScheduleRepository scheduleRepository, IOptions<RabbitMqSettings> rabbitMqSettings)
        {
            _scheduleRepository = scheduleRepository;

            var factory = new ConnectionFactory() { HostName = rabbitMqSettings.Value.HostName };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
        }

        public async Task CreateAutomaticAppointmentAsync(PetDto pet)
        {
            var appointments = new List<Appointment>();
        
            if (pet.Age < 2)
            {
                appointments.Add(new Appointment { PetId = pet.Id, PetName = pet.Name, OwnerName = pet.OwnerName, ServiceType = "Vacinação", Date = DateTime.Now.Date.AddMonths(1), Time = "10:00", OwnerEmail = pet.OwnerEmail });
            }
            else
            {
                appointments.Add(new Appointment { PetId = pet.Id, PetName = pet.Name, OwnerName = pet.OwnerName, ServiceType = "Check-up Inicial", Date = DateTime.Now.Date.AddMonths(1), Time = "10:00", OwnerEmail = pet.OwnerEmail });
            }
        
            appointments.Add(new Appointment { PetId = pet.Id, PetName = pet.Name, OwnerName = pet.OwnerName, ServiceType = "Banho e Tosa", Date = DateTime.Now.Date.AddMonths(1), Time = "11:00", OwnerEmail = pet.OwnerEmail });
        
            foreach (var appointment in appointments)
            {
                await _scheduleRepository.CreateAppointmentAsync(appointment);
                // Publicar evento appointment_created no RabbitMQ
                var appointmentCreatedEvent = new 
                { 
                    AppointmentId = appointment.Id, 
                    PetId = appointment.PetId, 
                    PetName = appointment.PetName, 
                    OwnerName = appointment.OwnerName, 
                    ServiceType = appointment.ServiceType, 
                    Date = appointment.Date.ToString("dd/MM/yyyy"), 
                    Time = appointment.Time, 
                    OwnerEmail = appointment.OwnerEmail 
                };
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(appointmentCreatedEvent));
                _channel.BasicPublish(exchange: "", routingKey: "appointment_created", basicProperties: null, body: body);
            }
        }

        public async Task HandlePetInfoRequestAsync(PetInfoRequestDto petInfoRequest)
        {
            var pet = await _scheduleRepository.GetPetByIdAsync(petInfoRequest.PetId);
            if (pet == null)
            {
                // Publicar evento pet_info_request no RabbitMQ
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(petInfoRequest));
                _channel.BasicPublish(exchange: "", routingKey: "pet_info_request", basicProperties: null, body: body);
            }
        }

        public async Task HandlePetInfoResponseAsync(PetInfoResponseDto petInfoResponse)
        {
            var appointment = new Appointment
            {
                PetId = petInfoResponse.PetId,
                ServiceType = petInfoResponse.ServiceType,
                Date = petInfoResponse.Date.Date,
                Time = petInfoResponse.Time
            };

            await _scheduleRepository.CreateAppointmentAsync(appointment);
            // Publicar evento appointment_created no RabbitMQ
            var appointmentCreatedEvent = new { AppointmentId = appointment.Id, PetId = appointment.PetId, ServiceType = appointment.ServiceType, Date = appointment.Date.ToString("dd/MM/yyyy"), Time = appointment.Time };
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(appointmentCreatedEvent));
            _channel.BasicPublish(exchange: "", routingKey: "appointment_created", basicProperties: null, body: body);
        }

        public async Task CreateManualAppointmentAsync(ManualAppointmentRequestDto appointmentDto)
        {
            var appointment = new Appointment
            {
                PetId = appointmentDto.PetId,
                ServiceType = appointmentDto.ServiceType,
                Date = DateTime.ParseExact(appointmentDto.Date, "dd/MM/yyyy", null), // Converter a data
                Time = appointmentDto.Hour,
                OwnerEmail = appointmentDto.OwnerEmail,
                OwnerName = appointmentDto.OwnerName
            };

            await _scheduleRepository.CreateAppointmentAsync(appointment);
            // Publicar evento appointment_created no RabbitMQ
            var appointmentCreatedEvent = new { AppointmentId = appointment.Id, PetId = appointment.PetId, ServiceType = appointment.ServiceType, Date = appointment.Date.ToString("dd/MM/yyyy"), Time = appointment.Time, OwnerEmail = appointment.OwnerEmail, OwnerName = appointment.OwnerName };
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(appointmentCreatedEvent));
            _channel.BasicPublish(exchange: "", routingKey: "appointment_created", basicProperties: null, body: body);
        }

        public async Task<Appointment> GetAppointmentByIdAsync(string appointmentId)
        {
            return await _scheduleRepository.GetAppointmentByIdAsync(appointmentId);
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            appointment.Date = DateTime.ParseExact(appointment.Date.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null); // Garantir que a data esteja no formato correto
            await _scheduleRepository.UpdateAppointmentAsync(appointment);
            // Publicar evento appointment_updated no RabbitMQ
            var appointmentUpdatedEvent = new { AppointmentId = appointment.Id, PetId = appointment.PetId, ServiceType = appointment.ServiceType, Date = appointment.Date.ToString("dd/MM/yyyy"), Time = appointment.Time };
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(appointmentUpdatedEvent));
            _channel.BasicPublish(exchange: "", routingKey: "appointment_updated", basicProperties: null, body: body);
        }

        public async Task DeleteAppointmentAsync(string appointmentId)
        {
            await _scheduleRepository.DeleteAppointmentAsync(appointmentId);
            // Publicar evento appointment_deleted no RabbitMQ
            var appointmentDeletedEvent = new { AppointmentId = appointmentId };
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(appointmentDeletedEvent));
            _channel.BasicPublish(exchange: "", routingKey: "appointment_deleted", basicProperties: null, body: body);
        }
    }
}