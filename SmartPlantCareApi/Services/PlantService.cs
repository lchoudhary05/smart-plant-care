using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SmartPlantCareApi.Models;
using SmartPlantCareApi.Settings;

namespace SmartPlantCareApi.Services
{
    public class PlantService
    {
        private readonly IMongoCollection<Plant> _plantCollection;

        public PlantService(IOptions<PlantDatabaseSettings> plantDbSettings)
        {
            var mongoClient = new MongoClient(plantDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(plantDbSettings.Value.DatabaseName);
            _plantCollection = mongoDatabase.GetCollection<Plant>(plantDbSettings.Value.PlantCollectionName);
        }

        public async Task<List<Plant>> GetAsync() =>
            await _plantCollection.Find(_ => true).ToListAsync();

        public async Task<Plant?> GetAsync(string id) =>
            await _plantCollection.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Plant newPlant) =>
            await _plantCollection.InsertOneAsync(newPlant);

        public async Task UpdateAsync(string id, Plant updatedPlant) =>
            await _plantCollection.ReplaceOneAsync(p => p.Id == id, updatedPlant);

        public async Task RemoveAsync(string id) =>
            await _plantCollection.DeleteOneAsync(p => p.Id == id);
    }
}
