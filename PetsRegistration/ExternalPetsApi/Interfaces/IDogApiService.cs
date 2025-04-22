using ExternalPetsApi.Dtos;

namespace ExternalPetsApi.Interfaces;

public interface IDogApiService
{
    Task<IEnumerable<DogBreedDto>> GetAllBreedsAsync();
    Task<string> GetImageUrlAsync(string breedId);
}