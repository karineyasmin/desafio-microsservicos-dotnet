namespace PetsCareScheduler.Service.DTOs
{
    public class PetInfoResponseDto
    {
        public string PetId { get; set; }
        public string ServiceType { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
    }
}