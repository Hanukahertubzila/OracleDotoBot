using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OracleDotoBot.Abstractions.Services;
using OracleDotoBot.DAL;
using OracleDotoBot.DAL.Repositories;
using OracleDotoBot.Models;
using OracleDotoBot.Services;
using Serilog;
using Serilog.Extensions.Logging;
using System.Reflection;
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

        var heroes = config.GetSection("AllHeroes:heroes");

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Build())
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        var stratzApiLogger = new SerilogLoggerFactory(Log.Logger)
            .CreateLogger<IStratzApiService>();

        var steamApiLogger = new SerilogLoggerFactory(Log.Logger)
            .CreateLogger<ISteamApiService>();

        Log.Logger.Information("App starting");

        var botToken = config["TelegramToken"];
        var stratzBaseUrl = config["StratzBaseUrl"];
        var stratzToken = config["StratzToken"];
        var steamToken = config["SteamApiToken"];

        if (botToken == null)
        {
            Log.Logger.Error("botToken was not found");
            return;
        }

        if (stratzBaseUrl == null)
        {
            Log.Logger.Error("stratzBaseUrl was not found");
            return;
        }

        if (stratzToken == null)
        {
            Log.Logger.Error("stratzToken was not found");
            return;
        }

        if (steamToken == null)
        {
            Log.Logger.Error("steamToken was not found");
            return;
        }

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<UsersDbContext>();
                services.AddScoped<UsersRepository>();
                services.AddSingleton<ITelegramBotClient>(
                    x => new TelegramBotClient(botToken));
                services.AddSingleton<IMatchesResultService, MatchesResultService>();
                services.AddSingleton<IStratzApiService>(
                    new StratzApiService(stratzBaseUrl, stratzToken, 
                    stratzApiLogger, heroes.Get<List<Hero>>() ?? new List<Hero>()));
                services.AddSingleton<ILiveMatchesService, LiveMatchesService>();
                services.AddSingleton<IUsersService, UsersService>();
                services.AddSingleton<ISteamApiService>(
                    new SteamApiService(steamToken, heroes.Get<List<Hero>>() ?? new List<Hero>(), steamApiLogger));
                services.AddScoped<IMatchAnaliticsService, MatchAnaliticsService>();
                services.AddTransient<IResponseService, ResponseService>();
                services.AddHostedService<MessagesRecieverService>();
                services.Configure<List<Hero>>(heroes);
            })
            .UseSerilog()
            .Build();
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        await host.RunAsync();
    }

    private static void BuildConfig(IConfigurationBuilder builder)
    {
        builder//.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("heroes.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}