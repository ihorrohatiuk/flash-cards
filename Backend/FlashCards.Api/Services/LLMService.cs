using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace FlashCards.Api.Services;

public class LLMService
{
    public LLMService() { }

    //TODO: rewrite pdf parser to another class
    public string ParsPdfFile(string filePath)
    {
        try
        {
            using PdfReader reader = new  PdfReader(filePath);
            using PdfDocument pdfDoc = new PdfDocument(reader);
            int numberOfPages = pdfDoc.GetNumberOfPages();

            var sb = new StringBuilder();
            for (int i = 1; i <= numberOfPages; i++)
            {
                Console.WriteLine($"Parsing {i}...");
                string pageText = ParsePage(pdfDoc, i);
                
                sb.AppendLine(pageText);
                //SavePageTextToFile(pageText, i, filePath + ".txt");
            }
            
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception($"Parsing {filePath} failed", ex);
        }
    }
    
    private string ParsePage(PdfDocument pdfDoc, int pageNumber)
    {
        if (pageNumber < 1 || pageNumber > pdfDoc.GetNumberOfPages())
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Incorrect page number.");
        
        return PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(pageNumber));
    }
    
    private void SavePageTextToFile(string pageText, int pageNumber, string filePath)
    {
        File.AppendAllText(filePath, pageText, Encoding.UTF8);
        Console.WriteLine($"Pdf text {pageNumber} saved in: {filePath}");
    }
    
    // Ollama
    public async Task<string> MakeFlashCardsFromPdfContent(string content)
    {
        string url = "http://localhost:11434/api/generate";
        var prepromtInfo = "From this file content generate 10 flash cards in format <Qustion>:<Answer>. Respond using JSON. File content: ";
        var jsonData = $@"
                       {{
                         ""model"": ""llama2"",
                         ""prompt"":""{prepromtInfo + content}"",
                         ""format"": ""json"",
                         ""stream"": false
                       }}";
        
        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
        
        HttpResponseMessage response = await client.PostAsync(url, httpContent);
        
        return await response.Content.ReadAsStringAsync();
    }
}