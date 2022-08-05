using System.Text.Json.Serialization;

namespace Shared.Dtos;

public class Response<T>
{
    public T Data { get; private set; }
    [JsonIgnore]
    public int StatusCode { get; private set; }
    public bool IsSuccess { get; private set; }
    public List<string> Errors { get; private set; }

    public static Response<T> Success(T data, int statusCode)
    {
        return new Response<T>
        {
            Data = data,
            StatusCode = statusCode,
            IsSuccess = true
        };
    }

    public static Response<T> Success(int statusCode)
    {
        return new Response<T>
        {
            Data = default(T),
            StatusCode = statusCode,
            IsSuccess = true
        };
    }

    public static Response<T> Error(List<string> errors, int statusCode)
    {
        return new Response<T>
        {
            Errors = errors,
            StatusCode = statusCode,
            IsSuccess = false
        };
    }

    public static Response<T> Error(string error, int statusCode)
    {
        return new Response<T>
        {
            Errors = new List<string>() { error },
            StatusCode = statusCode,
            IsSuccess = false
        };
    }
}
