using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Presentation.Models.ApiResultModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Endava.BookSharing.Presentation.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private static Dictionary<Type, int> HttpStatusCodeMapping = new()
    {
        { typeof(BookSharingGenericException), (int)HttpStatusCode.BadRequest },
        { typeof(BookSharingInvalidCredentialsException), (int)HttpStatusCode.Unauthorized },
        { typeof(BookSharingAccessDeniedException), (int)HttpStatusCode.Forbidden },
        { typeof(BookSharingNotFoundException), (int)HttpStatusCode.NotFound },
        { typeof(BookSharingEntityAlreadyExistException), (int)HttpStatusCode.Conflict },
        { typeof(BookSharingInternalException), (int)HttpStatusCode.InternalServerError }
    };

    public void OnException(ExceptionContext context)
    {
        var apiResult = new ApiResult<object>(context.Exception is BookSharingBaseException ? context.Exception.Message : null);
        context.Result = new ObjectResult(apiResult)
        {
            StatusCode = GetStatusCode(context.Exception)
        };
        context.ExceptionHandled = true;
    }

    private static int GetStatusCode(Exception ex)
    {
        var isKnownException = HttpStatusCodeMapping.TryGetValue(ex.GetType(), out int statusCode);
        if (isKnownException) return statusCode;
        if (ex is BookSharingBaseException) return (int)HttpStatusCode.BadRequest;
        return (int)HttpStatusCode.InternalServerError;
    }
}
