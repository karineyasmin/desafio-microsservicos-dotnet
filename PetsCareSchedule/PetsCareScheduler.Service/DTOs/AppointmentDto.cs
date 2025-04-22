namespace PetsCareScheduler.Service.DTOs
{
    public class AppointmentDto
    {
        public string PetId { get; set; }
        public string ServiceType { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
    }
}