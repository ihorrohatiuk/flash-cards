using System.Net.Http.Json;
using FlashCards.Core.Application.Dtos;
using Newtonsoft.Json;

namespace FlashCards.WebUI.Services;

public class UnitService
{
    private IHttpClientFactory _httpClientFactory;

    public UnitService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

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
        var result = JsonConvert.DeserializeObject<AddUnitResponseDto>(responseContent);

        if (result == null || string.IsNullOrEmpty(result.UnitId.ToString()))
            throw new InvalidOperationException("Invalid response from server.");

        return result.UnitId;
    }
    
    public async Task<FlashCardsUnitDto> GetUnitById(Guid unitId)
    {
        var response = await _httpClientFactory
            .CreateClient("ServerApi")
            .GetAsync($"api/units/{unitId}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Failed load unit: {error}");
        }
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var unitResponseDto = JsonConvert.DeserializeObject<FlashCardsUnitDto>(responseContent);

        if (unitResponseDto == null)
            throw new InvalidOperationException("Invalid response from server.");

        return unitResponseDto;
    }
    
    // 3. Get units by owner
    // 2. Update unit
    // 3. Delete unit
}