namespace RealEstateHub.Application.Responses;

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public IReadOnlyList<string> Errors { get; set; } = [];
    public bool IsSuccess { get; set; }

    public static ApiResponse<T> Success(T? data, string message = "Success", int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            StatusCode = statusCode,
            Message = message,
            Data = data,
            IsSuccess = true
        };
    }

    public static ApiResponse<T> Failure(string message, int statusCode = 400, IReadOnlyList<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            StatusCode = statusCode,
            Message = message,
            Errors = errors ?? [],
            IsSuccess = false
        };
    }
}
