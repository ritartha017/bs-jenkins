using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Presentation.Filters;
using Endava.BookSharing.Presentation.UnitTests.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Endava.BookSharing.Presentation.UnitTests.Filters
{
    public class ExceptionFiltersTest
    {
        private class BookExceptionTestClassException : BookSharingBaseException
        {
            public BookExceptionTestClassException(string message) : base(message) { }
        }

        [Fact]
        public async Task ExceptionFilterTest_Returns_Code500()
        {
            // Arrange
            var exceptionFilter = new ExceptionFilter();

            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext,
                new RouteData(),
                new ActionDescriptor(),
                new ModelStateDictionary());

            ExceptionContext exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>());

            exceptionContext.Exception = new Exception();

            // Act
            await Task.Run(() => exceptionFilter.OnException(exceptionContext));

            // Assert
            var objectResult = exceptionContext.Result as ObjectResult;

            Assert.Equal((int)System.Net.HttpStatusCode.InternalServerError, objectResult!.StatusCode);
        }

        [Fact]
        public async Task ExceptionFilterTest_Returns_Code400()
        {
            // Arrange
            var exceptionFilter = new ExceptionFilter();

            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext,
                new RouteData(),
                new ActionDescriptor(),
                new ModelStateDictionary());

            ExceptionContext exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>());

            exceptionContext.Exception = new BookExceptionTestClassException("");

            // Act
            await Task.Run(() => exceptionFilter.OnException(exceptionContext));

            // Assert
            var objectResult = exceptionContext.Result as ObjectResult;

            Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, objectResult!.StatusCode);
        }
    }
}
