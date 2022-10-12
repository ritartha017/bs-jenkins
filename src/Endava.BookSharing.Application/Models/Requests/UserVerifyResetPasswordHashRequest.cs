namespace Endava.BookSharing.Application.Models.Requests;

public class UserVerifyResetPasswordHashRequest
{
    public string Email { get; set; } = null!;
    public string Hash { get; set; } = null!;
}

