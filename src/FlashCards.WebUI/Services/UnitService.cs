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
    
    public async Task<FlashCardsUnitDto> GetUnitByIdAsync(Guid unitId)
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
    
    public async Task<List<FlashCardsUnitInfoDto>> GetUnitHeadersByOwnerIdAsync(Guid ownerId)
    {
        var response = await _httpClientFactory
            .CreateClient("ServerApi")
            .GetAsync($"api/units/GetByOwner/{ownerId}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<FlashCardsUnitInfoDto>();
            }

            throw new InvalidOperationException($"Failed load units: {error}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var unitResponseDto = JsonConvert.DeserializeObject<List<FlashCardsUnitInfoDto>>(responseContent);

        return unitResponseDto;
    }
    
    public async Task UpdateUnitAsync(FlashCardsUnitDto unitdto)
    {
        var response = await _httpClientFactory
            .CreateClient("ServerApi")
            .PostAsync($"api/units/{unitdto.FlashCardsUnit.Id}/update", JsonContent.Create(unitdto));

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Failed to update unit: {error}");
        }
    }
    
    public async Task DeleteUnitAsync(Guid unitId)
    {
        var response = await _httpClientFactory
            .CreateClient("ServerApi")
            .GetAsync($"api/units/{unitId}/delete");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Unable to delete a unit: {error}");
        }
    }
}