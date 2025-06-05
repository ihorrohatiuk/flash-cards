using System.Net.Http.Json;
using FlashCards.Core.Application.Dtos;
using Newtonsoft.Json;

namespace FlashCards.WebUI.Services;

public class ProgressService
{
    private IHttpClientFactory _httpClientFactory;

    public ProgressService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task SaveCbrProgressForUnit(Guid unitId, int progress)
    {
        var response = await _httpClientFactory
            .CreateClient("ServerApi")
            .PostAsync($"api/progress/cbr/{unitId}", JsonContent.Create(progress));

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Unable to save user progress: {error}");
        }
    }
    
    public async Task<List<Sm2FlashCardDto>> GetSm2FlashCardsByUnit(Guid unitId)
    {
        var response = await _httpClientFactory
            .CreateClient("ServerApi")
            .GetAsync($"api/progress/sm2/{unitId}");

        if (!response.IsSuccessStatusCode)
        {
            return new List<Sm2FlashCardDto>();
        }

        var content = await response.Content.ReadAsStringAsync();

        var flashCards = JsonConvert.DeserializeObject<List<Sm2FlashCardDto>>(content);

        return flashCards ?? new List<Sm2FlashCardDto>();
    }

    public async Task<bool> UpdateSm2FlashCard(Guid unitId, Sm2FlashCardDto flashCardDto)
    {
        var client = _httpClientFactory.CreateClient("ServerApi");

        var response = await client.PostAsync(
            $"api/progress/sm2/{unitId}/update",
            JsonContent.Create(flashCardDto)
        );

        return response.IsSuccessStatusCode;
    }
}