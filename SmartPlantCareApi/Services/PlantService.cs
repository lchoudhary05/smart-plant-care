using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SmartPlantCareApi.Models;
using SmartPlantCareApi.Settings;

namespace SmartPlantCareApi.Services
{
    public class PlantService
    {
        private readonly IMongoCollection<Plant> _plantCollection;
        private readonly IMongoCollection<User> _userCollection;

        public PlantService(IOptions<PlantDatabaseSettings> plantDbSettings)
        {
            var mongoClient = new MongoClient(plantDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(plantDbSettings.Value.DatabaseName);
            _plantCollection = mongoDatabase.GetCollection<Plant>(plantDbSettings.Value.PlantCollectionName);
            _userCollection = mongoDatabase.GetCollection<User>("Users");
        }

        public async Task<List<Plant>> GetUserPlantsAsync(string userId) =>
            await _plantCollection.Find(p => p.UserId == userId && p.IsActive).ToListAsync();

        public async Task<Plant?> GetUserPlantAsync(string userId, string plantId) =>
            await _plantCollection.Find(p => p.Id == plantId && p.UserId == userId && p.IsActive).FirstOrDefaultAsync();

        public async Task<Plant?> GetAsync(string id) =>
            await _plantCollection.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task<bool> CanUserAddPlantAsync(string userId)
        {
            var user = await _userCollection.Find(u => u.Id == userId).FirstOrDefaultAsync();
            if (user == null) return false;

            var plantCount = await _plantCollection.CountDocumentsAsync(p => p.UserId == userId && p.IsActive);
            return plantCount < user.MaxPlants;
        }

        public async Task<Plant> CreateAsync(string userId, PlantCreateRequest request)
        {
            // Check if user can add more plants
            if (!await CanUserAddPlantAsync(userId))
            {
                throw new InvalidOperationException("You have reached your plant limit for your subscription tier");
            }

            var plant = new Plant
            {
                UserId = userId,
                Name = request.Name,
                Species = request.Species,
                WateringFrequencyDays = request.WateringFrequencyDays,
                Sunlight = request.Sunlight,
                Location = request.Location,
                Notes = request.Notes,
                FertilizingFrequencyDays = request.FertilizingFrequencyDays,
                LastWatered = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _plantCollection.InsertOneAsync(plant);
            return plant;
        }

        public async Task UpdateAsync(string userId, string plantId, PlantUpdateRequest request)
        {
            var plant = await GetUserPlantAsync(userId, plantId);
            if (plant == null)
                throw new InvalidOperationException("Plant not found or access denied");

            var update = Builders<Plant>.Update
                .Set(p => p.Name, request.Name)
                .Set(p => p.Species, request.Species)
                .Set(p => p.WateringFrequencyDays, request.WateringFrequencyDays)
                .Set(p => p.Sunlight, request.Sunlight)
                .Set(p => p.Location, request.Location)
                .Set(p => p.Notes, request.Notes)
                .Set(p => p.FertilizingFrequencyDays, request.FertilizingFrequencyDays);

            await _plantCollection.UpdateOneAsync(p => p.Id == plantId, update);
        }

        public async Task WaterPlantAsync(string userId, string plantId)
        {
            var plant = await GetUserPlantAsync(userId, plantId);
            if (plant == null)
                throw new InvalidOperationException("Plant not found or access denied");

            var update = Builders<Plant>.Update.Set(p => p.LastWatered, DateTime.UtcNow);
            await _plantCollection.UpdateOneAsync(p => p.Id == plantId, update);
        }

        public async Task FertilizePlantAsync(string userId, string plantId)
        {
            var plant = await GetUserPlantAsync(userId, plantId);
            if (plant == null)
                throw new InvalidOperationException("Plant not found or access denied");

            var update = Builders<Plant>.Update.Set(p => p.LastFertilized, DateTime.UtcNow);
            await _plantCollection.UpdateOneAsync(p => p.Id == plantId, update);
        }

        public async Task RemoveAsync(string userId, string plantId)
        {
            var plant = await GetUserPlantAsync(userId, plantId);
            if (plant == null)
                throw new InvalidOperationException("Plant not found or access denied");

            // Soft delete - mark as inactive
            var update = Builders<Plant>.Update.Set(p => p.IsActive, false);
            await _plantCollection.UpdateOneAsync(p => p.Id == plantId, update);
        }

        public async Task<List<Plant>> GetPlantsNeedingWaterAsync(string userId)
        {
            var today = DateTime.UtcNow.Date;
            var plants = await GetUserPlantsAsync(userId);
            
            return plants.Where(p => 
                p.LastWatered.AddDays(p.WateringFrequencyDays) <= today).ToList();
        }

        public async Task<List<Plant>> GetPlantsNeedingFertilizerAsync(string userId)
        {
            var today = DateTime.UtcNow.Date;
            var plants = await GetUserPlantsAsync(userId);
            
            return plants.Where(p => 
                p.LastFertilized == null || 
                p.LastFertilized.Value.AddDays(p.FertilizingFrequencyDays) <= today).ToList();
        }
    }
}
