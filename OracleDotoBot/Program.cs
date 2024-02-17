using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OracleDotoBot.Abstractions;
using OracleDotoBot.Services;
using Serilog;
using Telegram.Bot;

public class Program
{
    private static async Task Main()
    {
        var builder = new ConfigurationBuilder();
        BuildConfig(builder);
        var config = builder
            .AddUserSecrets<Program>()
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Build())
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        Log.Logger.Information("App starting");

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<ITelegramBotClient>(
                    x => new TelegramBotClient(config["Token"]));
                services.AddTransient<IResponseService, ResponseService>();
                services.AddHostedService<MessagesRecieverService>();
            })
            .UseSerilog()
            .Build();
        await host.RunAsync();
    }

    private static void BuildConfig(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables();
    }
}