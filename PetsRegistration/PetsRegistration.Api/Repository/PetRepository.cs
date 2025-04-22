using MongoDB.Driver;
using PetsRegistration.Api.Interfaces;
using PetsRegistration.Api.Models;
using Microsoft.Extensions.Options;
using PetsRegistration.Api.Settings;

namespace PetsRegistration.Api.Repository
{
    public class PetRepository : IPetRepository
    {
        private readonly IMongoCollection<Pet> _pets;

        public PetRepository(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _pets = database.GetCollection<Pet>("Pets");
        }

        public async Task CreateAsync(Pet pet)
        {
            await _pets.InsertOneAsync(pet);
        }

        public async Task<IEnumerable<Pet>> SearchPetsAsync(string species, string breed)
        {
            var filter = Builders<Pet>.Filter.Eq(p => p.Species, species) & Builders<Pet>.Filter.Eq(p => p.Breed, breed);
            return await _pets.Find(filter).ToListAsync();
        }
    }
}