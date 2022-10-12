using Endava.BookSharing.Presentation.Models.ExceptionMiddlewareModels;

namespace Endava.BookSharing.Presentation.Models.ApiResultModels;

public class ApiResult<T>
{   
    public ApiResult() { }

    public ApiResult(bool isSuccess, T? data, ErrorDetails? details = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = details;
    }

    public ApiResult(string? errorDetails)
    {
        IsSuccess = false;
        if (string.IsNullOrEmpty(errorDetails)) Error = null; 
        else Error = new ErrorDetails() { Details = errorDetails };
    }

    public bool IsSuccess { get; set; } = true;
    public ErrorDetails? Error { get; set; }
    public T? Data { get; set; }
}