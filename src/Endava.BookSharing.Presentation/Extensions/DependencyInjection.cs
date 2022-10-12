using Endava.BookSharing.Infrastructure.Settings;
using Endava.BookSharing.Presentation.Filters;
using Endava.BookSharing.Presentation.Helpers;
using Endava.BookSharing.Presentation.Models.Options;
using Endava.BookSharing.Presentation.Models.Requests;
using Endava.BookSharing.Presentation.Models.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Endava.BookSharing.Presentation.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection ConfigurePresentation(this IServiceCollection services, IConfiguration configuration)
    {
        var corsOptions = configuration.GetSection(nameof(CorsOptions)).Get<CorsOptions>();
        string[] corsOrigins = corsOptions.AccessControlAllowOrigin
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Distinct()
            .ToArray();

        services.AddCors(options =>
        {
            options.AddPolicy(Consts.CorsPolicyName,
                builder => builder.WithOrigins(corsOrigins).AllowAnyHeader()
                .AllowAnyMethod().AllowCredentials());
        });

        var tokenSettings = configuration.GetSection(nameof(TokenSettings)).Get<TokenSettings>();
        services.AddControllers(options =>
        {
            options.Filters.Add<ExceptionFilter>();
            options.Filters.Add<ApiResultFilter>();
        });

        services.AddTransient<IValidator<BookUpdateRequest>, BookUpdateValidator>();
        services.AddTransient<JwtTokenHelper>();

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Events = new JwtBearerEvents()
            {
                OnMessageReceived = (context) =>
                {
                    context.Token = context.Request.Cookies[Consts.TokenCookieName];
                    return Task.CompletedTask;
                }
            };
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.ClaimsIssuer = tokenSettings.Issuer;
            options.Audience = tokenSettings.Audience;
            options.Validate(JwtBearerDefaults.AuthenticationScheme);
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = tokenSettings.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Secret))
            };
        });

        return services;
    }

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Endava.BookSharing",
                Version = "v1"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT containing userid claim",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            var security =
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            },
                            UnresolvedReference = true
                        },
                        new List<string>()
                    }
                };
            c.AddSecurityRequirement(security);
        });

        return services;
    }
}