using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Infrastructure.Persistence;
using Endava.BookSharing.Infrastructure.Persistence.Identity.Models;
using Endava.BookSharing.Infrastructure.Profiles;
using Endava.BookSharing.Infrastructure.Repositories;
using Endava.BookSharing.Infrastructure.Services;
using Endava.BookSharing.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Endava.BookSharing.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddTransient<ApplicationDbContext>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IRoleRepository, RoleRepository>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<ILanguageRepository, LanguageRepository>();
        services.AddTransient<IAuthorRepository, AuthorRepository>();
        services.AddTransient<IBookRepository, BookRepository>();
        services.AddTransient<IGenreRepository, GenreRepository>();
        services.AddTransient<ILanguageRepository, LanguageRepository>();
        services.AddTransient<IFileRepository, FileRepository>();
        services.AddTransient<IReviewRepository, ReviewRepository>();
        services.AddTransient<IWishlistRepository, WishlistRepository>();

        services.AddIdentity<UserDb, RoleDb>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddRoleStore<RoleStore<RoleDb, ApplicationDbContext,
                string, UserRole, IdentityRoleClaim<string>>>()
            .AddUserStore<UserStore<UserDb, RoleDb,
                ApplicationDbContext, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
                IdentityUserToken<string>, IdentityRoleClaim<string>>>()
            .AddUserManager<UserManager<UserDb>>()
            .AddRoleManager<RoleManager<RoleDb>>()
            .AddSignInManager<SignInManager<UserDb>>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 3;
            options.User.RequireUniqueEmail = false;
            options.Password.RequiredUniqueChars = 1;
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@&#*";
        });

        services.Configure<TokenSettings>(configuration.GetSection(nameof(TokenSettings)));
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(
            configuration.GetConnectionString("Postgresql")
        ));

        var passwordResetTokenSettings = configuration
            .GetSection(nameof(PasswordResetTokenSettings))
            .Get<PasswordResetTokenSettings>();

        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromMinutes(passwordResetTokenSettings.LifetimeInMinutes);
        });
        return services;
    }
}