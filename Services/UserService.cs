using GreenMonitor.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using GreenMonitor.Interfaces;

namespace GreenMonitor.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<Users> _userCollection;

        public UserService(IOptions<DatabaseSetting> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _userCollection = mongoDatabase.GetCollection<Users>(databaseSettings.Value.UserCollectionName);
        }

        public async Task<Users?> Login(LoginUser user) => await _userCollection.Find(x => x.Username == user.Username && x.HashedPassword == user.HashedPassword).FirstOrDefaultAsync();

        public async Task Register(Users user) => await _userCollection.InsertOneAsync(user);
        public async Task<Users> CheckUser(Users user) =>
        await _userCollection.Find(x => x.Username == user.Username && x.Email == user.Email).FirstOrDefaultAsync();
    }
}