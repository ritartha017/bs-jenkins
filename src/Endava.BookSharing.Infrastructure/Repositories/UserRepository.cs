using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Domain.Enums;
using Endava.BookSharing.Infrastructure.Persistence.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Endava.BookSharing.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<UserDb> _userManager;

    public UserRepository(UserManager<UserDb> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> ExistsEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(user =>
            user.Email == email, cancellationToken);

        if (user is null)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> ExistsUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(user =>
            user.UserName == userName, cancellationToken);

        if (user is null)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> AddUserToRoleAsync(string userName, Roles role, CancellationToken cancellationToken)
    {
        var appUser = await _userManager.Users.FirstOrDefaultAsync(x =>
            x.UserName == userName, cancellationToken);

        if (appUser is null)
        {
            return false;
        }

        var result = await _userManager.AddToRoleAsync(appUser, role.ToString());

        return result.Succeeded;
    }

    public async Task<bool> CreateAsync(User user, string clearPassword, CancellationToken cancellationToken)
    {
        var appUser = new UserDb
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName
        };

        var userCreateResult = await _userManager.CreateAsync(appUser, clearPassword);

        return userCreateResult.Succeeded;
    }

    public async Task<User?> GetByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var userDb = await _userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(author => author.Id == userId, cancellationToken);

        if (userDb is null)
        {
            return null;
        }

        var user = new User
        {
            Id = userDb.Id,
            FirstName = userDb.FirstName,
            LastName = userDb.LastName,
            Email = userDb.Email,
            UserName = userDb.UserName
        };

        return user;
    }
}