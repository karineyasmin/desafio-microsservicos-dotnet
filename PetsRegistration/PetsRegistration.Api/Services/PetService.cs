using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using PetsRegistration.Api.Interfaces;
using PetsRegistration.Api.Settings;
using PetsRegistration.Api.Models;
using ExternalPetsApi.Interfaces;
using System.Globalization;

namespace PetsRegistration.Api.Services
{
    public class PetService : IPetService
    {
        private readonly IMongoCollection<Pet> _pets;
        private readonly ICatApiService _catApiService;
        private readonly IDogApiService _dogApiService; 
        private readonly IRabbitMqService _rabbitMqService;

        public PetService(IOptions<MongoDbSettings> mongoDbSettings, 
            ICatApiService catApiService, 
            IDogApiService dogApiService, 
            IRabbitMqService rabbitMqService
            )
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _pets = database.GetCollection<Pet>("Pets");
            _catApiService = catApiService;
            _dogApiService = dogApiService;
            _rabbitMqService = rabbitMqService;
        }

        public async Task CreateAsync(Pet pet)
        {   
            pet.Species = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                pet.Species.ToLower()
            );

            pet.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                pet.Name.ToLower()
            );

            pet.OwnerName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                pet.OwnerName.ToLower()
            );

            // Obter informações adicionais da raça
            var breedInfo = await GetBreedInfoAsync(pet.Species, pet.BreedId);
            pet.Breed = breedInfo.Name;
            pet.Description = breedInfo.Description;
            pet.ImageUrl = breedInfo.ImageUrl;

            await _pets.InsertOneAsync(pet);
            _rabbitMqService.Publish(pet, "pet_created");
        }

        public async Task<Pet> GetPetByIdAsync(string petId)
        {
            if (!ObjectId.TryParse(petId, out var objectId))
            {
                throw new FormatException($"{petId} is not a valid 24 digit hex string.");
            }

            return await _pets.Find(p => p.Id == objectId.ToString()).FirstOrDefaultAsync();
        }

        public async Task<(string Name, string Description, string ImageUrl)> GetBreedInfoAsync(string species, string breedId)
        {
            if (species.ToLower() == "cat")
            {
                var breeds = await _catApiService.GetAllBreedsAsync();
                var breed = breeds.FirstOrDefault(b => b.Id == breedId);
                var imageUrl = await _catApiService.GetImageUrlAsync(breedId);
                return (breed?.Name, breed?.Description, imageUrl);
            }
            else
            {
                var breeds = await _dogApiService.GetAllBreedsAsync();
                var breed = breeds.FirstOrDefault(b => b.Id == int.Parse(breedId));
                var imageUrl = await _dogApiService.GetImageUrlAsync(breedId);
                return (breed?.Name, breed?.Temperament, imageUrl);
            }
        }

        public async Task<IEnumerable<Pet>> SearchPetsAsync(
            string species, 
            string breed,
            string name
            )
        {
            var filterBuilder = Builders<Pet>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrEmpty(species))
            {
                filter &= filterBuilder.Regex(p => 
                    p.Species, new MongoDB.Bson.BsonRegularExpression(species, "i"));
            }

            if (!string.IsNullOrEmpty(breed))
            {
                filter &= filterBuilder.Regex(p => 
                    p.Breed, new MongoDB.Bson.BsonRegularExpression(breed, "i"));
            }

            if (!string.IsNullOrEmpty(name))
            {
                filter &= filterBuilder.Regex(p => 
                    p.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));
            }

            return await _pets.Find(filter).ToListAsync();
        }

        public async Task UpdateAsync(Pet pet)
        {
            await _pets.ReplaceOneAsync(p => p.Id == pet.Id, pet);
        }

        public async Task DeleteAsync(string petId)
        {
            await _pets.DeleteOneAsync(p => p.Id == petId);
        }
    }
}