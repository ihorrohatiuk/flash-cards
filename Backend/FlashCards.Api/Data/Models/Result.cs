namespace FlashCards.Api.Data.Models;

public class Result<T> where T : class
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
    
    public Result(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public Result(T data, bool success, string message)
    {
        Data = data;
        Success = success;
        Message = message;
    }
}