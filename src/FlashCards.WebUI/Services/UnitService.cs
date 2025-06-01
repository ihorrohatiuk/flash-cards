using System.Net.Http.Json;
using FlashCards.Core.Application.Dtos;
using FlashCards.Core.Domain.Entities;

namespace FlashCards.WebUI.Services;

public class UnitService
{
    private IHttpClientFactory _httpClientFactory;

    public UnitService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    // 1. Create unit
    public async Task<Guid> CreateUnitAsync(FlashCardsUnitDto unitdto)
    {
        var response = await _httpClientFactory
            .CreateClient("ServerApi")
            .PostAsync("api/units/add-unit", JsonContent.Create(unitdto));

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Failed to add flashcards: {error}");
        }
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = System.Text.Json.JsonSerializer.Deserialize<AddUnitResponseDto>(responseContent);

        if (result == null || string.IsNullOrEmpty(result.UnitId.ToString()))
            throw new InvalidOperationException("Invalid response from server.");

        return result.UnitId;
    }
    
    // 2. Get unit
    // 3. Get units by owner
    // 2. Update unit
    // 3. Delete unit
}