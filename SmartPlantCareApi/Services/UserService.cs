using Microsoft.Extensions.Options;
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
        private readonly JwtSettings _jwtSettings;

        public UserService(IOptions<PlantDatabaseSettings> plantDbSettings, IOptions<JwtSettings> jwtSettings)
        {
            var mongoClient = new MongoClient(plantDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(plantDbSettings.Value.DatabaseName);
            _userCollection = mongoDatabase.GetCollection<User>("Users");
            _jwtSettings = jwtSettings.Value;
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
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

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
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
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