using Endava.BookSharing.Presentation.Filters;
using Endava.BookSharing.Presentation.Models.ApiResultModels;
using Endava.BookSharing.Presentation.UnitTests.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Endava.BookSharing.Presentation.UnitTests.Filters
{   
    public class ApiResultFilterTests
    {
        [Theory]
        [AutoMoqData]
        public async Task ApiResultFilterTest_ObjectResult_ReturnValidApiResult(DefaultHttpContext httpContext,
            string someValidActionResult)
        {
            var expected = new ObjectResult(new ApiResult<object>(true, someValidActionResult));
            var objectResult = new ObjectResult(someValidActionResult) { StatusCode = 200 };
            var resultExecutingContext = GetResourceExecutingContext(httpContext, objectResult);

            await Task.Run(() => new ApiResultFilter().OnResultExecuting(resultExecutingContext));

            Assert.NotNull(resultExecutingContext.Result);
            Assert.Equal(200, (resultExecutingContext.Result as ObjectResult)!.StatusCode);
            expected.Value.Should().BeEquivalentTo((resultExecutingContext.Result as ObjectResult)!.Value);
        }

        [Theory]
        [AutoMoqData]
        public async Task ApiResultFilterTest_OnResultExecuted_ShouldDoNothing(DefaultHttpContext httpContext)
        {
            var objectResult = new ObjectResult("") { StatusCode = 200 };

            var resultExecutingContext = GetResourceExecutingContext(httpContext, objectResult);
            await Task.Run(() => new ApiResultFilter().OnResultExecuting(resultExecutingContext));

            var resultExecutedContext = GetResourceExecutedContext(httpContext, resultExecutingContext.Result);
            await Task.Run(() => new ApiResultFilter().OnResultExecuted(resultExecutedContext));

            Assert.NotNull(resultExecutedContext.Result);
            Assert.Equal(200, (resultExecutedContext.Result as ObjectResult)!.StatusCode);
            (resultExecutedContext.Result as ObjectResult)!.Value
                .Should().BeEquivalentTo((resultExecutingContext.Result as ObjectResult)!.Value);
        }

        private static ResultExecutingContext GetResourceExecutingContext(HttpContext httpContext, IActionResult result)
        {
            return new ResultExecutingContext(
                new ActionContext(httpContext, new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>(),
                result,
                controller: new object());
        }

        private static ResultExecutedContext GetResourceExecutedContext(HttpContext httpContext, IActionResult result)
        {
            return new ResultExecutedContext(
                new ActionContext(httpContext, new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>(),
                result,
                controller: new object());
        }
    }
}
