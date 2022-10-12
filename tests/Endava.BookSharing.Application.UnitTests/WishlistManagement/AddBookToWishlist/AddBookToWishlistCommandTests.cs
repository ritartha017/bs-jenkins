using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Application.WishlistManagement.AddBookToWishlist;
using FluentAssertions;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.WishlistManagement.AddBookToWishlist;

public class AddBookToWishlistCommandTests
{
    [Theory]
    [AutoMoqData]
    public void AddBookToWishlistCommand_WithValidRequest_ValuesHaveBeenAssignedToFields(
       string bookId,
       string currentUserId)
    {
        var result = new AddBookToWishlistCommand(bookId, currentUserId);

        result.UserId.Should().Be(currentUserId);
        result.BookId.Should().Be(bookId);
    }
}
