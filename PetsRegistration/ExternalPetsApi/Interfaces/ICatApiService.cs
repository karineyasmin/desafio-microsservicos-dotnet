using ExternalPetsApi.Dtos;

namespace ExternalPetsApi.Interfaces;

public interface ICatApiService
{
    Task<IEnumerable<CatBreedDto>> GetAllBreedsAsync();
    Task<string> GetImageUrlAsync(string breedId);
}