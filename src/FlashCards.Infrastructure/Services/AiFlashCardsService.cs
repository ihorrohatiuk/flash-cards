using System.Net.Http.Headers;
using FlashCards.Core.Domain.Entities;
using FlashCards.Infrastructure.Security;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FlashCards.Infrastructure.Services;

public class AiFlashCardsService
{
    private readonly string _apiKey;
    private const string Model = "gpt-4.1-nano";
    public AiFlashCardsService(IOptions<AiServiceOptions> aiServiceOptions)
    {
        _apiKey = aiServiceOptions.Value.ApiKey;
        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new KeyNotFoundException("Ai flash cards api key not configured, check your appsettings.json file.");
        }
    }

    public async Task<IEnumerable<FlashCard>> CreateFlashCardsFromFileUsingAi(string filePath, int numberOfQuestions)
    {
        if (numberOfQuestions <= 0 || numberOfQuestions > 50)
        {
            throw new InvalidDataException($"Number of questions must be between 1 and 50, but it was {numberOfQuestions}.");
        }
        
        // 1. Upload file to api
        string fileId = await UploadFile(filePath);
        // 2. Make a request with prompt 
        string aiResponse = await SendPromptToCreateFlashCards(fileId, numberOfQuestions);
        // 3. Return generated flashcards of exception
        IEnumerable<FlashCard> flashCards = GetFlashCardsFromAiResponse(aiResponse);
        //4. Delete file 
        await DeleteFile(fileId);
        
        return flashCards;
    }
    
    private async Task<string> UploadFile(string filePath)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        using var multipartContent = new MultipartFormDataContent();
        multipartContent.Add(new StringContent("user_data"), "purpose");
        var fileStream = File.OpenRead(filePath);
        var fileContent = new StreamContent(fileStream);
        multipartContent.Add(fileContent, "file", Path.GetFileName(filePath));

        var response = await client.PostAsync("https://api.openai.com/v1/files", multipartContent);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException("Failed to upload file to AI model." + response.Content.ReadAsStringAsync().Result);
        }
        var responseString = await response.Content.ReadAsStringAsync();

        var json = JObject.Parse(responseString);
        return json["id"]?.ToString();
    }

    private async Task<string> SendPromptToCreateFlashCards(string fileId, int numberOfQuestions)
    {
        string prompt = @"I have the following PDF document. " +
            "Please generate exactly " + numberOfQuestions + " questions " +
            "and their answers in the following JSON " +
            "format:[{\"Question\": \"" +
            "<question context>\", \"Answer\": \"" +
            "<answer context>\"},{\"Question\": " +
            "\"<question context>\",\"Answer\": " +
            "\"<answer context>\"}, ...]. " +
            "Make sure the answers are short and concise, " +
            "without repeating the question. Respond strictly" +
            " in the required format. Also note that neither " +
            "the card nor the answer can be longer than 300 characters.";
        
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var requestJson = new JObject
        {
            ["model"] = Model,
            ["input"] = new JArray
            {
                new JObject
                {
                    ["role"] = "user",
                    ["content"] = new JArray
                    {
                        new JObject
                        {
                            ["type"] = "input_file",
                            ["file_id"] = fileId
                        },
                        new JObject
                        {
                            ["type"] = "input_text",
                            ["text"] = prompt
                        }
                    }
                }
            }
        };

        var content = new StringContent(requestJson.ToString());
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var response = await client.PostAsync("https://api.openai.com/v1/responses", content);
        if (!response.IsSuccessStatusCode)
        {
            await DeleteFile(fileId);
            throw new HttpRequestException("Failed to send prompt to AI model." + response.Content.ReadAsStringAsync().Result);
        }
        return await response.Content.ReadAsStringAsync();
    }

    private IEnumerable<FlashCard> GetFlashCardsFromAiResponse(string responseJson)
    {
        var root = JObject.Parse(responseJson);

        var textContent = root["output"]?[0]?["content"]?[0]?["text"]?.ToString();

        if (string.IsNullOrWhiteSpace(textContent))
            return Enumerable.Empty<FlashCard>();

        var flashCards = JsonConvert.DeserializeObject<List<FlashCard>>(textContent);

        return flashCards ?? throw new InvalidOperationException("Failed to deserialize FlashCards from AI response." + responseJson);
    }
    
    private async Task DeleteFile(string fileId)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var response = await client.DeleteAsync($"https://api.openai.com/v1/files/{fileId}");

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Failed to delete file from AI service. Status: {response.StatusCode}, Details: {errorMessage}");
        }
    }
}