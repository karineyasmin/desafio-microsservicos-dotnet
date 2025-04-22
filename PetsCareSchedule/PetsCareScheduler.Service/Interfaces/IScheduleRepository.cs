using PetsCareScheduler.Service.Models;

namespace PetsCareScheduler.Service.Interfaces
{
    public interface IScheduleRepository
    {
        Task CreateAppointmentAsync(Appointment appointment);
        Task<Pet> GetPetByIdAsync(string petId);
        Task<Appointment> GetAppointmentByIdAsync(string appointmentId);
        Task UpdateAppointmentAsync(Appointment appointment);
        Task DeleteAppointmentAsync(string appointmentId);
    }
}