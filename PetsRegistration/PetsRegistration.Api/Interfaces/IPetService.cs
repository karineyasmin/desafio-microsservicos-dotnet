using PetsRegistration.Api.Models;

namespace PetsRegistration.Api.Interfaces
{
    public interface IPetService
    {
        Task CreateAsync(Pet pet);
        Task<Pet> GetPetByIdAsync(string petId);
        Task<(string Name, string Description, string ImageUrl)> GetBreedInfoAsync(string species, string breedId);
        Task<IEnumerable<Pet>> SearchPetsAsync(string species, string breed, string name);
        Task UpdateAsync(Pet pet);
        Task DeleteAsync(string petId);
    }
}