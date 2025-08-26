using MongoDB.Driver;
using SmartPlantCareApi.Models;
using SmartPlantCareApi.Settings;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SmartPlantCareApi.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IConfiguration _configuration;

        public UserService(IConfiguration configuration)
        {
            var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING")
                ?? configuration.GetConnectionString("MongoDB");
            
            var databaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME")
                ?? configuration["MongoDB:DatabaseName"]
                ?? "SmartPlantCareDb";
            
            var userCollectionName = Environment.GetEnvironmentVariable("MONGODB_USER_COLLECTION_NAME")
                ?? configuration["MongoDB:UserCollectionName"]
                ?? "Users";
            
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _userCollection = database.GetCollection<User>(userCollectionName);
            _configuration = configuration;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _userCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> CreateAsync(UserRegistrationRequest request)
        {
            // Check if user already exists
            var existingUser = await GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists");
            }

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                SubscriptionTier = "Free",
                MaxPlants = 5
            };

            await _userCollection.InsertOneAsync(user);
            return user;
        }

        public async Task<bool> ValidatePasswordAsync(string email, string password)
        {
            var user = await GetByEmailAsync(email);
            if (user == null || !user.IsActive)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public async Task UpdateLastLoginAsync(string userId)
        {
            var update = Builders<User>.Update.Set(u => u.LastLoginAt, DateTime.UtcNow);
            await _userCollection.UpdateOneAsync(u => u.Id == userId, update);
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            // Get JWT secret from environment variable or fall back to config
            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                ?? _configuration["JwtSettings:SecretKey"];
            
            var key = Encoding.ASCII.GetBytes(secretKey!);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id!),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim("subscriptionTier", user.SubscriptionTier),
                new Claim("maxPlants", user.MaxPlants.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationMinutes"]!)),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        public async Task<bool> UpdateSubscriptionAsync(string userId, string subscriptionTier, int maxPlants)
        {
            var update = Builders<User>.Update
                .Set(u => u.SubscriptionTier, subscriptionTier)
                .Set(u => u.MaxPlants, maxPlants);

            var result = await _userCollection.UpdateOneAsync(u => u.Id == userId, update);
            return result.ModifiedCount > 0;
        }
    }
} 