namespace Endava.BookSharing.Application.Models.Requests
{
    public class UserSignInRequest
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}