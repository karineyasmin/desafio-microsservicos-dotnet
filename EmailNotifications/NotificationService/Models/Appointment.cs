namespace NotificationService.Models;
public class Appointment
{
    public string Id { get; set; }
    public string PetId { get; set; }
    public string ServiceType { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
    public string OwnerEmail { get; set; }
    public string PetName { get; set; }

}
