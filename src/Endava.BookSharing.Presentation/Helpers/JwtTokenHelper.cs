using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Endava.BookSharing.Presentation.Helpers;

public class JwtTokenHelper
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly TokenSettings _settings;

    public JwtTokenHelper(
        IOptions<TokenSettings> settings,
        IDateTimeProvider dateTimeProvider)
    {
        this._settings = settings.Value;
        this._dateTimeProvider = dateTimeProvider;
    } 

    public string CreateAuthToken(string userId, IEnumerable<string> roles)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            IssuedAt = _dateTimeProvider.Now,
            NotBefore = _dateTimeProvider.Now,
            Expires = _dateTimeProvider.Now.Add(TimeSpan.FromMinutes(_settings.ExpirationInMinutes)),
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(Consts.UserIdClaimName, userId),
                new Claim(Consts.RolesClaimName, string.Join(",", roles))
            })
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
