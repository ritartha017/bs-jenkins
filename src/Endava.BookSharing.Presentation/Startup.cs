using Endava.BookSharing.Application.Extensions;
using Endava.BookSharing.Infrastructure.Extensions;
using Endava.BookSharing.Infrastructure.Persistence;
using Endava.BookSharing.Presentation.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Endava.BookSharing.Presentation;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureInfrastructure(Configuration);
        services.ConfigurePresentation(Configuration);
        services.ConfigureApplication(Configuration);
        services.ConfigureSwagger();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dataContext)
    {
        dataContext.Database.Migrate();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookSharing Api"));

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors(Consts.CorsPolicyName);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("BookSharing Api is running.");
            });
        });
    }
}