using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartPlantCareApi.Models
{
    public class Plant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("species")]
        public string Species { get; set; } = string.Empty;

        [BsonElement("lastWatered")]
        public DateTime LastWatered { get; set; } = DateTime.UtcNow;

        [BsonElement("wateringFrequency")]
        public int WateringFrequencyDays { get; set; } = 7;

        [BsonElement("sunlight")]
        public string Sunlight { get; set; } = string.Empty;

        [BsonElement("location")]
        public string Location { get; set; } = string.Empty;

        [BsonElement("notes")]
        public string Notes { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("lastFertilized")]
        public DateTime? LastFertilized { get; set; }

        [BsonElement("fertilizingFrequency")]
        public int FertilizingFrequencyDays { get; set; } = 30;

        [BsonElement("imageUrl")]
        public string? ImageUrl { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;
    }

    public class PlantCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public int WateringFrequencyDays { get; set; } = 7;
        public string Sunlight { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int FertilizingFrequencyDays { get; set; } = 30;
    }

    public class PlantUpdateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public int WateringFrequencyDays { get; set; } = 7;
        public string Sunlight { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int FertilizingFrequencyDays { get; set; } = 30;
    }
}
