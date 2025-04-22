using PetsRegistration.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetsRegistration.Api.Interfaces
{
    public interface IPetRepository
    {
        Task CreateAsync(Pet pet);
        Task<IEnumerable<Pet>> SearchPetsAsync(string species, string breed);
    }
}