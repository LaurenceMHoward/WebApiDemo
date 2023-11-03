namespace WebApiDemo.Api;

using Serilog;

public class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog((context, logger) => { logger.ReadFrom.Configuration(context.Configuration); })
            .ConfigureWebHostDefaults(bldr =>
            {
                bldr.ConfigureAppConfiguration((_, config) => { config.AddEnvironmentVariables(); })
                    .UseStartup<Startup.Startup>();
            });

    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }
}