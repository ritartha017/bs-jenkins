using Endava.BookSharing.Application;
using Endava.BookSharing.Application.BookManagement.ListBooks;
using Endava.BookSharing.Application.Models.Requests;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.BookManagement.ListBooks;

public class ListAvailableBooksQueryTests
{
    [Fact]
    public void ListAvailableBooksQuery_Constructor_PageShouldBeOneWhenRequestPageNotSpeficied()
    {
        var expectedPage = 1;
        var request = new FilterBooksRequest();
        var query = new ListAvailableBooksQuery(request);

        Assert.Equal(expectedPage, query.Page);
    }

    [Fact]
    public void ListAvailableBooksQuery_Constructor_PageShouldBeWhatIsSpeficied()
    {
        var expectedPage = 3;
        var request = new FilterBooksRequest()
        {
            Page = expectedPage
        };
        var query = new ListAvailableBooksQuery(request);

        Assert.Equal(expectedPage, query.Page);
    }

    [Fact]
    public void ListAvailableBooksQuery_Constructor_RatingShouldZeroAndFiveWhenBothNull()
    {
        var minRating = AppConsts.MinReviewRating;
        var maxRating = AppConsts.MaxReviewRating;

        var request = new FilterBooksRequest();
        var query = new ListAvailableBooksQuery(request);

        Assert.Equal(minRating, query.RatingMin);
        Assert.Equal(maxRating, query.RatingMax);
        Assert.False(query.IsRatingSpecified);
    }

    [Fact]
    public void ListAvailableBooksQuery_Constructor_RatingShouldGiveFiveForMaxWhenNullAndThreeForMinWhenSpecified()
    {
        var minRating = 3;
        var maxRating = 5;

        var request = new FilterBooksRequest()
        {
            RatingMin = minRating
        };
        var query = new ListAvailableBooksQuery(request);

        Assert.Equal(minRating, query.RatingMin);
        Assert.Equal(maxRating, query.RatingMax);
        Assert.True(query.IsRatingSpecified);
    }

    [Fact]
    public void ListAvailableBooksQuery_Constructor_RatingShouldGiveFourForMaxWhenSpecifiedAndZeroForMinWhenNull()
    {
        var minRating = AppConsts.MinReviewRating;
        var maxRating = 4;

        var request = new FilterBooksRequest()
        {
            RatingMax = maxRating
        };
        var query = new ListAvailableBooksQuery(request);

        Assert.Equal(minRating, query.RatingMin);
        Assert.Equal(maxRating, query.RatingMax);
        Assert.True(query.IsRatingSpecified);
    }
    [Fact]
    public void ListAvailableBooksQuery_Constructor_GenresShouldBeEmptyWhenGivenNull()
    {
        var request = new FilterBooksRequest();
        var query = new ListAvailableBooksQuery(request);

        Assert.Empty(query.Genres);
    }
    [Fact]
    public void ListAvailableBooksQuery_Constructor_GenresShouldCountTwoWhenGivenStringOfTwoGenres()
    {
        string genres = "1,2";
        var expectedCount = 2;
        var request = new FilterBooksRequest()
        {
            Genres = genres
        };
        var query = new ListAvailableBooksQuery(request);

        Assert.Equal(expectedCount, query.Genres.Count);
    }
}
