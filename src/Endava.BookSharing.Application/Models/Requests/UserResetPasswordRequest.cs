namespace Endava.BookSharing.Application.Models.Requests;

public class UserResetPasswordRequest
{
    public string Email { get; set; } = null!;
    public string Hash { get; set; } = null!;
    public string Password { get; set; } = null!;
}

