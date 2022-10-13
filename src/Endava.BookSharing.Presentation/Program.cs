#pragma warning disable S1118 // Utility classes should not have public constructors

namespace Endava.BookSharing.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                .UseStartup<Startup>()
                .UseKestrel()
                .UseIISIntegration();
            });
    }
}

#pragma warning restore S1118 // Utility classes should not have public constructors