using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Domain.Enums;
using Endava.BookSharing.Infrastructure.Persistence.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Endava.BookSharing.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly RoleManager<RoleDb> _roleManager;

    public RoleRepository(RoleManager<RoleDb> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<bool> ExistsAsync(Roles role, CancellationToken cancellationToken)
    {
        var result = await _roleManager.RoleExistsAsync(role.ToString());

        return result;
    }

    public async Task<bool> CreateAsync(Roles role, CancellationToken cancellationToken)
    {
        var applicationRole = new RoleDb
        {
            Id = Guid.NewGuid().ToString(),
            Name = role.ToString()
        };

        var createRoleResult = await _roleManager.CreateAsync(applicationRole);

        return createRoleResult.Succeeded;
    }
}