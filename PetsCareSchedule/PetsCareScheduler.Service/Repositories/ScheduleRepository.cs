using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PetsCareScheduler.Service.Interfaces;
using PetsCareScheduler.Service.Models;
using PetsCareScheduler.Service.Settings;

namespace PetsCareScheduler.Service.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly IMongoCollection<Appointment> _appointments;
        private readonly IMongoCollection<Pet> _pets;

        public ScheduleRepository(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _appointments = database.GetCollection<Appointment>("Schedules");
            _pets = database.GetCollection<Pet>("Pets");
        }

        public async Task CreateAppointmentAsync(Appointment appointment)
        {
            await _appointments.InsertOneAsync(appointment);
        }

        public async Task<Pet> GetPetByIdAsync(string petId)
        {
            return await _pets.Find(p => p.Id == petId).FirstOrDefaultAsync();
        }

        public async Task<Appointment> GetAppointmentByIdAsync(string appointmentId)
        {
            return await _appointments.Find(a => a.Id == appointmentId).FirstOrDefaultAsync();
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            await _appointments.ReplaceOneAsync(a => a.Id == appointment.Id, appointment);
        }

        public async Task DeleteAppointmentAsync(string appointmentId)
        {
            await _appointments.DeleteOneAsync(a => a.Id == appointmentId);
        }
    }
}