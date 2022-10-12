using Endava.BookSharing.Presentation.Models.ApiResultModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Endava.BookSharing.Presentation.Filters;

public class ApiResultFilter : IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult objectResult && objectResult.Value is not null)
        {
            var isSuccess = objectResult.StatusCode is (>= 200 and < 300);
            context.Result = new ObjectResult(new ApiResult<object>(isSuccess, objectResult.Value)) { StatusCode = objectResult.StatusCode };
        }
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        // Intentionally left blank
    }
}