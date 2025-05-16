using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartPlantCareApi.Models
{
    public class Plant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("lastWatered")]
        public string LastWatered { get; set; } = string.Empty;

        [BsonElement("sunlight")]
        public string Sunlight { get; set; } = string.Empty;
    }
}
