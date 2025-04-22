using Microsoft.AspNetCore.Mvc;
using ExternalPetsApi.Interfaces;
using ExternalPetsApi.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace PetsRegistration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PetsInfoController : ControllerBase
{
    private readonly ICatApiService _catApiService;
    private readonly IDogApiService _dogApiService;

    public PetsInfoController(ICatApiService catApiService, IDogApiService dogApiService)
    {
        _catApiService = catApiService;
        _dogApiService = dogApiService;
    }

    /// <summary>
    /// Gets all breeds from TheCatAPI or TheDogAPI based on the species provided.
    /// </summary>
    /// <param name="species">The species of the pet (cat or dog).</param>
    /// <returns>List of breeds for the specified species.</returns>
    /// <response code="200">Returns the list of breeds.</response>
    /// <response code="400">If the species is invalid.</response>
    [HttpGet("breeds")]
    [SwaggerOperation(
        Summary = "Gets all breeds from TheCatAPI or TheDogAPI based on the species provided.",
        Description = "Retrieves a list of all breeds for the specified species (cat or dog) from TheCatAPI or TheDogAPI. Use this endpoint to discover the ID of a specific breed."
    )]
    [ProducesResponseType(typeof(IEnumerable<CatBreedDto>), 200)]
    [ProducesResponseType(typeof(IEnumerable<DogBreedDto>), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> GetAllBreeds([FromQuery] string species)
    {
        if (string.IsNullOrEmpty(species))
        {
            return BadRequest("Species is required.");
        }

        if (species.ToLower() == "cat" || species.ToLower() == "cats")
        {
            var breeds = await _catApiService.GetAllBreedsAsync();
            return Ok(breeds);
        }
        else if (species.ToLower() == "dog" || species.ToLower() == "dogs")
        {
            var breeds = await _dogApiService.GetAllBreedsAsync();
            return Ok(breeds);
        }
        else
        {
            return BadRequest("Invalid species. Only 'cat' or 'dog' are allowed.");
        }
    }
}