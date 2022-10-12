using Endava.BookSharing.Application.BookManagement.CreateBook;
using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Domain.Abstractions;
using FluentAssertions;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.BookManagement.CreateBook;

public class CreateBookCommandTests
{
    [Theory]
    [AutoMoqData]
    public void CreateBook_WithValidRequest_ValuesHaveBeenAssignedToFields(
        CreateBookRequest request,
        string currenUserId,
        IFileData image)
    {
        request.AuthorId = "someId";
        request.AuthorFullName = null!;
        var result = new CreateBookCommand(request, currenUserId, image);

        result.Should().BeEquivalentTo(request, options
            => options.ComparingByMembers<CreateBookRequest>().ExcludingMissingMembers());

        result.CurrentUserId.Should().Be(currenUserId);
        result.CoverData.Should().Be(image);
    }
}
