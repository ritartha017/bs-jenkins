using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Domain.Enums;
using Endava.BookSharing.Infrastructure.Persistence.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Endava.BookSharing.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly IRoleRepository roleRepository;
    private readonly IUserRepository userRepository;
    private readonly SignInManager<UserDb> signInManager;
    private readonly UserManager<UserDb> userManager;

    public IdentityService(IRoleRepository roleRepository,
        IUserRepository userRepository,
        SignInManager<UserDb> signInManager,
        UserManager<UserDb> userManager)
    {
        this.roleRepository = roleRepository;
        this.userRepository = userRepository;
        this.signInManager = signInManager;
        this.userManager = userManager;
    }

    public async Task<bool> IsRoleAssigned(string id, Roles role, CancellationToken cancellationToken)
    {
        var userDbById = await userManager.Users.FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
        if (userDbById is null)
        {
            throw new BookSharingNotFoundException("Entity user was not found.");
        }

        var roles = await userManager.GetRolesAsync(userDbById);
        if (roles.Contains(role.ToString()))
        {
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateRole(string id, Roles role, CancellationToken cancellationToken)
    {
        var userDbById = await userManager.Users.FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
        if (userDbById is null)
        {
            throw new BookSharingNotFoundException("Entity user was not found.");
        }

        var isRoleExists = await roleRepository.ExistsAsync(role, cancellationToken);
        if (isRoleExists is false)
        {
            await roleRepository.CreateAsync(role, cancellationToken);
        }

        await userManager.RemoveFromRoleAsync(userDbById, Roles.Admin.ToString());
        await userManager.RemoveFromRoleAsync(userDbById, Roles.User.ToString());

        var result = await userRepository.AddUserToRoleAsync(userDbById.UserName, role, cancellationToken);

        return result;
    }

    public async Task<bool> AddUserToRole(User user, Roles role, CancellationToken cancellationToken)
    {
        var isRoleExists = await roleRepository.ExistsAsync(role, cancellationToken);

        if (isRoleExists is false)
        {
            await roleRepository.CreateAsync(role, cancellationToken);
        }

        var result = await userRepository.AddUserToRoleAsync(user.UserName, role, cancellationToken);

        return result;
    }

    public async Task CreateRoleIfNotExists(Roles role, CancellationToken cancellationToken)
    {
        var isExists = await roleRepository.ExistsAsync(role, cancellationToken);

        if (isExists is false)
        {
            await roleRepository.CreateAsync(role, cancellationToken);
        }
    }

    public async Task<UserRolesDto?> ValidateUserCredentialsAsync(string username, string password, CancellationToken cancellationToken)
    {
        var result = await userRepository.ExistsUserNameAsync(username, cancellationToken);
        if (result is false)
        {
            return null;
        }
        var appUser = await userManager.FindByNameAsync(username);

        var checkResult = await signInManager.CheckPasswordSignInAsync(appUser, password, false);
        if (!checkResult.Succeeded)
        {
            return null;
        }

        var roles = await userManager.GetRolesAsync(appUser);

        return new UserRolesDto(appUser.Id, roles);
    }

    public async Task<bool> VerifyResetPasswordHash(string email, string hash, CancellationToken cancellationToken)
    {
        var user = userManager.Users.SingleOrDefault(u => u.Email == email);

        if (user is null)
        {
            return false;
        }

        const string passwordResetTokenProvider = "Default";
        const string resetPasswordTokenPurpose = "ResetPassword";

        var result = await userManager.VerifyUserTokenAsync(user, passwordResetTokenProvider, resetPasswordTokenPurpose, hash);

        return result;
    }

    public async Task<bool> PasswordResetAsync(string email, string hash, string password, CancellationToken cancellationToken)
    {
        var user = userManager.Users.SingleOrDefault(u => u.Email == email);

        if (user is null)
        {
            return false;
        }

        var result = await userManager.ResetPasswordAsync(user, hash, password);

        return result.Succeeded;
    }

    public async Task<string> CreatePasswordResetToken(string email, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new BookSharingGenericException("User doesn't exist");
        }
        return await userManager.GeneratePasswordResetTokenAsync(user);
    }
}