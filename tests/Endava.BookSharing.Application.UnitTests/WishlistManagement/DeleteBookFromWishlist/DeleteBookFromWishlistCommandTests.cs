using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Application.WishlistManagement.DeleteBookFromWishlist;
using FluentAssertions;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.WishlistManagement.DeleteBookFromWishlist;

public class DeleteBookFromWishlistCommandTests
{
    [Theory]
    [AutoMoqData]
    public void AddBookToWishlistCommand_WithValidRequest_ValuesHaveBeenAssignedToFields(
       string bookId,
       string currentUserId)
    {
        var result = new DeleteBookFromWishlistCommand(bookId, currentUserId);

        result.UserId.Should().Be(currentUserId);
        result.BookId.Should().Be(bookId);
    }
}
