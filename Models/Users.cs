using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GreenMonitor.Models
{
    public class Users
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? HashedPassword { get; set; }
        public DateTime? CreatedAt { get; set; }
        [Required]
        public string? Role { get; set; }

        public string? ProfilePictureUrl { get; set; }
    }
}