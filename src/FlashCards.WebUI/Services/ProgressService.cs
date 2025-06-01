using System.Net.Http.Json;

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
}