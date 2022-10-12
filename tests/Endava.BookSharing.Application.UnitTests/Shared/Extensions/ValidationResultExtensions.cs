using FluentValidation.Results;
using System.Linq;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.Shared.Extensions
{
    public static class ValidationResultExtensions
    {
        public static void AssertSuccessValidation(this ValidationResult? result)
        {
            Assert.NotNull(result);
            Assert.True(result!.IsValid);
            Assert.Empty(result!.Errors);
        }

        public static void AssertFailedValidation(this ValidationResult? result, string message)
        {
            Assert.NotNull(result);
            Assert.False(result!.IsValid);
            string[] errors = (from r in result.Errors
                               select r.ErrorMessage).ToArray();

            Assert.Contains(message, errors);
        }

        public static void HasNotErrorMessage(this ValidationResult? result, string message)
        {
            Assert.NotNull(result);
            string[] errors = (from r in result!.Errors
                               select r.ErrorMessage).ToArray();

            Assert.DoesNotContain(message, errors);
        }
    }
}
