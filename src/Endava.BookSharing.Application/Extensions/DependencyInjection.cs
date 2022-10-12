using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Models.Mappers;
using Endava.BookSharing.Application.Models.Options;
using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Application.Models.Validators;
using Endava.BookSharing.Application.Profiles;
using Endava.BookSharing.Application.Providers.DateTime;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Endava.BookSharing.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IValidator<UserSignUpRequest>, UserSignUpValidator>();

        services.Configure<BookCoverOptions>(configuration.GetSection(nameof(BookCoverOptions)));

        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

        services.AddTransient<IModelMapper, ModelMapper>();

        services.AddFluentValidation(options =>
        {
            options.ImplicitlyValidateChildProperties = true;
            options.ImplicitlyValidateRootCollectionElements = true;
            options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        });

        var featureToggleOptions = configuration
            .GetSection(nameof(FeatureToggleOptions))
            .Get<FeatureToggleOptions>() ?? new FeatureToggleOptions();
        if (featureToggleOptions.UseMagicBytesImageValidation)
        {
            services.AddSingleton<IImageValidator, MagicBytesImageValidator>();
        }
        else
        {
            services.AddSingleton<IImageValidator, DefaultImageValidator>();
        }

        return services;
    }
}