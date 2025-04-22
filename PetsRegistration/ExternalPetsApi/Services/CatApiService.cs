using System.Text.Json;
using ExternalPetsApi.Dtos;
using ExternalPetsApi.Interfaces;
using ExternalPetsApi.Settings;
using Microsoft.Extensions.Options;

namespace ExternalPetsApi.Services;

public class CatApiService : ICatApiService
{
    private readonly HttpClient _httpClient;
    private readonly ExternalApiSettings _settings;

    public CatApiService(HttpClient httpClient, IOptions<ExternalApiSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<IEnumerable<CatBreedDto>> GetAllBreedsAsync()
    {
        var response = await _httpClient.GetAsync("https://api.thecatapi.com/v1/breeds");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<CatBreedDto>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<string> GetImageUrlAsync(string breedId)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.thecatapi.com/v1/images/search?breed_id={breedId}");
            request.Headers.Add("x-api-key", _settings.ApiKey);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var images = JsonSerializer.Deserialize<List<CatImageDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return images?.FirstOrDefault()?.Url;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        {
            // Log the error and return a default image URL or handle it accordingly
            return "https://example.com/default-image.jpg"; // Substitua pela URL da imagem padrão
        }
    }
}
