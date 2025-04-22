namespace PetsCareScheduler.Service.DTOs
{
    public class ManualAppointmentRequestDto
    {
        public string PetId { get; set; }
        public string ServiceType { get; set; }
        public string Date { get; set; }
        public string Hour { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerName { get; set; }
    }
}