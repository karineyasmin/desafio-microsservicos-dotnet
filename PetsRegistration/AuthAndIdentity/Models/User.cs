using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthAndIdentity.Models;

public class User
{   
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    
    private string _role;
    public string Role
    {
        get => _role;
        set => _role = value.ToLower();
    }
}
