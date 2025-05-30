using System.Net.Http.Json;
using FlashCards.Core.Domain.Entities;
using Microsoft.AspNetCore.Components.Forms;

namespace FlashCards.WebUI.Services;

public class AiService
{
    private IHttpClientFactory _httpClientFactory;

    public AiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<FlashCard>> GetFlashCardsAsync(IBrowserFile file, int numberOfCards)
    {
        var client = _httpClientFactory.CreateClient("ServerApi");

        var content = new MultipartFormDataContent();

        var stream = file.OpenReadStream(10 * 1024 * 1024); // TODO: move max file length validation to AiFlashCards.razor 
        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

        content.Add(fileContent, "file", file.Name);
        content.Add(new StringContent(numberOfCards.ToString()), "numberOfCards");

        var response = await client.PostAsync("api/AiFlashCards/CreateFlashCards", content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Failed to generate flashcards: {error}");
        }

        var flashCards = await response.Content.ReadFromJsonAsync<IEnumerable<FlashCard>>();

        if (flashCards == null)
            throw new InvalidOperationException("Failed to deserialize flashcards from response.");

        return flashCards;
    }
}