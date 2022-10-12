namespace Endava.BookSharing.Application.Abstract;

public interface ITokenService
{
    string CreateAuthToken(Guid id, string userName, IEnumerable<string> roles);
}