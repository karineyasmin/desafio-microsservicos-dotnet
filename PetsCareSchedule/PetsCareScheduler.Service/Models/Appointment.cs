using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PetsCareScheduler.Service.Models
{
    public class Appointment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string PetId { get; set; }
        public string PetName { get; set; } 
        public string OwnerEmail { get; set; }	
        public string OwnerName { get; set; }
        public string ServiceType { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local, DateOnly = true)]
        public DateTime Date { get; set; }

        public string Time { get; set; }
        
    }
}