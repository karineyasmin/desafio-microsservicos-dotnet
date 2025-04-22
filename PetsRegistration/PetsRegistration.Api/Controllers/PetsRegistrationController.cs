using Microsoft.AspNetCore.Mvc;
using PetsRegistration.Api.Interfaces;
using PetsRegistration.Api.Models;
using PetsRegistration.Api.Dtos;
using System.Globalization;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace PetsRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly IPetService _petService;

        public PetsController(IPetService petService)
        {
            _petService = petService;
        }

        /// <summary>
        /// Registers a new pet.
        /// </summary>
        /// <param name="createPetDto">Pet data to be registered.</param>
        /// <returns>The registered pet.</returns>
        /// <response code="201">Returns the newly created pet.</response>
        /// <response code="400">If the pet data is invalid.</response>
        /// <response code="403">If the user is not authorized to perform this action.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new pet", Description = "Creates a new pet with the provided details.")]
        [ProducesResponseType(typeof(Pet), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Pet>> Create([FromBody] CreatePetDto createPetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (createPetDto.Species.ToLower() != "cat" && createPetDto.Species.ToLower() != "dog")
            {
                return BadRequest("Invalid species. Only 'cat' or 'dog' are allowed.");
            }

            createPetDto.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(createPetDto.Name.Trim().ToLower());

            var breedInfo = await _petService.GetBreedInfoAsync(createPetDto.Species, createPetDto.BreedId);
            var pet = new Pet
            {
                Species = createPetDto.Species,
                BreedId = createPetDto.BreedId,
                Name = createPetDto.Name,
                Age = createPetDto.Age,
                Sex = createPetDto.Sex,
                Weight = createPetDto.Weight,
                OwnerName = createPetDto.OwnerName,
                OwnerEmail = createPetDto.OwnerEmail,
                Breed = breedInfo.Name,
                Description = breedInfo.Description,
                ImageUrl = breedInfo.ImageUrl
            };

            await _petService.CreateAsync(pet);
            return CreatedAtAction(nameof(Create), new { id = pet.Id }, pet);
        }

        /// <summary>
        /// Searches for registered pets based on the provided filters.
        /// </summary>
        /// <param name="species">Pet species (optional).</param>
        /// <param name="breed">Pet breed (optional).</param>
        /// <param name="name">Pet name (optional).</param>
        /// <returns>List of pets that match the filters.</returns>
        /// <response code="200">Returns the list of pets.</response>
        /// <response code="400">If the query parameters are invalid.</response>
        /// <response code="403">If the user is not authorized to perform this action.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet("search")]
        [SwaggerOperation(Summary = "Search for pets", Description = "Search for pets by species, breed, and name.")]
        [ProducesResponseType(typeof(IEnumerable<Pet>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "user,admin")]
        public async Task<ActionResult<IEnumerable<Pet>>> Search(
            [FromQuery] string species = null, 
            [FromQuery] string breed = null, 
            [FromQuery] string name = null
            )
        {
            species = species?.Trim();
            breed = breed?.Trim();
            name = name?.Trim();

            var pets = await _petService.SearchPetsAsync(species, breed, name);
            return Ok(pets);
        }

        /// <summary>
        /// Updates an existing pet.
        /// </summary>
        /// <param name="id">Pet ID.</param>
        /// <param name="updatePetDto">Updated pet data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the pet was updated successfully.</response>
        /// <response code="400">If the pet data is invalid.</response>
        /// <response code="404">If the pet was not found.</response>
        /// <response code="403">If the user is not authorized to perform this action.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing pet", Description = "Updates an existing pet with the provided details.")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdatePetDto updatePetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pet = await _petService.GetPetByIdAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            pet.Name = updatePetDto.Name;
            pet.Age = updatePetDto.Age;
            pet.Sex = updatePetDto.Sex;
            pet.Weight = updatePetDto.Weight;
            pet.OwnerName = updatePetDto.OwnerName;
            pet.OwnerEmail = updatePetDto.OwnerEmail;

            await _petService.UpdateAsync(pet);
            return NoContent();
        }

        /// <summary>
        /// Deletes an existing pet.
        /// </summary>
        /// <param name="id">Pet ID.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the pet was deleted successfully.</response>
        /// <response code="404">If the pet was not found.</response>
        /// <response code="403">If the user is not authorized to perform this action.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an existing pet", Description = "Deletes an existing pet by ID.")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var pet = await _petService.GetPetByIdAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            await _petService.DeleteAsync(id);
            return NoContent();
        }
    }
}