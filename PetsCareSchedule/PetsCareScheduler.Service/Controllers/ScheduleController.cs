using Microsoft.AspNetCore.Mvc;
using PetsCareScheduler.Service.DTOs;
using PetsCareScheduler.Service.Models;
using PetsCareScheduler.Service.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace PetsCareScheduler.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleService _scheduleService;

        public ScheduleController(ScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        /// <summary>
        /// Creates a manual appointment for a specific care.
        /// </summary>
        /// <param name="appointmentDto">The appointment details.</param>
        /// <returns>Returns a status indicating the result of the operation.</returns>
        [HttpPost("appointment")]
        [SwaggerOperation(Summary = "Creates a manual appointment for a specific care", Description = "Creates a manual appointment for a specific care")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Appointment created successfully")]
        [SwaggerResponse(400, "Invalid data")]
        public async Task<IActionResult> CreateManualAppointment([FromBody] ManualAppointmentRequestDto appointmentDto)
        {
            await _scheduleService.CreateManualAppointmentAsync(appointmentDto);
            return Ok();
        }

        /// <summary>
        /// Gets an appointment by ID.
        /// </summary>
        /// <param name="id">The ID of the appointment.</param>
        /// <returns>Returns the appointment with the specified ID.</returns>
        [HttpGet("appointment/{id}")]
        [SwaggerOperation(Summary = "Gets an appointment by ID", Description = "Gets an appointment by ID")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Appointment retrieved successfully")]
        [SwaggerResponse(404, "Appointment not found")]
        public async Task<IActionResult> GetAppointmentById(string id)
        {
            var appointment = await _scheduleService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }

        /// <summary>
        /// Updates an appointment.
        /// </summary>
        /// <param name="appointment">The appointment with updated details.</param>
        /// <returns>Returns a status indicating the result of the operation.</returns>
        [HttpPut("appointment")]
        [SwaggerOperation(Summary = "Updates an appointment", Description = "Updates an appointment")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Appointment updated successfully")]
        [SwaggerResponse(400, "Invalid data")]
        public async Task<IActionResult> UpdateAppointment([FromBody] Appointment appointment)
        {
            await _scheduleService.UpdateAppointmentAsync(appointment);
            return Ok();
        }

        /// <summary>
        /// Deletes an appointment by ID.
        /// </summary>
        /// <param name="id">The ID of the appointment to delete.</param>
        /// <returns>Returns a status indicating the result of the operation.</returns>
        [HttpDelete("appointment/{id}")]
        [SwaggerOperation(Summary = "Deletes an appointment by ID", Description = "Deletes an appointment by ID")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Appointment deleted successfully")]
        [SwaggerResponse(404, "Appointment not found")]
        public async Task<IActionResult> DeleteAppointment(string id)
        {
            await _scheduleService.DeleteAppointmentAsync(id);
            return Ok();
        }
    }
}