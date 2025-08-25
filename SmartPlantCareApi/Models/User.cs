using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SmartPlantCareApi.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [BsonElement("passwordHash")]
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [BsonElement("firstName")]
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [BsonElement("lastName")]
        [Required]
        public string LastName { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("lastLoginAt")]
        public DateTime? LastLoginAt { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

        [BsonElement("subscriptionTier")]
        public string SubscriptionTier { get; set; } = "Free"; // Free, Basic, Premium

        [BsonElement("maxPlants")]
        public int MaxPlants { get; set; } = 5; // Default limit for free tier
    }

    public class UserRegistrationRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;
    }

    public class UserLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserProfile User { get; set; } = new();
    }

    public class UserProfile
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string SubscriptionTier { get; set; } = string.Empty;
        public int MaxPlants { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 