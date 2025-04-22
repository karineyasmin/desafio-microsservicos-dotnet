namespace PetsRegistration.Api.Models;

public class PetInfoResponse
{
    public string PetId { get; set; }
    public string Name { get; set; }
    public string Species { get; set; }
    public string Breed { get; set; }
    public int Age { get; set; }
    public string ImageUrl { get; set; }
}