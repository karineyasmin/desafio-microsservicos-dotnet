using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PetsRegistration.Api.Models;
public class Pet

{   [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Species { get; set; }
    public string BreedId { get; set; }
    public int Age { get; set; }
    public string Sex { get; set; }
    public double Weight { get; set; }
    public string OwnerName { get; set; }
    public string OwnerEmail { get; set; }
    public string Breed { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
}